using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NewMaster.Core;
using NewMaster.Localization;

namespace NewMaster.UI
{
    public sealed class NewMasterDailyRewardPanel : MonoBehaviour
    {
        [SerializeField] private GameEngine gameEngine;
        [SerializeField] private TMP_Text streakText;
        [SerializeField] private TMP_Text rewardText;
        [SerializeField] private Button claimButton;

        private void Awake()
        {
            if (gameEngine == null)
                gameEngine = FindFirstObjectByType<GameEngine>();

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

        private void Claim()
        {
            if (gameEngine == null)
                return;

            if (gameEngine.TryClaimDailyReward())
                return;

            Refresh(gameEngine.State);
        }

        private void Refresh(GameState state)
        {
            if (state == null)
                return;

            var day = Mathf.Max(0, state.DailyReward.StreakDay);
            if (streakText != null)
                streakText.text = $"Daily streak: {day}/7";

            var preview = new DailyRewardService().Preview(state, System.DateTimeOffset.UtcNow);
            if (rewardText != null)
            {
                rewardText.text = preview.IsAvailable
                    ? $"+{preview.Coins:N0} coins · +{preview.Energy} energy"
                    : "Daily reward already claimed";
            }

            if (claimButton != null)
                claimButton.interactable = preview.IsAvailable;
        }
    }
}
