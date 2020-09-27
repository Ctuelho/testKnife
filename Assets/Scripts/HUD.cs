using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HUD : MonoBehaviour
    {
        //reference
        public static HUD Instance = null;

        #region private fields
        private bool _refreshLevels;
        #endregion private fields

        #region unity event functions
        // Start is called before the first frame update
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

        private void RefreshLevels()
        {
            LevelsHUD.Instance.Reset();
            LevelsHUD.Instance.Show();
        }
        #endregion private functions
    }
}