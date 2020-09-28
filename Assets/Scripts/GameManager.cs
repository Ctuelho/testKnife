using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

//controls the game and hold global values and states
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
        public const int MAX_APPLES = 10;
        public const float APPLE_INCREASE_CHANCE = 0.5f;
        public const float APPLE_SPAWN_CHANCE = 0.5f;
        public const int MAX_PROPS_ON_TARGET = 20;
        //layers
        public const int LAYER_DEFAULT = 0;
        public const int LAYER_KNIVES = 8;
        public const int LAYER_USED_KNIVES = 9;
        public const int LAYER_TARGET = 10;
        //speeds
        public const float SPEED_KNIFE = 100f;
        public const float SPEED_MIN_SPIN = 45f;
        public const float SPEED_MAX_SPIN = 270f;
        //distances
        public const float DISTANCE_KNIFE = 6f;
        public const float DISTANCE_APPLE = 7f;
        //durations
        public const float DURATION_0 = 0.2f;
        public const float DURATION_1 = 0.4f;
        public const float DURATION_2 = 1.2f;
        public const float DURATION_3 = 5f;
        public const float DURATION_4 = 0.8f;
        public const float DURATION_MIN_SPIN = 2.0f;
        public const float DURATION_MAX_SPIN = 4.0f;
        //positions
        public const float Y_KNIFE_BLOCK = 5f;
        public const float Y_TARGET_START = 50f;
        public const float Y_TARGET = 25f;
        //costs
        public const int COST_DEFAULT_KNIFE = 10;
        #endregion consts

        #region properties
        public bool ADAvailable { get; private set; } = true;
        public int BestScore { get => _bestScore; private set { _bestScore = value; } }
        public int Points
        {
            get => _points;
            private set
            {
                _points = value; GameEvents.OnPointsChanged();
            }
        }
        public int Apples
        { 
            get => _apples; 
            private set 
            { 
                _apples = value; GameEvents.OnApplesChanged();
            } 
        }

        public Color KnifeColor
        {
            get => _knifeColor;
            private set => _knifeColor = value;
        }

        public KNIFE_ITEMS CurrentKnife
        {
            get;
            private set ;
        }
        #endregion properties

        #region private fields
        [SerializeField] private GameObject _targetPrefab;
        [SerializeField] private GameObject _bossPrefab;
        private int _currentLevel = 1;
        private int _obstacles = 0;
        private int _possibleApples = 0;
        private int _knives = MIN_KNIVES;
        private int _bestScore = 0;
        private int _points = 0;
        private int _apples = 0;
        private bool _bossLevel = false;
        private bool _forceIncreaseApple = false;

        //knives locked status data
        private Dictionary<KNIFE_ITEMS, Color> _knivesColors;
        private UnlockedKnivesData _knivesUnlockedData;
        private Color _knifeColor;
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

                //initialize the sample knife colors
                _knivesColors = new Dictionary<KNIFE_ITEMS, Color>();
                _knivesColors.Add(KNIFE_ITEMS.WHITE, Color.white);
                _knivesColors.Add(KNIFE_ITEMS.ORANGE, new Color(1, 0.75f, 0, 1));
                _knivesColors.Add(KNIFE_ITEMS.GREEN, Color.green);
                _knivesColors.Add(KNIFE_ITEMS.CYAN, new Color(0, 1, 1, 1));
                _knivesColors.Add(KNIFE_ITEMS.PINK, new Color(1, 0.05f, 0.5f, 1));
                _knivesColors.Add(KNIFE_ITEMS.BLACK, new Color(0.05f, 0.05f, 0.05f, 1));

                //initialize the save data
                _knivesUnlockedData = new UnlockedKnivesData();
                var knivesTypes = Enum.GetValues(typeof(KNIFE_ITEMS));
                foreach(KNIFE_ITEMS knifeType in knivesTypes)
                {
                    _knivesUnlockedData.knivesData.Add(new UnlockedKnifeData { KnifeType = knifeType, Unlocked = false});
                }
                var whiteKniveData = _knivesUnlockedData.knivesData.Where(k => k.KnifeType == KNIFE_ITEMS.WHITE).FirstOrDefault();
                if(whiteKniveData != null)
                {
                    //the default is white unlocked
                    whiteKniveData.Unlocked = true;
                }
            }
        }

        void OnEnable()
        {
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
            GameEvents.AppleHit += OnAppleHit;
            GameEvents.KnifeUnlocked += OnKnifeUnlocked;
            GameEvents.KnifeSelected += SelectKnife;
        }

        void OnDisable()
        {
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
            GameEvents.AppleHit -= OnAppleHit;
            GameEvents.KnifeUnlocked -= OnKnifeUnlocked;
            GameEvents.KnifeSelected -= SelectKnife;
        }

        private void Start()
        {
            LoadData();
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
        private void OnKnifeBroken()
        {
            KnifeBlock.Instance.DisableControlls();
            Target.Instance.Stop();
        }

        private void OnKnifeFired()
        {
            Target.Instance.CompleteShake();
        }

        private void OnKnifeAttached()
        {
            Points++;
            SaveData();
            GameEvents.OnPointsChanged();
            KnifeBlock.Instance.ReadyKnife();
            Target.Instance.Shake();
        }

        private void OnAllKnivesUsed()
        {
            KnifeBlock.Instance.DisableControlls();
            KnifeBlock.Instance.ShakeOffKnives();
            Target.Instance.Explode();
            if(_bossLevel)
            {
                _bossLevel = false;
                GameEvents.BossDefeated();
            }
        }

        private void OnStartedGame()
        {
            SpawnLevel();
        }

        private void OnRestartedGame()
        {
            Points = 0;
            _currentLevel = 1;
            _obstacles = 0;
            _possibleApples = 0;
            _forceIncreaseApple = false;
            _bossLevel = false;
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

        //determine the characteristics of the current level
        //and spawns the elements
        private void SpawnLevel()
        {
            int spawnedApples = 0;
            //roll to spawn apples
            for(int i = 0; i < _possibleApples; i++)
            {
                if(Random.value <= APPLE_SPAWN_CHANCE)
                {
                    spawnedApples++;
                }
            }

            if (_bossLevel)
            {
                Instantiate(_bossPrefab);
                Target.Instance.AddObstacles(_obstacles + 1);
                Target.Instance.AddApples(spawnedApples + 1);
            }
            else
            {
                Instantiate(_targetPrefab);
                Target.Instance.AddObstacles(_obstacles);
                Target.Instance.AddApples(spawnedApples);
            }
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
            
            //roll for increase number of apples
            if(_forceIncreaseApple || Random.value <= APPLE_INCREASE_CHANCE)
            {
                _possibleApples = Mathf.Min(++_possibleApples, MAX_APPLES);
                _forceIncreaseApple = false;
            }
            else
            {
                //next time one possible aple will be added to avoid not spawning any at all
                _forceIncreaseApple = true;
            }

            if (_currentLevel % 5f == 0)
            {
                //boss level
                _bossLevel = true;
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
            //disable ads for the current gameplay
            ADAvailable = false;
        }

        private void OnAppleHit()
        {
            //increse apples
            Apples++;
            SaveData();
        }

        private void OnKnifeUnlocked(KNIFE_ITEMS knife)
        {
            var unlockedKniveData = _knivesUnlockedData.knivesData.Where(k => k.KnifeType == knife).FirstOrDefault();
            if (unlockedKniveData != null)
            {
                //marks the knife as unlocked in the save data
                unlockedKniveData.Unlocked = true;
            }
            SaveData();
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt("Apples", Apples);
            
            //saves the higher score
            if(Points > _bestScore)
            {
                _bestScore = Points;
                PlayerPrefs.SetInt("Points", Points);
            }

            PlayerPrefs.SetString("CurrentKnife", CurrentKnife.ToString());

            var path = Path.Combine(Application.persistentDataPath, "unlockedKnivesData.txt");
            var unlockedKnivesAsJson = JsonUtility.ToJson(_knivesUnlockedData);
            File.WriteAllText(path, unlockedKnivesAsJson);
        }

        private void LoadData()
        {
            if (PlayerPrefs.HasKey("Apples"))
            {
                Apples = PlayerPrefs.GetInt("Apples");
            }

            if (PlayerPrefs.HasKey("Points"))
            {
                _bestScore = PlayerPrefs.GetInt("Points");
            }

            if (PlayerPrefs.HasKey("CurrentKnife"))
            {
                KNIFE_ITEMS savedKnife;
                if(Enum.TryParse(PlayerPrefs.GetString("CurrentKnife"), out savedKnife))
                {
                    SelectKnife(savedKnife);
                }
                else
                {
                    //fallback to white
                    SelectKnife(KNIFE_ITEMS.WHITE);
                }
            }

            var path = Path.Combine(Application.persistentDataPath, "unlockedKnivesData.txt");
            if (File.Exists(path))
            {
                string loadedData = File.ReadAllText(path);
                Debug.Log(loadedData);
                _knivesUnlockedData = JsonUtility.FromJson<UnlockedKnivesData>(loadedData);
            }
        }

        private void SelectKnife(KNIFE_ITEMS knife)
        {
            if (!_knivesColors.ContainsKey(knife))
            {
                //fallback to white
                _knifeColor = _knivesColors[KNIFE_ITEMS.WHITE];
            }
            _knifeColor = _knivesColors[knife];
            CurrentKnife = knife;
        }
        #endregion private functions

        #region public functions
        //returns if the knife is unlocked
        public bool IsKnifeUnlocked(KNIFE_ITEMS knife)
        {
            var unlockedKniveData = _knivesUnlockedData.knivesData.Where(k => k.KnifeType == knife).FirstOrDefault();
            if (unlockedKniveData == null)
            {
                return false;
            }
            return unlockedKniveData.Unlocked;
        }

        //check if has enough apples to unlock a knife
        public bool CanUnlockKnife()
        {
            if(_apples >= COST_DEFAULT_KNIFE)
            {
                Apples -= COST_DEFAULT_KNIFE;
                return true;
            }
            return false;
        }
        #endregion public functions
    }
}