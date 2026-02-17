using UnityEngine;


namespace Deviloop
{
    public class GameStateManager : Singleton<GameStateManager>
    {
        [SerializeField] private GameObject _gameOverPage;

        public bool CanPlayerDrawLasso { get; set; }
        public bool IsInLassoingState { get; set; }

        private void Start()
        {
            Player.PlayerCombatCharacter.OnDeath += HandlePlayerDeath;
        }

        private void OnDestroy()
        {
            Player.PlayerCombatCharacter.OnDeath -= HandlePlayerDeath;
        }

        private void HandlePlayerDeath(CombatCharacter combatCharacter)
        {
            Time.timeScale = 0f;
            _gameOverPage.SetActive(true);
        }
    }
}
