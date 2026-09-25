using TMPro;
using UnityEngine;
using NewMaster.Localization;
using NewMaster.Social;

namespace NewMaster.UI
{
    public sealed class NewMasterLeaderboardPanel : MonoBehaviour
    {
        [SerializeField] private NewMasterSocialPanel socialPanel;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text entriesText;
        [SerializeField, Min(1)] private int maxEntries = 5;

        private readonly LeaderboardService leaderboard = new();

        private void OnEnable()
        {
            if (socialPanel != null)
                socialPanel.StateChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (socialPanel != null)
                socialPanel.StateChanged -= Refresh;
        }

        public void Refresh()
        {
            if (socialPanel == null || entriesText == null)
                return;

            var state = socialPanel.PersistenceState;
            if (state?.Guild == null)
            {
                entriesText.text = NewMasterLocalization.Get(NewMasterTextKeys.SocialLeaderboardUnavailable);
                return;
            }

            var snapshot = leaderboard.BuildGuildContributionSnapshot(state.Guild, Mathf.Max(1, maxEntries));
            if (titleText != null)
                titleText.text = NewMasterLocalization.Get(NewMasterTextKeys.SocialLeaderboardTitle);

            if (snapshot.TotalEntries == 0)
            {
                entriesText.text = NewMasterLocalization.Get(NewMasterTextKeys.SocialLeaderboardEmpty);
                return;
            }

            var builder = new System.Text.StringBuilder();
            for (var i = 0; i < snapshot.Entries.Count; i++)
            {
                var entry = snapshot.Entries[i];
                builder.Append('#').Append(entry.Rank).Append("  ")
                    .Append(entry.DisplayName).Append("  ")
                    .Append(entry.Score).AppendLine();
            }
            entriesText.text = builder.ToString().TrimEnd();
        }
    }
}
