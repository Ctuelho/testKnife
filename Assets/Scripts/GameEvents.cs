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
        //ui
        public delegate void OnRestartedGameEvent();
        public delegate void OnSecondChangeGivenEvent();
        #endregion delegates

        #region events
        //gameplay
        public static OnKnifeAttachedEvent KnifeAttached;
        public static OnKnifeBrokenEvent KnifeBroken;
        public static OnKnifeFiredEvent KnifeFired;
        public static OnKnivesCreatedEvent KnivesCreated;
        public static OnAllKnivesUsedEvent AllKnivesUsed;
        //ui
        public static OnRestartedGameEvent RestartedGame;
        public static OnSecondChangeGivenEvent SecondChangeGiven;
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

        public static void OnRestartedGame()
        {
            RestartedGame?.Invoke();
        }

        public static void OnSecondChangeGiven()
        {
            SecondChangeGiven?.Invoke();
        }
        #endregion public functions
    }
}