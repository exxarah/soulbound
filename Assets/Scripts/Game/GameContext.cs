using System;
using Core;
using Core.Unity.Localisation;
using Core.Unity.Utils;
using Game.Data;
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

        [SerializeField]
        private Database m_database = null;
        public Database Database => m_database;

        [SerializeField]
        private LanguageData[] m_localisation = Array.Empty<LanguageData>();
        public LanguageData[] Localisation => m_localisation;

        [Header("Toy Managers")]
        public MinionManager MinionManager = null;
        public EnemyManager EnemyManager = null;
        public GameState GameState;

        private void Start()
        {
            Log.RegisterSink<UnitySink>();
        }
    }
}