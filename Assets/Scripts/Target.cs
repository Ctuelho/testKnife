using UnityEngine;
using DG.Tweening;

namespace Game
{
    public class Target : MonoBehaviour
    {
        //reference
        public static Target Instance = null;

        #region private fields
        [SerializeField] private Collider2D _collider;
        private bool _spinning = false;
        private Vector3 _originalScale;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
            transform.localPosition = Vector3.up * GameManager.Y_TARGET_START;
            _collider.enabled = false;
            _originalScale = transform.localScale;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_spinning)
            {
                transform.Rotate(Vector3.forward * 90 * Time.deltaTime);
            }
            
        }
        #endregion unity event functions

        #region private functions   
        #endregion private functions

        #region public functions
        public void Show()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOMoveY(GameManager.Y_TARGET, GameManager.DURATION_1);
            transform.DOScale(_originalScale, GameManager.DURATION_1).
                OnComplete(() => { _spinning = true; _collider.enabled = true; });

        }

        public void Spin()
        {
            _spinning = true;
        }

        public void Shake()
        {
            transform.DOShakePosition(GameManager.DURATION_0);
        }

        public void CompleteShake()
        {
            transform.DOComplete();
        }

        public void Explode()
        {
            transform.DOScale(Vector3.zero, GameManager.DURATION_2).
                OnComplete(() => { GameEvents.OnOnLevelFinishedClearing(); });
        }

        public void Stop()
        {
            _spinning = false;
        }
        #endregion public functions
    }
}