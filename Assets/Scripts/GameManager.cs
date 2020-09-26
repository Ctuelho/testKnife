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
        //layers
        public const int LAYER_DEFAULT = 0;
        public const int LAYER_KNIVES = 8;
        public const int LAYER_USED_KNIVES = 9;
        public const int LAYER_TARGET = 10;
        //speeds
        public const float SPEED_KNIFE = 85f;
        //distances
        public const float DISTANCE_KNIFE = 6f;
        #endregion consts

        #region properties
        public bool ADAvailable { get; private set; } = true;
        #endregion properties

        #region private fields
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
            GameEvents.RestartedGame += OnRestartedGame;
            GameEvents.SecondChangeGiven += OnSecondChangeGiven;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            GameEvents.KnifeAttached -= OnKnifeAttached;
            GameEvents.KnifeBroken -= OnKnifeBroken;
            GameEvents.KnifeFired -= OnKnifeFired;
            GameEvents.AllKnivesUsed -= OnAllKnivesUsed;
            GameEvents.RestartedGame -= OnRestartedGame;
            GameEvents.SecondChangeGiven -= OnSecondChangeGiven;
        }

        private void Start()
        {
            KnifeBlock.Instance.CreateKnives(20);
            KnifeBlock.Instance.EnableControlls();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
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
            OnRestartedGame();
        }

        private void OnRestartedGame()
        {
            ADAvailable = true;
            KnifeBlock.Instance.EnableControlls();
            KnifeBlock.Instance.CreateKnives(20);
            Target.Instance.Spin();
        }

        private void OnSecondChangeGiven()
        {
            ADAvailable = false;
            KnifeBlock.Instance.AddKnife();
            KnifeBlock.Instance.EnableControlls();
            Target.Instance.Spin();
        }
        #endregion private functions

        #region public functions 
        #endregion public functions
    }
}