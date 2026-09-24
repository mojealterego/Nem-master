using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NewMaster.Core;
using NewMaster.Raids;
using NewMaster.Localization;

namespace NewMaster.UI
{
    public sealed class NewMasterRaidPanel : MonoBehaviour
    {
        [SerializeField] private GameEngine gameEngine;
        [SerializeField] private RaidTarget target = new();
        [SerializeField] private TMP_Text targetText;
        [SerializeField] private TMP_Text lootText;
        [SerializeField] private TMP_Text shieldText;
        [SerializeField] private TMP_Text tokenText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button raidButton;

        private readonly RaidGameService raidService = new();

        private void Awake()
        {
            if (gameEngine == null)
                gameEngine = FindFirstObjectByType<GameEngine>();

            raidButton?.onClick.AddListener(Raid);
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

            raidButton?.onClick.RemoveListener(Raid);
        }

        private void Raid()
        {
            if (gameEngine == null)
                return;

            var result = raidService.TryRaid(gameEngine.State, target);
            var message = NewMasterLocalization.Get(gameEngine.State.Status);

            if (resultText != null)
                resultText.text = message;

            gameEngine.NotifyStateChanged();
        }

        private void Refresh(GameState state)
        {
            if (state == null)
                return;

            if (targetText != null)
                targetText.text = target.DisplayName ?? "Raid target";

            if (lootText != null)
                lootText.text = NewMasterLocalization.Get(NewMasterTextKeys.RaidLootLabel, target.AvailableLoot);

            if (shieldText != null)
                shieldText.text = NewMasterLocalization.Get(NewMasterTextKeys.RaidShieldsLabel, target.ShieldCount);

            if (tokenText != null)
                tokenText.text = NewMasterLocalization.Get(NewMasterTextKeys.RaidTokensLabel, state.RaidTokens);

            if (raidButton != null)
                raidButton.interactable = !state.IsSpinning && state.RaidTokens > 0 && target.AvailableLoot > 0;
        }
    }
}
