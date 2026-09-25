using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NewMaster.Core;
using NewMaster.Localization;

namespace NewMaster.UI
{
    public sealed class NewMasterWorldBossPanel : MonoBehaviour
    {
        [SerializeField] private GameEngine gameEngine;
        [SerializeField] private TMP_Text bossText;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text contributionText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button attackButton;
        [SerializeField] private Button claimButton;

        private void Awake()
        {
            if (gameEngine == null)
                gameEngine = FindFirstObjectByType<GameEngine>();

            attackButton?.onClick.AddListener(Attack);
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

            attackButton?.onClick.RemoveListener(Attack);
            claimButton?.onClick.RemoveListener(Claim);
        }

        private void Attack()
        {
            if (gameEngine != null && gameEngine.TryAttackWorldBoss())
                Refresh(gameEngine.State);
        }

        private void Claim()
        {
            if (gameEngine != null && gameEngine.TryClaimWorldBossReward())
                Refresh(gameEngine.State);
        }

        private void Refresh(GameState state)
        {
            if (state?.WorldBoss == null)
                return;

            var boss = state.WorldBoss;
            if (bossText != null)
                bossText.text = string.IsNullOrWhiteSpace(boss.BossId) ? "WORLD BOSS" : boss.BossId;

            if (healthText != null)
                healthText.text = $"HP {boss.Health:N0} / {boss.MaxHealth:N0}";

            if (contributionText != null)
                contributionText.text = $"Contribution {boss.PersonalContribution:N0}";

            if (resultText != null)
                resultText.text = NewMasterLocalization.Get(state.Status);

            if (attackButton != null)
                attackButton.interactable = !state.IsSpinning && state.Energy > 0 && !boss.Defeated;

            if (claimButton != null)
                claimButton.interactable = boss.Defeated && !boss.RewardClaimed;
        }
    }
}
