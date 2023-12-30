using System;
using UnityEngine;

namespace Game.Toy
{
    public class GameState : MonoBehaviour
    {
        public int GameWaves { get; private set; } = 10;
        public float SecondsPassed { get; private set; } = 0.0f;
        public bool GameInProgress { get; private set; } = false;
        public bool IsWon { get; private set; } = false;

        public int WaveCount;
        public int MinionsEarned;
        public int MinionsSpent;
        public int PlayerDamageTaken;
        public int PlayerDamageGiven;

        public event Action OnGameStarted;
        public event Action OnGameEnded;

        public void Initialise()
        {
            SecondsPassed = 0;
            WaveCount = 0;
            MinionsEarned = 0;
            MinionsSpent = 0;
            PlayerDamageGiven = 0;
            PlayerDamageTaken = 0;
            IsWon = false;

            GameInProgress = true;
            OnGameStarted?.Invoke();
        }

        private void Update()
        {
            if (GameInProgress)
            {
                SecondsPassed += Time.deltaTime;
                if (WaveCount > GameWaves)
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