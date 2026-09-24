using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NewMaster.Core;
using NewMaster.Localization;
using NewMaster.Monetization;

namespace NewMaster.UI
{
    public sealed class NewMasterBattlePassPanel : MonoBehaviour
    {
        [SerializeField] private GameEngine gameEngine;
        [SerializeField] private BattlePassDefinition definition;
        [SerializeField] private TMP_Text seasonText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private TMP_Text rewardText;
        [SerializeField] private Button claimButton;

        private readonly BattlePassService service = new();
        private BattlePassDefinition runtimeDefinition;

        private void Awake()
        {
            if (gameEngine == null)
                gameEngine = FindFirstObjectByType<GameEngine>();

            if (definition == null)
                definition = CreateRuntimeDefinition();

            claimButton?.onClick.AddListener(Claim);
        }

        private void OnEnable()
        {
            if (gameEngine != null)
                gameEngine.StateChanged += Refresh;

            Refresh(gameEngine?.State);
        }

        private void OnDisable()
        {
            if (gameEngine != null)
                gameEngine.StateChanged -= Refresh;

            claimButton?.onClick.RemoveListener(Claim);
        }

        private void OnDestroy()
        {
            if (runtimeDefinition != null)
                Destroy(runtimeDefinition);
        }

        private void Claim()
        {
            if (gameEngine == null || definition == null)
                return;

            service.TryClaimNext(definition, gameEngine.State.BattlePass, gameEngine.State);
            gameEngine.NotifyStateChanged();
        }

        private void Refresh(GameState state)
        {
            if (state == null || definition == null)
                return;

            var unlocked = service.GetUnlockedTier(definition, state.BattlePass);
            var next = FindNextTier(definition, state.BattlePass.ClaimedTier + 1);

            if (seasonText != null)
                seasonText.text = definition.seasonId;

            if (progressText != null)
                progressText.text = $"XP {state.BattlePass.Experience:N0} · Tier {unlocked}";

            if (rewardText != null)
            {
                rewardText.text = next == null
                    ? "Battle Pass complete"
                    : $"+{next.coinReward:N0} coins · +{next.energyReward} energy";
            }

            if (claimButton != null)
                claimButton.interactable = next != null && next.level <= unlocked;
        }

        private static BattlePassDefinition.Tier FindNextTier(BattlePassDefinition definition, int level)
        {
            for (var i = 0; i < definition.tiers.Count; i++)
            {
                var tier = definition.tiers[i];
                if (tier != null && tier.level == level)
                    return tier;
            }

            return null;
        }

        private BattlePassDefinition CreateRuntimeDefinition()
        {
            runtimeDefinition = ScriptableObject.CreateInstance<BattlePassDefinition>();
            runtimeDefinition.seasonId = "season-1";
            runtimeDefinition.tiers = new List<BattlePassDefinition.Tier>();

            for (var level = 1; level <= 30; level++)
            {
                runtimeDefinition.tiers.Add(new BattlePassDefinition.Tier
                {
                    level = level,
                    experienceRequired = level * 100,
                    coinReward = level * 500L,
                    energyReward = level % 5 == 0 ? 2 : 1
                });
            }

            return runtimeDefinition;
        }
    }
}
