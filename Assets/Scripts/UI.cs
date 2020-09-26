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

        private void OnEnable()
        {
            GameEvents.KnifeBroken += OnKnifeBroken;
        }

        private void OnDisable()
        {
            GameEvents.KnifeBroken -= OnKnifeBroken;
        }
        #endregion unity event functions

        #region private functions
        private void OnKnifeBroken()
        {
            GameOverMenu.Instance.Show();
        }
        #endregion private functions
    }
}