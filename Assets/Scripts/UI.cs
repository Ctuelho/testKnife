using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UI : MonoBehaviour
    {
        //reference
        public static UI Instance = null;

        #region private fields
        [SerializeField] private int u;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            MainMenu.Instance.Show();
        }

        private void OnEnable()
        {
            GameEvents.KnifeBroken += OnKnifeBroken;
            GameEvents.RestartedGame += OnRestartGame;
            GameEvents.SecondChangeGiven += OnSecondChanceGiven;
            GameEvents.AdTimerEnded += OnAdTimerEnded;
        }

        private void OnDisable()
        {
            GameEvents.KnifeBroken -= OnKnifeBroken;
            GameEvents.RestartedGame -= OnRestartGame;
            GameEvents.SecondChangeGiven -= OnSecondChanceGiven;
            GameEvents.AdTimerEnded -= OnAdTimerEnded;
        }
        #endregion unity event functions

        #region private functions
        private void OnKnifeBroken()
        {
            if (!GameManager.Instance.ADAvailable) MainMenu.Instance.Show();
            GameOverMenu.Instance.Show();
        }

        private void OnRestartGame()
        {
            MainMenu.Instance.Hide();
        }

        private void OnSecondChanceGiven()
        {
            MainMenu.Instance.Hide();
        }

        private void OnAdTimerEnded()
        {
            MainMenu.Instance.Show();
            GameOverMenu.Instance.Show();
        }
        #endregion private functions
    }
}