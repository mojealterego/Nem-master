using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NewMaster.Core;
using NewMaster.LiveOps;

namespace NewMaster.UI
{
    public sealed class NewMasterLiveOpsPanel : MonoBehaviour
    {
        [SerializeField] private GameEngine gameEngine;
        [SerializeField] private LiveEventDefinition activeEvent;
        [SerializeField] private LiveMissionDefinition[] missions;
        [SerializeField] private TMP_Text eventText;
        [SerializeField] private TMP_Text[] missionTexts;
        [SerializeField] private Button[] claimButtons;

        private readonly LiveOpsProgressService progressService = new();
        private UnityAction[] claimHandlers;

        private void Awake()
        {
            if (gameEngine == null)
                gameEngine = FindFirstObjectByType<GameEngine>();

            BindButtons();
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

            UnbindButtons();
        }

        private void BindButtons()
        {
            if (claimButtons == null)
                return;

            claimHandlers = new UnityAction[claimButtons.Length];
            for (var i = 0; i < claimButtons.Length; i++)
            {
                var index = i;
                var handler = new UnityAction(() => Claim(index));
                claimHandlers[i] = handler;
                claimButtons[i]?.onClick.AddListener(handler);
            }
        }

        private void UnbindButtons()
        {
            if (claimButtons == null || claimHandlers == null)
                return;

            for (var i = 0; i < claimButtons.Length && i < claimHandlers.Length; i++)
                claimButtons[i]?.onClick.RemoveListener(claimHandlers[i]);

            claimHandlers = null;
        }

        private void Claim(int index)
        {
            if (gameEngine == null || missions == null ||
                index < 0 || index >= missions.Length)
                return;

            var mission = missions[index];
            if (mission == null)
                return;

            var eventId = activeEvent == null ? string.Empty : activeEvent.eventId;
            if (activeEvent != null && !activeEvent.IsActive(DateTime.UtcNow))
                return;

            if (progressService.TryClaim(
                    gameEngine.State.LiveOps,
                    eventId,
                    mission.missionId,
                    mission.target,
                    mission.coinReward,
                    mission.energyReward,
                    gameEngine.State))
            {
                gameEngine.NotifyStateChanged();
            }
        }

        private void Refresh(GameState state)
        {
            if (state == null)
                return;

            var now = DateTime.UtcNow;
            var liveEvent = activeEvent != null && activeEvent.IsActive(now)
                ? activeEvent
                : null;

            var eventId = liveEvent == null ? string.Empty : liveEvent.eventId;
            if (state.LiveOps.ActiveEventId != eventId)
                progressService.ActivateEvent(state.LiveOps, eventId);

            if (eventText != null)
                eventText.text = liveEvent == null ? "No active event" : liveEvent.localizationKey;

            if (missions == null)
                return;

            for (var i = 0; i < missions.Length; i++)
            {
                var mission = missions[i];
                if (mission == null)
                    continue;

                var progress = state.LiveOps.GetOrCreate(eventId, mission.missionId);

                if (missionTexts != null && i < missionTexts.Length && missionTexts[i] != null)
                {
                    var status = progress.Claimed
                        ? "CLAIMED"
                        : $"{progress.Progress:N0}/{mission.target:N0}";
                    missionTexts[i].text = $"{mission.localizationKey} · {status}";
                }

                if (claimButtons != null && i < claimButtons.Length && claimButtons[i] != null)
                {
                    claimButtons[i].interactable =
                        liveEvent != null &&
                        !progress.Claimed &&
                        progress.Progress >= mission.target;
                }
            }
        }
    }
}
