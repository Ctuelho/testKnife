using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class MainMenu : MonoBehaviour
    {
        public static MainMenu Instance = null;

        #region private fields
        [SerializeField] private Transform _panel;
        [SerializeField] private CanvasGroup _raycastGroup;
        [SerializeField] private Button _rankingButton;
        [SerializeField] private Button _knifeStoreButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;
        private bool _rankingClicked;
        private bool _knifeStoreClicked;
        private bool _playClicked;
        #endregion private fields

        #region unity event functions
        void Awake()
        {
            Instance = this;

            _rankingButton.onClick.AddListener(() => {
                _rankingClicked = true;
                ClickChildMenu();
            });

            _knifeStoreButton.onClick.AddListener(() => {
                _knifeStoreClicked = true;
                ClickChildMenu();
            });

            _playButton.onClick.AddListener(() => {
                _playClicked = true;
                Hide();
            });

            _exitButton.onClick.AddListener(() => {
                Application.Quit();
            });
        }
        #endregion unity event functions

        #region private functions
        //show a child menu
        private void ClickChildMenu() 
        {
            _raycastGroup.blocksRaycasts = false;
            if (_knifeStoreClicked)
            {
                //shows the knife store
                _knifeStoreClicked = false;
                GameEvents.OnKnifeStoreOpen();
            }
            else if (_rankingClicked)
            {
                //show the ranking
                _rankingClicked = false;
                GameEvents.OnRankingMenuOpen();
            }
        }
        #endregion private functions

        #region public functions
        public void Show()
        {
            _raycastGroup.blocksRaycasts = false;
            _rankingClicked = false;
            _knifeStoreClicked = false;
            _playClicked = false;
            _panel.transform.localScale = Vector3.zero;
            _panel.gameObject.SetActive(true);
            _panel.transform.DOScale(Vector3.one, GameManager.DURATION_1).
                SetEase(Ease.OutBounce).
                    OnComplete(() => { _raycastGroup.blocksRaycasts = true; });
        }

        public void Hide()
        {
            _raycastGroup.blocksRaycasts = false;
            if (_playClicked)
            {
                //start the game
                _playButton.gameObject.SetActive(false);
                _playClicked = false;
                GameEvents.OnStartedGame();
            }

            _panel.transform.DOScale(Vector3.zero, GameManager.DURATION_1).
                    SetEase(Ease.InBounce).OnComplete(() => { _panel.gameObject.SetActive(false); });
        }

        public void SetInteractionsEnabled()
        {
            _raycastGroup.blocksRaycasts = true;
        }

        public void DisablePlayButton()
        {
            _playButton.interactable = false;
        }

        public void EnablePlayButton()
        {
            _playButton.interactable = true;
        }
        #endregion public functions
    }
}