using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NewMaster.Core;
using NewMaster.Localization;

namespace NewMaster.UI
{
    public sealed class NewMasterCounterAttackPanel : MonoBehaviour
    {
        [SerializeField] private GameEngine gameEngine;
        [SerializeField] private TMP_Text targetText;
        [SerializeField] private TMP_Text bountyText;
        [SerializeField] private Button claimButton;
        [SerializeField] private TMP_Text resultText;

        private readonly CounterAttackService service = new();

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

            if (service.TryClaim(gameEngine.State.CounterAttack, gameEngine.State, new EconomyService()))
                gameEngine.NotifyStateChanged();
        }

        private void Refresh(GameState state)
        {
            if (state?.CounterAttack == null)
                return;

            if (targetText != null)
                targetText.text = state.CounterAttack.Available
                    ? state.CounterAttack.TargetPlayerId
                    : "No counter-attack";

            if (bountyText != null)
                bountyText.text = NewMasterLocalization.Get(
                    NewMasterTextKeys.RaidLootLabel,
                    service.Preview(state.CounterAttack));

            if (resultText != null)
                resultText.text = NewMasterLocalization.Get(state.Status);

            if (claimButton != null)
                claimButton.interactable = state.CounterAttack.Available && state.CounterAttack.Bounty > 0;
        }
    }
}
