namespace Game
{
    public static class GameEvents
    {
        #region delegates
        //gameplay
        public delegate void OnKnifeAttachedEvent();
        public delegate void OnKnifeBrokenEvent();
        public delegate void OnKnifeFiredEvent();
        public delegate void OnKnivesCreatedEvent(int knives);
        public delegate void OnAllKnivesUsedEvent();
        public delegate void OnStartedGameEvent();
        public delegate void OnRestartedGameEvent();
        public delegate void OnSecondChangeGivenEvent();
        public delegate void OnChildMenuOpenedEvent(ChildMenu childMenu);
        public delegate void OnChildMenuClosedEvent(ChildMenu childMenu);
        public delegate void OnKnifeStoreOpenEvent();
        public delegate void OnRankingMenuOpenEvent();
        public delegate void OnLevelFinishedSpawningEvent();
        public delegate void OnLevelFinishedClearingEvent();
        public delegate void OnAdTimerEndedEvent();
        public delegate void OnBossDefeatedEvent();
        public delegate void OnAppleHitEvent();
        public delegate void OnPointsChangedEvent();
        public delegate void OnApplesChangedEvent();
        public delegate void OnKnifeUnlockedEvent(KNIFE_ITEMS knife);
        public delegate void OnKnifeSelectedEvent(KNIFE_ITEMS knife);
        #endregion delegates

        #region events
        public static OnKnifeAttachedEvent KnifeAttached;
        public static OnKnifeBrokenEvent KnifeBroken;
        public static OnKnifeFiredEvent KnifeFired;
        public static OnKnivesCreatedEvent KnivesCreated;
        public static OnAllKnivesUsedEvent AllKnivesUsed;
        public static OnStartedGameEvent StartedGame;
        public static OnRestartedGameEvent RestartedGame;
        public static OnSecondChangeGivenEvent SecondChangeGiven;
        public static OnChildMenuOpenedEvent ChildMenuOpened;
        public static OnChildMenuClosedEvent ChildMenuClosed;
        public static OnKnifeStoreOpenEvent KnifeStoreOpen;
        public static OnRankingMenuOpenEvent RankingMenuOpen;
        public static OnLevelFinishedSpawningEvent LevelFinishedSpawning;
        public static OnLevelFinishedClearingEvent LevelFinishedClearing;
        public static OnAdTimerEndedEvent AdTimerEnded;
        public static OnBossDefeatedEvent BossDefeated;
        public static OnAppleHitEvent AppleHit;
        public static OnPointsChangedEvent PointsChanged;
        public static OnApplesChangedEvent ApplesChanged;
        public static OnKnifeUnlockedEvent KnifeUnlocked;
        public static OnKnifeSelectedEvent KnifeSelected;
        #endregion events

        #region public functions
        //to notify when the knife hits the target
        public static void OnKnifeAttached()
        {
            KnifeAttached?.Invoke();
        }

        //to notify when the knife hits another knife
        //means the player failed
        public static void OnKnifeBroken()
        {
            KnifeBroken?.Invoke();
        }

        //to notify when the knife player throws a knife
        public static void OnKnifeFired()
        {
            KnifeFired?.Invoke();
        }

        //to notify when the number of knives for the level
        //are created, so UI/HUD elements can update with the correct number
        public static void OnKnivesCreated(int knives)
        {
            KnivesCreated?.Invoke(knives);
        }

        //to notify when the player has used all knives
        //means that the player had success
        public static void OnAllKnivesUsed()
        {
            AllKnivesUsed?.Invoke();
        }

        //to notify when the start button was clicked
        //so UI/HUD can update their status
        public static void OnStartedGame()
        {
            StartedGame?.Invoke();
        }

        //to notify when the resstart button was clicked
        //so UI/HUD can update their status
        public static void OnRestartedGame()
        {
            RestartedGame?.Invoke();
        }

        //to notify when player got an additional try
        //so UI/HUD can update their status
        public static void OnSecondChangeGiven()
        {
            SecondChangeGiven?.Invoke();
        }

        //to notify when a child menu was open
        //so UI/HUD can update their status
        public static void OnChildMenuOpened(ChildMenu childMenu)
        {
            ChildMenuOpened?.Invoke(childMenu);
        }

        //to notify when a childmenu was closed
        //so UI/HUD can update their status
        public static void OnChildMenuClosed(ChildMenu childMenu)
        {
            ChildMenuClosed?.Invoke(childMenu);
        }

        //to notify when the menu was open
        //so UI/HUD can update their status
        public static void OnKnifeStoreOpen()
        {
            KnifeStoreOpen?.Invoke();
        }

        //to notify when the menu was open
        //so UI/HUD can update their status
        public static void OnRankingMenuOpen()
        {
            RankingMenuOpen?.Invoke();
        }

        //to notify when the level elements finished being created
        //so UI/HUD can update their status
        public static void OnLevelFinishedSpawning()
        {
            LevelFinishedSpawning?.Invoke();
        }

        //to notify when the level complete animations ended
        //so UI/HUD can update their status
        public static void OnOnLevelFinishedClearing()
        {
            LevelFinishedClearing?.Invoke();
        }

        //to notify when the watch ad timer ends
        //so UI/HUD can update their status
        public static void OnAdTimerEnded()
        {
            AdTimerEnded?.Invoke();
        }

        //to notify when a boss level was cleared
        //so UI/HUD can update their status
        public static void OnBossDefeated()
        {
            BossDefeated?.Invoke();
        }

        //to notify when the the player managed to hit an apple
        //so UI/HUD can update their status
        public static void OnAppleHit()
        {
            AppleHit?.Invoke();
        }

        //to notify when the score changed
        //so UI/HUD can update their status
        public static void OnPointsChanged()
        {
            PointsChanged?.Invoke();
        }

        //to notify when the number of apples changed
        //so UI/HUD can update their status
        public static void OnApplesChanged()
        {
            ApplesChanged?.Invoke();
        }

        //to notify when a new knife was unlocked
        //so UI/HUD can update their status
        public static void OnKnifeUnlocked(KNIFE_ITEMS knife)
        {
            KnifeUnlocked?.Invoke(knife);
        }

        //to notify when a kind of knife was selected
        //so UI/HUD can update their status
        public static void OnKnifeSelected(KNIFE_ITEMS knife)
        {
            KnifeSelected?.Invoke(knife);
        }

        #endregion public functions
    }
}