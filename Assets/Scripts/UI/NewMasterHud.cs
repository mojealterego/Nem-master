using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NewMaster.Core;

namespace NewMaster.UI
{
    public sealed class NewMasterHud : MonoBehaviour
    {
        [Header("Core")]
        [SerializeField] private GameEngine gameEngine;

        [Header("Economy")]
        [SerializeField] private TMP_Text coinsText;
        [SerializeField] private TMP_Text energyText;
        [SerializeField] private TMP_Text worldText;
        [SerializeField] private TMP_Text villageText;
        [SerializeField] private TMP_Text statusText;

        [Header("Spin")]
        [SerializeField] private TMP_Text[] slotTexts;
        [SerializeField] private Button spinButton;

        public event Action SpinRequested;

        private void Awake()
        {
            if (gameEngine == null)
                gameEngine = FindFirstObjectByType<GameEngine>();

            if (spinButton != null)
                spinButton.onClick.AddListener(RequestSpin);
        }

        private void OnEnable()
        {
            if (gameEngine != null)
                gameEngine.StateChanged += Refresh;

            Refresh(gameEngine == null ? null : gameEngine.State);
        }

        private void OnDisable()
        {
            if (gameEngine != null)
                gameEngine.StateChanged -= Refresh;

            if (spinButton != null)
                spinButton.onClick.RemoveListener(RequestSpin);
        }

        private void RequestSpin()
        {
            SpinRequested?.Invoke();
            gameEngine?.Spin();
        }

        private void Refresh(GameState state)
        {
            if (state == null)
                return;

            if (coinsText != null)
                coinsText.text = state.Coins.ToString("N0");

            if (energyText != null)
                energyText.text = state.Energy.ToString();

            if (worldText != null)
                worldText.text = $"WORLD {state.CurrentWorldId:000}";

            if (villageText != null)
                villageText.text = $"WIOSKA {state.CurrentVillageLevel}";

            if (statusText != null)
                statusText.text = state.StatusMessage;

            if (slotTexts == null)
                return;

            for (var i = 0; i < slotTexts.Length && i < state.Slots.Count; i++)
            {
                if (slotTexts[i] != null)
                    slotTexts[i].text = state.Slots[i];
            }

            if (spinButton != null)
                spinButton.interactable = !state.IsSpinning && state.Energy > 0;
        }
    }
}
