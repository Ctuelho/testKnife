using UnityEngine;
using DG.Tweening;

namespace Game
{
    public class Target : MonoBehaviour
    {
        //reference
        public static Target Instance = null;

        #region private fields
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        private bool _spinning = true;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            Instance = this;
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

        public void Spin()
        {
            _spinning = true;
        }

        public void Shake()
        {
            transform.DOShakePosition(0.2f, 0.2f, 10);
        }

        public void CompleteShake()
        {
            transform.DOComplete();
        }

        public void Stop()
        {
            _spinning = false;
        }
        #endregion public functions
    }
}