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
        public delegate void OnKnifeStoreOpenEvent();
        public delegate void OnLevelFinishedSpawningEvent();
        public delegate void OnLevelFinishedClearingEvent();
        public delegate void OnAdTimerEndedEvent();
        public delegate void OnBossDefeatedEvent();
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
        public static OnKnifeStoreOpenEvent KnifeStoreOpen;
        public static OnLevelFinishedSpawningEvent LevelFinishedSpawning;
        public static OnLevelFinishedClearingEvent LevelFinishedClearing;
        public static OnAdTimerEndedEvent AdTimerEnded;
        public static OnBossDefeatedEvent BossDefeated;
        #endregion events

        #region public functions
        public static void OnKnifeAttached()
        {
            KnifeAttached?.Invoke();
        }

        public static void OnKnifeBroken()
        {
            KnifeBroken?.Invoke();
        }

        public static void OnKnifeFired()
        {
            KnifeFired?.Invoke();
        }

        public static void OnKnivesCreated(int knives)
        {
            KnivesCreated?.Invoke(knives);
        }

        public static void OnAllKnivesUsed()
        {
            AllKnivesUsed?.Invoke();
        }

        public static void OnStartedGame()
        {
            StartedGame?.Invoke();
        }
        public static void OnRestartedGame()
        {
            RestartedGame?.Invoke();
        }

        public static void OnSecondChangeGiven()
        {
            SecondChangeGiven?.Invoke();
        }

        public static void OnKnifeStoreOpen()
        {
            KnifeStoreOpen?.Invoke();
        }

        public static void OnLevelFinishedSpawning()
        {
            LevelFinishedSpawning?.Invoke();
        }

        public static void OnOnLevelFinishedClearing()
        {
            LevelFinishedClearing?.Invoke();
        }

        public static void OnAdTimerEnded()
        {
            AdTimerEnded?.Invoke();
        }

        public static void OnBossDefeated()
        {
            BossDefeated?.Invoke();
        }
        #endregion public functions
    }
}