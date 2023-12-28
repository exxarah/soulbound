using Core;
using Core.Unity.Utils;
using Game.Input;
using UnityEngine;

namespace Game
{
    public class GameContext : SceneSingleton<GameContext>
    {
        [Header("Global Managers")]
        [SerializeField]
        private InputManager m_inputManager = null;

        [Header("Toy Managers")]
        public MinionManager MinionManager = null;

        private void Start()
        {
            Log.RegisterSink<UnitySink>();
        }
    }
}