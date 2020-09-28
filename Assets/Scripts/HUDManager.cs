using UnityEngine;

namespace Game
{
    //controls the HUD state
    public class HUDManager : MonoBehaviour
    {
        //reference
        public static HUDManager Instance = null;

        #region private fields
        private bool _refreshLevels;
        #endregion private fields

        #region unity event functions
        void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            GameEvents.StartedGame += OnStartedGame;
            GameEvents.KnivesCreated += OnKnivesCreated;
            GameEvents.KnifeFired += OnKnifeFired;
            GameEvents.SecondChangeGiven += OnSecondChangeGiven;
            GameEvents.LevelFinishedClearing += OnLevelFinishedClearing;
            GameEvents.RestartedGame += OnRestartedGame;
            GameEvents.BossDefeated += OnBossDefeated;
            GameEvents.PointsChanged += OnPointsChanged;
            GameEvents.ApplesChanged += OnApplesChanged;
        }

        private void OnDisable()
        {
            GameEvents.StartedGame -= OnStartedGame;
            GameEvents.KnivesCreated -= OnKnivesCreated;
            GameEvents.KnifeFired -= OnKnifeFired;
            GameEvents.SecondChangeGiven -= OnSecondChangeGiven;
            GameEvents.LevelFinishedClearing -= OnLevelFinishedClearing;
            GameEvents.RestartedGame -= OnRestartedGame;
            GameEvents.BossDefeated -= OnBossDefeated;
            GameEvents.PointsChanged -= OnPointsChanged;
            GameEvents.ApplesChanged -= OnApplesChanged;
        }
        #endregion unity event functions

        #region private functions
        private void OnStartedGame()
        {
            KnivesCounterHUD.Instance.Show();
            LevelsHUD.Instance.Reset(); 
            LevelsHUD.Instance.Show();
        }

        private void OnKnivesCreated(int knives)
        {
            KnivesCounterHUD.Instance.DisplayKnives(knives);
        }

        private void OnKnifeFired()
        {
            KnivesCounterHUD.Instance.UseKnife();
        }

        private void OnSecondChangeGiven()
        {
            KnivesCounterHUD.Instance.RecoverKnife();
        }

        private void OnLevelFinishedClearing()
        {
            if (_refreshLevels)
            {
                _refreshLevels = false;
                RefreshLevels();
            }
            else
            {
                LevelsHUD.Instance.IncreaseLevel();
            }
            LevelsHUD.Instance.HighlightLevel();
        }

        private void OnRestartedGame()
        {
            RefreshLevels();
        }

        private void OnBossDefeated()
        {
            _refreshLevels = true;
            LevelsHUD.Instance.Hide();
        }

        //refreshes the levels HUD
        private void RefreshLevels()
        {
            LevelsHUD.Instance.Reset();
            LevelsHUD.Instance.Show();
        }

        private void OnPointsChanged()
        {
            PointsHUD.Instance.Refresh();
        }

        private void OnApplesChanged()
        {
            ApplesHUD.Instance.Refresh();
        }
        #endregion private functions
    }
}