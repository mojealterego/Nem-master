using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NewMaster.Core;
using NewMaster.Raids;

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
            var message = result.Type switch
            {
                RaidResultType.Success => $"+{result.Loot:N0} łupu",
                RaidResultType.Blocked when result.ShieldsConsumed > 0 => "Rajd zablokowany przez tarczę.",
                RaidResultType.Blocked => "Brak tokenu ataku.",
                _ => "Cel nie ma dostępnego łupu."
            };

            if (resultText != null)
                resultText.text = message;

            gameEngine.NotifyStateChanged();
        }

        private void Refresh(GameState state)
        {
            if (state == null)
                return;

            if (targetText != null)
                targetText.text = target.DisplayName ?? "Cel rajdu";

            if (lootText != null)
                lootText.text = $"Łup: {target.AvailableLoot:N0}";

            if (shieldText != null)
                shieldText.text = $"Tarcze: {target.ShieldCount}";

            if (tokenText != null)
                tokenText.text = $"Tokeny: {state.RaidTokens}";

            if (raidButton != null)
                raidButton.interactable = !state.IsSpinning && state.RaidTokens > 0 && target.AvailableLoot > 0;
        }
    }
}
