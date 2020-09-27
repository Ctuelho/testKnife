using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game
{
    public class KnifeBlock : MonoBehaviour
    {
        //reference
        public static KnifeBlock Instance = null;

        #region properties
        public bool UsedAllKnives { get => _knives.Count > 0 && _currentKnife != null; }

        public int KnivesLeft { get => _knives.Count + (_currentKnife != null ? 1 : 0); }

        public int KnivesUsed { get => _usedknives.Count; }

        public bool ControllsEnabled { get; private set; } = false;
        #endregion properties

        #region private fields
        [SerializeField] private GameObject _knifePrefab;
        [SerializeField] private Queue<GameObject> _knives;
        [SerializeField] private List<GameObject> _usedknives;
        private GameObject _currentKnife;
        private Vector3 _knifeOriginalScale;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            Instance = this;
            _knives = new Queue<GameObject>();
            _usedknives = new List<GameObject>();

            CreateKnives(1);
        }
        #endregion unity event functions

        #region private functions
        private void ClearKnives()
        {
            if(_currentKnife != null)
            {
                Destroy(_currentKnife);
            }

            while(_knives.Count > 0)
            {
                var knive = _knives.Dequeue();
                Destroy(knive);
            }

            _knives = new Queue<GameObject>();

            while (_usedknives.Count > 0)
            {
                var knive = _usedknives[0];
                Destroy(knive);
                _usedknives.RemoveAt(0);
            }

            _usedknives = new List<GameObject>();
        }
        #endregion private functions

        #region public functions
        public void Show()
        {
            transform.DOMoveY(GameManager.Y_KNIFE_BLOCK, GameManager.DURATION_1).
                OnComplete(() => { ControllsEnabled = true; });
        }

        public void ReadyKnife()
        {
            if (_knives.Count > 0)
            {
                _currentKnife = _knives.Dequeue();
                _currentKnife.transform.position = transform.position;
                _currentKnife.SetActive(true);
            }
            else
            {
                _currentKnife = null;
                GameEvents.OnAllKnivesUsed();
            }
        }

        public void CreateKnives(int knives)
        {
            ClearKnives();

            for(int i = 0; i < knives; i++)
            {
                GameObject newKnife = Instantiate(_knifePrefab);
                newKnife.transform.SetParent(transform);
                newKnife.SetActive(false);
                _knives.Enqueue(newKnife);
            }

            ReadyKnife();

            _knifeOriginalScale = _currentKnife.transform.localScale;
            _currentKnife.transform.localScale = Vector3.zero;
            _currentKnife.transform.DOScale(_knifeOriginalScale, GameManager.DURATION_0);

            GameEvents.OnKnivesCreated(knives);
        }

        public void FireKnife()
        {
            if (!ControllsEnabled)
            {
                return;
            }

            if (_currentKnife != null)
            {
                Knife knifeComponent = _currentKnife.GetComponent<Knife>();
                knifeComponent.SetVelocity(Vector3.up * GameManager.SPEED_KNIFE);
                _usedknives.Add(_currentKnife);
                _currentKnife = null;

                GameEvents.OnKnifeFired();
            }
            else
            {
                Debug.Log("Out of knives");
            }
        }

        public void EnableControlls()
        {
            ControllsEnabled = true;
        }

        public void DisableControlls()
        {
            ControllsEnabled = false;
        }

        public void AddKnife()
        {
            GameObject newKnife = Instantiate(_knifePrefab);
            newKnife.SetActive(false);
            _knives.Enqueue(newKnife);

            ReadyKnife();
        }

        public void ShakeOffKnives()
        {
            foreach(var knife in _usedknives)
            {
                var randomDirection = new Vector3(
                            Random.Range(180, -180) + 1,
                            Random.Range(180, -180) + 1,
                            Random.Range(180, -180) + 1).normalized;
                knife.transform.SetParent(transform);
                knife.GetComponent<Knife>().
                    SetVelocity(randomDirection * GameManager.SPEED_KNIFE);
                knife.transform.rotation = Quaternion.LookRotation(randomDirection);
            }
        }
        #endregion public functions
    }
}