using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public enum KNIFE_ITEMS { WHITE, ORANGE, GREEN, CYAN, PINK, BLACK };

    [Serializable]
    public class UnlockedKnifeData
    {
        public KNIFE_ITEMS KnifeType;
        public bool Unlocked;
    }

    [Serializable]
    public class UnlockedKnivesData
    {
        public List<UnlockedKnifeData> knivesData;

        public UnlockedKnivesData()
        {
            knivesData = new List<UnlockedKnifeData>();
        }
    }
}
