using System;
using UnityEngine;

namespace Game.Toy
{
    public class GameState : MonoBehaviour
    {
        public int GameSeconds { get; private set; } = 20;
        public float SecondsPassed { get; private set; } = 0.0f;
        public bool GameInProgress { get; private set; } = false;
        public bool IsWon { get; private set; } = false;
        public int WaveCount;

        public event Action OnGameStarted;
        public event Action OnGameEnded;

        public void Initialise()
        {
            SecondsPassed = 0;
            WaveCount = 0;
            IsWon = false;

            GameInProgress = true;
            OnGameStarted?.Invoke();
        }

        private void Update()
        {
            if (GameInProgress)
            {
                // TODO: Revisit win conditions
                SecondsPassed += Time.deltaTime;
                if (SecondsPassed >= GameSeconds)
                {
                    IsWon = true;
                    GameInProgress = false;
                    OnGameEnded?.Invoke();
                }   
            }
        }

        public void EndGame(bool won)
        {
            IsWon = won;
            GameInProgress = false;
            OnGameEnded?.Invoke();
        }
    }
}