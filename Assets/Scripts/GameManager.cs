using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        //singleton
        public static GameManager Instance = null;

        #region consts
        //values
        public const int MAX_KNIVES = 15;
        public const int MIN_KNIVES = 7;
        public const int MAX_OBSTACLES = 10;
        //layers
        public const int LAYER_DEFAULT = 0;
        public const int LAYER_KNIVES = 8;
        public const int LAYER_USED_KNIVES = 9;
        public const int LAYER_TARGET = 10;
        //speeds
        public const float SPEED_KNIFE = 85f;
        //distances
        public const float DISTANCE_KNIFE = 6f;
        //durations
        public const float DURATION_0 = 0.2f;
        public const float DURATION_1 = 0.4f;
        public const float DURATION_2 = 1.2f;
        public const float DURATION_3 = 5f;
        //positions
        public const float Y_KNIFE_BLOCK = 5f;
        public const float Y_TARGET_START = 50f;
        public const float Y_TARGET = 25f;
        #endregion consts

        #region properties
        public bool ADAvailable { get; private set; } = true;
        #endregion properties

        #region private fields
        [SerializeField] private GameObject _targetPrefab;
        private int _currentLevel = 1;
        private int _obstacles = 0;
        private int _knives = MIN_KNIVES;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            GameEvents.KnifeBroken += OnKnifeBroken;
            GameEvents.KnifeAttached += OnKnifeAttached;
            GameEvents.KnifeFired += OnKnifeFired;
            GameEvents.AllKnivesUsed += OnAllKnivesUsed;
            GameEvents.StartedGame += OnStartedGame;
            GameEvents.RestartedGame += OnRestartedGame;
            GameEvents.SecondChangeGiven += OnSecondChangeGiven;
            GameEvents.LevelFinishedSpawning += OnLevelFinishedSpawning;
            GameEvents.LevelFinishedClearing += OnLevelFinishedClearing;
            GameEvents.AdTimerEnded += OnAdTimerEnded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            GameEvents.KnifeAttached -= OnKnifeAttached;
            GameEvents.KnifeBroken -= OnKnifeBroken;
            GameEvents.KnifeFired -= OnKnifeFired;
            GameEvents.AllKnivesUsed -= OnAllKnivesUsed;
            GameEvents.StartedGame -= OnStartedGame;
            GameEvents.RestartedGame -= OnRestartedGame;
            GameEvents.SecondChangeGiven -= OnSecondChangeGiven;
            GameEvents.LevelFinishedSpawning -= OnLevelFinishedSpawning;
            GameEvents.LevelFinishedClearing -= OnLevelFinishedClearing;
            GameEvents.AdTimerEnded -= OnAdTimerEnded;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                KnifeBlock.Instance.FireKnife();
            }
        }
        #endregion unity event functions

        #region private functions
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);
        }
        
        private void OnKnifeBroken()
        {
            Debug.Log("Knife broken");
            KnifeBlock.Instance.DisableControlls();
            Target.Instance.Stop();
        }

        private void OnKnifeFired()
        {
            Target.Instance.CompleteShake();
        }

        private void OnKnifeAttached()
        {
            KnifeBlock.Instance.ReadyKnife();
            Target.Instance.Shake();
        }

        private void OnAllKnivesUsed()
        {
            KnifeBlock.Instance.DisableControlls();
            KnifeBlock.Instance.ShakeOffKnives();
            Target.Instance.Explode();
            if(_currentLevel % 5 == 0)
            {
                GameEvents.BossDefeated();
            }
        }

        private void OnStartedGame()
        {
            SpawnLevel();
        }

        private void OnRestartedGame()
        {
            _currentLevel = 0;
            _obstacles = 0;
            _knives = MIN_KNIVES;
            ADAvailable = true;
            SpawnLevel();
        }

        private void OnSecondChangeGiven()
        {
            ADAvailable = false;
            KnifeBlock.Instance.AddKnife();
            KnifeBlock.Instance.EnableControlls();
            Target.Instance.Spin();
        }

        private void SpawnLevel()
        {
            Instantiate(_targetPrefab);
            KnifeBlock.Instance.CreateKnives(_knives);
            KnifeBlock.Instance.Show();
            Target.Instance.Show();
        }

        private void OnLevelFinishedSpawning()
        {
            KnifeBlock.Instance.EnableControlls();
        }

        private void OnLevelFinishedClearing()
        {
            _currentLevel++;
            _knives = Mathf.Min(++_knives, MAX_KNIVES);

            if (_currentLevel % 5f == 0)
            {
                //boss level
            }
            else if (_currentLevel % 2f == 0)
            {
                //increase obstacles
                _obstacles = Mathf.Min(++_obstacles, MAX_OBSTACLES);
            }
            SpawnLevel();
        }

        private void OnAdTimerEnded()
        {
            ADAvailable = false;
        }
        #endregion private functions

        #region public functions 
        #endregion public functions
    }
}