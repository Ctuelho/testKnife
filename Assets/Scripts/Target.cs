using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace Game
{
    public class Target : MonoBehaviour
    {
        //reference
        public static Target Instance = null;

        #region private fields
        [SerializeField] private GameObject _obstaclePrefab;
        [SerializeField] private GameObject _applePrefab;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private ParticleSystem _explosion;
        private bool _spinning = false;
        private float _spin = 1f;
        private Vector3 _originalScale;
        private List<float> _availableAngles;
        private Sequence _spinSequence;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
            transform.localPosition = Vector3.up * GameManager.Y_TARGET_START;
            _collider.enabled = false;
            _originalScale = transform.localScale;

            //define the angles for apples/obstacles
            //so when placed randomly they won't collide
            _availableAngles = new List<float>();
            float radialStep = 360f / GameManager.MAX_PROPS_ON_TARGET;
            for (int i = 0; i < GameManager.MAX_PROPS_ON_TARGET; i++)
            {
                _availableAngles.Add(i * radialStep);
            }

            _spinSequence = DOTween.Sequence();

            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_spinning)
            {
                //spins the target
                transform.Rotate(Vector3.forward * _spin * Time.deltaTime);
            }

        }
        #endregion unity event functions

        #region private functions
        private GameObject SpawnProp(GameObject prefab)
        {
            //spawn the prop at an available random angle
            var prop = Instantiate(prefab);
            var randomAngleIndex = Random.Range(0, _availableAngles.Count);
            var anlge = _availableAngles[randomAngleIndex];
            //rotate the prop
            prop.transform.rotation = Quaternion.AngleAxis(anlge, Vector3.forward);         
            prop.transform.SetParent(transform);

            //remove the used position, so there is no conflict
            _availableAngles.RemoveAt(randomAngleIndex);

            return prop;
        }
        #endregion private functions

        #region public functions
        public void Show()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOMoveY(GameManager.Y_TARGET, GameManager.DURATION_1);
            transform.DOScale(_originalScale, GameManager.DURATION_1).
                OnComplete(() => { Spin(true); _collider.enabled = true; });

        }

        public void Spin(bool startSpin = false)
        {
            _spinSequence.Kill();
            //first dfeine if it will be clockwise or counterclockwise
            var signal = Random.value > 0.5f ? 1 : -1;
            //get a random duration
            var duration = Random.Range(GameManager.DURATION_MIN_SPIN, GameManager.DURATION_MAX_SPIN);
            //get a random spin speed
            var speed = Random.Range(GameManager.SPEED_MIN_SPIN, GameManager.SPEED_MAX_SPIN);
            if (startSpin)
            {
                _spin = GameManager.SPEED_MIN_SPIN * signal;
                speed = GameManager.SPEED_MAX_SPIN;
                duration = GameManager.DURATION_MIN_SPIN;
            } 
            speed *= signal;

            //set the spin motion
            _spinning = true;
            _spinSequence.SetEase(Ease.OutBounce);
            _spinSequence.Append(DOTween.To(() => _spin, x => _spin = x, speed, duration).OnComplete(()=> { Spin(); }));
        }

        public void Shake()
        {
            //animate to simulate impact with a knife
            transform.DOShakePosition(GameManager.DURATION_0);
        }

        public void CompleteShake()
        {
            transform.DOComplete();
        }

        public void Explode()
        {
            //animate to show that the player had success destroying the target
            _explosion.Play();
            transform.DOScale(Vector3.zero, GameManager.DURATION_2).
                OnComplete(() => {
                    GameEvents.OnOnLevelFinishedClearing(); 
                });
        }

        public void Stop()
        {
            _spinning = false;
            _spinSequence.Kill();
        }

        public void AddObstacles(int obstacles)
        {
            //spawn the obstalces at available random angles
            for (int i = 0; i < obstacles; i++)
            {
                var obstacle = SpawnProp(_obstaclePrefab);
                obstacle.transform.position = transform.position + (-obstacle.transform.up * GameManager.DISTANCE_KNIFE);
            }
        }

        public void AddApples(int apples)
        {
            //spawn the apples at available random angles
            for (int i = 0; i < apples; i++)
            {
                var apple = SpawnProp(_applePrefab);
                apple.transform.position = transform.position + (-apple.transform.up * GameManager.DISTANCE_APPLE);
            }
        }
        #endregion public functions
    }
}