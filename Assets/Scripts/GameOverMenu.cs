using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class GameOverMenu : MonoBehaviour
    {
        public static GameOverMenu Instance = null;

        #region private fields
        [SerializeField] private Transform _panel;
        [SerializeField] private CanvasGroup _raycastGroup;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _adButton;
        private bool _restartClicked = false;
        private bool _adClicked = false;
        #endregion private fields

        #region unity event functions
        void Awake()
        {
            Instance = this;

            _restartButton.onClick.AddListener(() => {
                _restartClicked = true; 
                Hide();
            });

            _adButton.onClick.AddListener(() => {
                _adClicked = true; 
                Hide();
            });
        }
        #endregion unity event functions

        #region public functions
        public void Show()
        {
            _restartClicked = false;
            _adClicked = false;
            _adButton.gameObject.SetActive(GameManager.Instance.ADAvailable);
            _panel.transform.localScale = Vector3.zero;
            _panel.gameObject.SetActive(true);
            _panel.transform.DOScale(Vector3.one, 0.5f).
                SetEase(Ease.OutBounce).
                    OnComplete(() => { _raycastGroup.blocksRaycasts = true; });
        }

        public void Hide()
        {
            _raycastGroup.blocksRaycasts = false;
            _panel.transform.DOScale(Vector3.zero, 0.5f).
                SetEase(Ease.InBounce)
                    .OnComplete(() => 
                    {
                        if (_restartClicked)
                        {
                            //restart the game
                            GameEvents.OnRestartedGame();
                        }
                        else if (_adClicked)
                        {
                            //give one knife
                            GameEvents.OnSecondChangeGiven();
                        }

                        _panel.gameObject.SetActive(false);
                    });
        }
        #endregion public functions
    }
}