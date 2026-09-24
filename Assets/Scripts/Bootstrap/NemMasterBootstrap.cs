using UnityEngine;
using NemMaster.Core;

namespace NemMaster.Bootstrap
{
    public sealed class NemMasterBootstrap : MonoBehaviour
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
