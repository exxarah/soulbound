using Core.Unity.Utils;
using Game.Input;
using UnityEngine;

namespace Game
{
    public class GameContext : SceneSingleton<GameContext>
    {
        [SerializeField]
        private InputManager m_inputManager = null;
    }
}