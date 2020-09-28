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
        [SerializeField] private Image _timer;
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
                _timer.DOKill();
                Hide();
            });
        }
        #endregion unity event functions

        #region public functions
        public void Show()
        {
            _restartClicked = false;
            _adClicked = false;         
            if (GameManager.Instance.ADAvailable)
            {
                _adButton.gameObject.SetActive(true);
                _restartButton.gameObject.SetActive(false);
                _timer.fillAmount = 1;
                _timer.DOFillAmount(0, GameManager.DURATION_3).OnComplete(()=> 
                {
                    GameEvents.OnAdTimerEnded();
                });
            }
            else
            {
                _adButton.gameObject.SetActive(false);
                _restartButton.gameObject.SetActive(true);
            }
            _panel.transform.localScale = Vector3.zero;
            _panel.gameObject.SetActive(true);
            _panel.transform.DOScale(Vector3.one, GameManager.DURATION_1).
                SetEase(Ease.OutBounce).
                    OnComplete(() => { _raycastGroup.blocksRaycasts = true; });
        }

        public void Hide()
        {
            _raycastGroup.blocksRaycasts = false;
            if (_restartClicked)
            {
                //restart the game
                GameEvents.OnRestartedGame();
            }
            _panel.transform.DOScale(Vector3.zero, GameManager.DURATION_1).
                SetEase(Ease.InBounce)
                    .OnComplete(() => {                      
                        if (_adClicked)
                        {
                            //give one knife
                            GameEvents.OnSecondChangeGiven();
                        }

                        _panel.gameObject.SetActive(false);
                    });
        }

        public void EnableInteractions()
        {
            _raycastGroup.blocksRaycasts = true;
        }

        public void DisableInteractions()
        {
            _raycastGroup.blocksRaycasts = false;
        }
        #endregion public functions
    }
}