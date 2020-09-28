using UnityEngine;
using DG.Tweening;

namespace Game
{
    public class Apple : MonoBehaviour
    {
        #region private fields
        [SerializeField] private Collider2D _collider;
        [SerializeField] private ParticleSystem _confetti;
        private bool _used;
        #endregion private fields

        #region unity event functions
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_used)
            {
                return;
            }

            if (other.gameObject.layer == GameManager.LAYER_KNIVES)
            {
                _used = true;
                //throw the apple at a random direction
                _collider.enabled = false;
                _confetti.Play();
                transform.DOScale(Vector3.zero, GameManager.DURATION_4);

                GameEvents.OnAppleHit();
            }
        }
        #endregion unity event functions
    }
}