using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class LevelsHUD : MonoBehaviour
    {
        //reference
        public static LevelsHUD Instance = null;

        #region private fields
        [SerializeField] private GameObject _panel;
        [SerializeField] private LevelIcon[] _levelIcons;
        private int _currentIconIndex;
        #endregion private fields

        #region unity event functions
        void Awake()
        {
            Instance = this;
        }
        #endregion unity event functions

        #region public functions
        public void Show()
        {
            _panel.SetActive(true);
            transform.DOComplete();
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, GameManager.DURATION_1).SetEase(Ease.OutBounce).
                OnComplete(() => { HighlightLevel(); });
        }

        public void Hide()
        {
            transform.DOScale(Vector3.zero, GameManager.DURATION_1).SetEase(Ease.InBounce);
        }

        public void Reset()
        {
            transform.DOComplete();
            _panel.SetActive(false);
            _currentIconIndex = 0;
            foreach (var icon in _levelIcons)
            {
                icon.Reset();
            }
        }

        public void HighlightLevel()
        {
            _levelIcons[_currentIconIndex].Flash();
        }

        public void IncreaseLevel()
        {
            _currentIconIndex++;
        }
        #endregion public functions
    }
}