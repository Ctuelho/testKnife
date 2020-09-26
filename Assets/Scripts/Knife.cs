using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class Knife : MonoBehaviour
    {
        #region properties
        public bool Broken
        {
            get => _used;
            private set => _used = value;
        }
        #endregion properties

        #region private fields
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        private bool _used;
        #endregion private fields

        #region unity event functions
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_used)
            {
                return;
            }

            if(other.gameObject.layer == GameManager.LAYER_USED_KNIVES)
            {
                _used = true;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.gravityScale = 1;
                _collider.enabled = false;

                GetComponent<SpriteRenderer>().color = Color.red;

                GameEvents.OnKnifeBroken();
            }
            else if(other.gameObject.layer == GameManager.LAYER_TARGET)
            {
                //hit the target
                _used = true;
                _rigidbody.isKinematic = true;
                Attach(other.gameObject.transform, other.gameObject.transform.position - Vector3.up * GameManager.DISTANCE_KNIFE);
                GameEvents.OnKnifeAttached();
            }
        }
        #endregion unity event functions

        #region public functions
        /// <summary>
        /// Sets the knife's velocity
        /// </summary>
        /// <param name="veclocity">A vector3 representing the velocity</param>
        public void SetVelocity(Vector3 veclocity)
        {
            _rigidbody.velocity = veclocity;
        }

        public void Attach(Transform parent, Vector3 position)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            transform.SetParent(parent);
            transform.position = position;

            gameObject.layer = GameManager.LAYER_USED_KNIVES;
        }
        #endregion public functions
    }
}