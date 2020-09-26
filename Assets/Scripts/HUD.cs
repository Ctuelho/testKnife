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
        #endregion private fields

        #region unity event functions
        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            GameEvents.KnivesCreated += OnKnivesCreated;
            GameEvents.KnifeFired += OnKnifeFired;
            GameEvents.SecondChangeGiven += OnSecondChangeGiven;
        }

        private void OnDisable()
        {
            GameEvents.KnivesCreated -= OnKnivesCreated;
            GameEvents.KnifeFired -= OnKnifeFired;
            GameEvents.SecondChangeGiven -= OnSecondChangeGiven;
        }
        #endregion unity event functions

        #region private functions
        private void OnKnivesCreated(int knives)
        {
            KnivesCounter.Instance.DisplayKnives(knives);
        }

        private void OnKnifeFired()
        {
            KnivesCounter.Instance.UseKnife();
        }

        private void OnSecondChangeGiven()
        {
            KnivesCounter.Instance.RecoverKnife();
        }
        #endregion private functions
    }
}