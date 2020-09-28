using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

namespace Game
{
    public class KnifeStoreMenu : MonoBehaviour, ChildMenu
    {
        //reference
        public static KnifeStoreMenu Instance = null;

        #region private fields
        [SerializeField] private Transform _panel;
        [SerializeField] private CanvasGroup _raycastGroup;
        [SerializeField] private Button _unlockButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Text _costText;
        [SerializeField] private List<KnifeItem> _knives;
        private bool _isOpen;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            if(_knives.Any(k => !k.Unlocked))
            {
                _unlockButton.enabled = true;
            }
            else
            {
                _unlockButton.enabled = false;
            }

            Instance = this;
            _unlockButton.onClick.AddListener(() => {
                if (GameManager.Instance.CanUnlockKnife())
                {
                    UnlockRandomKnife();
                }
            });

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

            foreach (var knife in _knives)
            {
                if (GameManager.Instance.IsKnifeUnlocked(knife.KnifeType))
                {
                    knife.Unlock(true);
                }
            }

            _costText.text = GameManager.COST_DEFAULT_KNIFE.ToString();

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

        public void UnlockRandomKnife()
        {
            var possibleKnives = _knives.Where(k => !k.Unlocked).ToList();
            if (possibleKnives == null || possibleKnives.Count == 0)
            {
                //no knives to be unlocked
                return;
            }
            var randomKnive = possibleKnives[Random.Range(0, possibleKnives.Count)];
            randomKnive.Unlock(false);
            GameEvents.OnKnifeUnlocked(randomKnive.KnifeType);
        }

        public void RefreshSelectedKnive()
        {
            foreach (var knife in _knives)
            {
                if (knife.KnifeType != GameManager.Instance.CurrentKnife)
                {
                    knife.Unselect();
                }
            }
        }
        #endregion public functions
    }
}