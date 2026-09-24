using UnityEngine;
using NewMaster.Core;

namespace NewMaster.Bootstrap
{
    public sealed class NewMasterBootstrap : MonoBehaviour
    {
        [SerializeField] private GameEngine gameEngine;

        private void Awake()
        {
            if (gameEngine == null)
                gameEngine = GetComponent<GameEngine>();

            if (gameEngine == null)
                gameEngine = gameObject.AddComponent<GameEngine>();
        }
    }
}
