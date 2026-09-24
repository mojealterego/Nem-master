using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NewMaster.Collections;
using NewMaster.Core;
using NewMaster.Localization;

namespace NewMaster.UI
{
    public sealed class NewMasterCollectionPanel : MonoBehaviour
    {
        [SerializeField] private GameEngine gameEngine;
        [SerializeField] private CollectionSet collection = new();
        [SerializeField] private CollectionState previewCollectionState = new();
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private TMP_Text rewardText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button completeButton;

        private readonly CollectionService collectionService = new();

        private void Awake()
        {
            if (gameEngine == null)
                gameEngine = FindFirstObjectByType<GameEngine>();

            completeButton?.onClick.AddListener(Complete);
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

            completeButton?.onClick.RemoveListener(Complete);
        }

        private void Complete()
        {
            if (gameEngine == null)
                return;

            var state = gameEngine.CollectionState ?? previewCollectionState;
            var completed = collectionService.TryComplete(collection, state, gameEngine.State);

            if (resultText != null)
                resultText.text = NewMasterLocalization.Get(gameEngine.State.Status);

            if (completed)
                completeButton.interactable = false;

            gameEngine.NotifyStateChanged();
        }

        private void Refresh(GameState state)
        {
            if (state == null)
                return;

            var required = collection.RequiredCards?.Count ?? 0;
            var owned = 0;

            for (var i = 0; i < required; i++)
            {
                if ((gameEngine.CollectionState ?? previewCollectionState).Owns(collection.RequiredCards[i]))
                    owned++;
            }

            if (titleText != null)
                titleText.text = collection.DisplayName ?? "Collection";

            if (progressText != null)
                progressText.text = NewMasterLocalization.Get(NewMasterTextKeys.CollectionCards, owned, required);

            if (rewardText != null)
                rewardText.text = NewMasterLocalization.Get(NewMasterTextKeys.CollectionReward, collection.CompletionReward);

            if (completeButton != null)
                completeButton.interactable = required > 0 && owned == required;
        }
    }
}
