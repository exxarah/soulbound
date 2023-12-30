using Core;
using Core.Unity.Utils;
using Game.Enemy;
using Game.Input;
using Game.Minions;
using Game.Toy;
using UnityEngine;

namespace Game
{
    public class GameContext : SceneSingleton<GameContext>
    {
        [Header("Global Managers")]
        [SerializeField]
        private InputManager m_inputManager = null;
        public InputManager InputManager => m_inputManager;

        [Header("Toy Managers")]
        public MinionManager MinionManager = null;
        public EnemyManager EnemyManager = null;
        public GameState GameState = new GameState();

        private void Start()
        {
            Log.RegisterSink<UnitySink>();
        }
    }
}