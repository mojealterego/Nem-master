using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace NewMaster.Bootstrap
{
    public sealed class NewMasterInputBootstrap : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;

        private void Awake()
        {
            if (eventSystem == null)
                eventSystem = FindFirstObjectByType<EventSystem>();

            if (eventSystem == null)
            {
                var inputRoot = new GameObject("New Master EventSystem");
                eventSystem = inputRoot.AddComponent<EventSystem>();
            }

            if (eventSystem.GetComponent<InputSystemUIInputModule>() == null)
                eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
        }
    }
}
