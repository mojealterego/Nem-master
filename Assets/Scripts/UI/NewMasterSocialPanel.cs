using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NewMaster.Core;
using NewMaster.Social;

namespace NewMaster.UI
{
    public sealed class NewMasterSocialPanel : MonoBehaviour
    {
        [SerializeField] private GameEngine gameEngine;
        [SerializeField] private TMP_Text guildNameText;
        [SerializeField] private TMP_Text memberText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text contributionText;
        [SerializeField] private TMP_Text milestoneText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_InputField playerIdInput;
        [SerializeField] private Button contributeButton;
        [SerializeField] private Button claimMilestoneButton;

        [SerializeField] private int contributionAmount = 100;
        [SerializeField] private int milestoneStep = 1000;
        [SerializeField] private long milestoneBaseReward = 5000;

        private SocialPersistenceState state;
        private SocialPersistenceService persistence;
        private GuildCoopService guildCoop;
        private EconomyService economy;

        private void Awake()
        {
            persistence = new SocialPersistenceService();
            guildCoop = new GuildCoopService();
            economy = new EconomyService();
            state = new SocialPersistenceState();
            persistence.TryLoad(out state);

            state.Social.PlayerId = string.IsNullOrWhiteSpace(state.Social.PlayerId)
                ? "local"
                : state.Social.PlayerId;

            if (state.Guild == null)
                state.Guild = new GuildState();

            if (string.IsNullOrWhiteSpace(state.Guild.GuildId))
            {
                var guildService = new GuildService();
                guildService.TryCreate(
                    state.Guild,
                    "new-master-local",
                    "New Master",
                    state.Social.PlayerId);
            }

            if (!state.Guild.HasMember(state.Social.PlayerId))
                new GuildService().TryAddMember(state.Guild, state.Social.PlayerId);

            state.Normalize();

            contributeButton?.onClick.AddListener(Contribute);
            claimMilestoneButton?.onClick.AddListener(ClaimMilestone);
            Refresh();
        }

        private void OnDestroy()
        {
            contributeButton?.onClick.RemoveListener(Contribute);
            claimMilestoneButton?.onClick.RemoveListener(ClaimMilestone);
            Save();
        }

        private void Contribute()
        {
            var playerId = string.IsNullOrWhiteSpace(playerIdInput?.text)
                ? state.Social.PlayerId
                : playerIdInput.text.Trim();

            if (!state.Guild.HasMember(playerId))
            {
                resultText.text = "Member not found.";
                return;
            }

            var result = guildCoop.Contribute(
                state.Guild,
                playerId,
                Mathf.Max(1, contributionAmount),
                Mathf.Max(1, milestoneStep));

            if (result.ScoreAdded <= 0)
            {
                resultText.text = "Contribution rejected.";
                return;
            }

            persistence.Save(state);
            resultText.text = $"+{result.ScoreAdded} contribution";
            Refresh();
        }

        private void ClaimMilestone()
        {
            if (gameEngine == null)
            {
                resultText.text = "Game engine is not assigned.";
                return;
            }

            var milestone = Mathf.Max(1, state.Guild.CooperativeScore / Mathf.Max(1, milestoneStep));
            var preview = guildCoop.PreviewMilestoneClaim(
                state.Guild,
                milestone,
                Mathf.Max(1, milestoneStep),
                Mathf.Max(1L, milestoneBaseReward));

            if (!preview.Claimed)
            {
                resultText.text = "No new milestone.";
                return;
            }

            var granted = economy.GrantCoins(gameEngine.State, preview.Reward);
            if (granted <= 0)
            {
                resultText.text = "Reward could not be granted.";
                return;
            }

            guildCoop.CommitMilestoneClaim(state.Guild, preview.Milestone);
            gameEngine.SaveProgress();
            persistence.Save(state);
            resultText.text = $"Milestone {preview.Milestone}: +{granted} coins";
            Refresh();
        }

        private void Refresh()
        {
            if (state?.Guild == null)
                return;

            if (guildNameText != null)
                guildNameText.text = string.IsNullOrWhiteSpace(state.Guild.Name) ? "Guild" : state.Guild.Name;
            if (memberText != null)
                memberText.text = $"Members: {state.Guild.MemberIds.Count}";
            if (scoreText != null)
                scoreText.text = $"Co-op score: {state.Guild.CooperativeScore}";
            if (contributionText != null)
                contributionText.text =
                    $"Your contribution: {state.Guild.GetContribution(state.Social.PlayerId)}";

            var step = Mathf.Max(1, milestoneStep);
            var nextMilestone = (state.Guild.CooperativeScore / step) + 1;
            if (milestoneText != null)
                milestoneText.text = $"Next milestone: {nextMilestone * step}";

            if (claimMilestoneButton != null)
            {
                var currentMilestone = state.Guild.CooperativeScore / step;
                claimMilestoneButton.interactable =
                    currentMilestone > 0 &&
                    guildCoop.CanClaimMilestone(state.Guild, currentMilestone, step);
            }
        }

        private void Save()
        {
            if (state != null && persistence != null)
                persistence.Save(state);
        }
    }
}
