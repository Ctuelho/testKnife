using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private Queue<GameObject> _usedknives;
        private GameObject _currentKnife;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            Instance = this;
            _knives = new Queue<GameObject>();
            _usedknives = new Queue<GameObject>();
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
                var knive = _usedknives.Dequeue();
                Destroy(knive);
            }

            _usedknives = new Queue<GameObject>();
        }
        #endregion private functions

        #region public functions
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
                newKnife.SetActive(false);
                _knives.Enqueue(newKnife);
            }

            ReadyKnife();

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
                _usedknives.Enqueue(_currentKnife);
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
        #endregion public functions
    }
}