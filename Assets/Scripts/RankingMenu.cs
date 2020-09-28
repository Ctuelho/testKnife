using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class RankingMenu : MonoBehaviour, ChildMenu
    {
        //reference
        public static RankingMenu Instance = null;

        #region private fields
        [SerializeField] private Transform _panel;
        [SerializeField] private Text _points;
        [SerializeField] private CanvasGroup _raycastGroup;
        [SerializeField] private Button _closeButton;
        private bool _isOpen;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            Instance = this;
            _closeButton.onClick.AddListener(() => { Hide(); });
        }
        #endregion unity event functions

        #region public functions
        public void Show()
        {
            GameEvents.OnChildMenuOpened(this);

            if (_isOpen)
            {
                return;
            }

            _panel.DOKill();
            _isOpen = true;
            _raycastGroup.blocksRaycasts = false;
            _panel.localScale = Vector3.zero;
            _panel.gameObject.SetActive(true);
            _points.text = GameManager.Instance.BestScore.ToString();
            
            _panel.DOScale(Vector3.one, GameManager.DURATION_1).SetEase(Ease.OutBounce).OnComplete(() => {
                _raycastGroup.blocksRaycasts = true; 
            });

        }

        public void Hide()
        {
            _panel.DOKill();
            _isOpen = false;
            _raycastGroup.blocksRaycasts = false;
            GameEvents.OnChildMenuClosed(this);
            _panel.DOScale(Vector3.zero, GameManager.DURATION_1).SetEase(Ease.OutBounce).
                OnComplete(() => { _panel.gameObject.SetActive(false); });
        }
        #endregion public functions
    }
}