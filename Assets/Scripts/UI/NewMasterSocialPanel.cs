using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NewMaster.Social;

namespace NewMaster.UI
{
    public sealed class NewMasterSocialPanel : MonoBehaviour
    {
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

        private void Awake()
        {
            persistence = new SocialPersistenceService();
            guildCoop = new GuildCoopService();
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
            var milestone = Mathf.Max(1, state.Guild.CooperativeScore / Mathf.Max(1, milestoneStep));
            var result = guildCoop.TryClaimMilestone(
                state.Guild,
                milestone,
                Mathf.Max(1, milestoneStep),
                Mathf.Max(1L, milestoneBaseReward));

            if (!result.Claimed)
            {
                resultText.text = "No new milestone.";
                return;
            }

            persistence.Save(state);
            resultText.text = $"Milestone {result.Milestone}: +{result.Reward} coins";
            Refresh();
        }

        private void Refresh()
        {
            guildNameText.text = string.IsNullOrWhiteSpace(state.Guild.Name) ? "Guild" : state.Guild.Name;
            memberText.text = $"Members: {state.Guild.MemberIds.Count}";
            scoreText.text = $"Co-op score: {state.Guild.CooperativeScore}";
            contributionText.text =
                $"Your contribution: {state.Guild.GetContribution(state.Social.PlayerId)}";

            var nextMilestone = (state.Guild.CooperativeScore / Mathf.Max(1, milestoneStep)) + 1;
            milestoneText.text = $"Next milestone: {nextMilestone * Mathf.Max(1, milestoneStep)}";

            if (claimMilestoneButton != null)
                claimMilestoneButton.interactable =
                    state.Guild.CooperativeScore >= milestoneStep &&
                    !state.Guild.HasClaimedMilestone(
                        Mathf.Max(1, state.Guild.CooperativeScore / Mathf.Max(1, milestoneStep)));
        }

        private void Save()
        {
            if (state != null && persistence != null)
                persistence.Save(state);
        }
    }
}
