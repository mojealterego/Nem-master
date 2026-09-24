using TMPro;
using UnityEngine;
using NewMaster.Core;

namespace NewMaster.UI
{
    public sealed class NewMasterMasteryPanel : MonoBehaviour
    {
        [SerializeField] private GameEngine gameEngine;
        [SerializeField] private TMP_Text rankText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private TMP_Text milestoneText;
        [SerializeField] private TMP_Text perkText;

        private readonly MasteryService service = new();
        private readonly MasteryPerkService perks = new();

        private void Awake()
        {
            if (gameEngine == null)
                gameEngine = FindFirstObjectByType<GameEngine>();
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
        }

        private void Refresh(GameState state)
        {
            if (state?.Mastery == null)
                return;

            var mastery = state.Mastery;
            var rank = service.GetRank(mastery.Experience);

            if (rankText != null)
                rankText.text = $"MASTERY {rank:N0}";

            if (progressText != null)
            {
                var nextRequirement = rank >= MasteryService.MaxRank
                    ? 0
                    : service.GetExperienceForNextRank(rank);
                progressText.text = nextRequirement <= 0
                    ? $"XP {mastery.Experience:N0} · MAX"
                    : $"XP {mastery.Experience:N0} · NEXT {nextRequirement:N0}";
            }

            if (milestoneText != null)
                milestoneText.text = $"Milestones {mastery.LifetimeMilestones:N0}";

            if (perkText != null)
            {
                var coinBonus = perks.GetCoinBonusPercent(rank);
                var energyBonus = perks.GetEnergyRewardBonus(rank);
                perkText.text = $"Perks +{coinBonus}% coins · +{energyBonus} energy reward";
            }
        }
    }
}
