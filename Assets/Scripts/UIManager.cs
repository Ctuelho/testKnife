using UnityEngine;

namespace Game
{
    //controls the flow and state of the UI
    public class UIManager : MonoBehaviour
    {
        //reference
        public static UIManager Instance = null;

        #region private fields
        [SerializeField] private ChildMenu _currentChildMenu;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            MainMenu.Instance.Show();
        }

        private void OnEnable()
        {
            GameEvents.KnifeBroken += OnKnifeBroken;
            GameEvents.RestartedGame += OnRestartGame;
            GameEvents.SecondChangeGiven += OnSecondChanceGiven;
            GameEvents.AdTimerEnded += OnAdTimerEnded;
            GameEvents.ChildMenuOpened += OnChildMenuOpened;
            GameEvents.ChildMenuClosed += OnChildMenuClosed;
            GameEvents.KnifeStoreOpen += OnKnifeStoreOpen;
            GameEvents.RankingMenuOpen += OnRankingMenuOpen;
            GameEvents.KnifeSelected += OnKnifeSelected;
        }

        private void OnDisable()
        {
            GameEvents.KnifeBroken -= OnKnifeBroken;
            GameEvents.RestartedGame -= OnRestartGame;
            GameEvents.SecondChangeGiven -= OnSecondChanceGiven;
            GameEvents.AdTimerEnded -= OnAdTimerEnded;
            GameEvents.ChildMenuOpened -= OnChildMenuOpened;
            GameEvents.ChildMenuClosed -= OnChildMenuClosed;
            GameEvents.KnifeStoreOpen -= OnKnifeStoreOpen;
            GameEvents.RankingMenuOpen -= OnRankingMenuOpen;
            GameEvents.KnifeSelected -= OnKnifeSelected;
        }
        #endregion unity event functions

        #region private functions
        private void OnChildMenuOpened(ChildMenu childMenu)
        {
            if (_currentChildMenu != null && _currentChildMenu != childMenu)
            {
                _currentChildMenu.Hide();
            }

            _currentChildMenu = childMenu;

            MainMenu.Instance.SetInteractionsEnabled();
            MainMenu.Instance.DisablePlayButton();
            GameOverMenu.Instance.DisableInteractions();
        }

        private void OnChildMenuClosed(ChildMenu childMenu)
        {
            if (_currentChildMenu != null && _currentChildMenu == childMenu)
            {
                //closed all child menus
                _currentChildMenu = null;
                MainMenu.Instance.SetInteractionsEnabled();
                MainMenu.Instance.EnablePlayButton();
            }

            GameOverMenu.Instance.EnableInteractions();
        }

        private void OnKnifeBroken()
        {
            //shows the menu only after the timer ends
            if (!GameManager.Instance.ADAvailable) MainMenu.Instance.Show();
            
            GameOverMenu.Instance.Show();
        }

        private void OnRestartGame()
        {
            MainMenu.Instance.Hide();
        }

        private void OnSecondChanceGiven()
        {
            MainMenu.Instance.Hide();
        }

        private void OnAdTimerEnded()
        {
            MainMenu.Instance.Show();
            GameOverMenu.Instance.Show();
        }

        private void OnKnifeStoreOpen()
        {
            KnifeStoreMenu.Instance.Show();
        }

        private void OnRankingMenuOpen()
        {
            RankingMenu.Instance.Show();
        }

        private void OnKnifeSelected(KNIFE_ITEMS knife)
        {
            KnifeStoreMenu.Instance.RefreshSelectedKnive();
        }
        #endregion private functions
    }
}