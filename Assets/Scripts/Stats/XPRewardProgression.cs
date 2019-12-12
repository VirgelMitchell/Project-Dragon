using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "XPRewardProgression", menuName = "Progression/New XPRewardProgression", order = 0)]
    public class XPRewardProgression : ScriptableObject
    {
        [SerializeField] ChallengeProgression[] challengeProgressions = null;

        Dictionary<string, int[]> lookupTable = null;

        public int GetReward(string challengeRating, int attackerLevel)
        {
            BuildLookup();

            if (attackerLevel > 20) { attackerLevel = 20; }

            if (!lookupTable.ContainsKey(challengeRating)) { return 0; }
            int[] rewardLevels = lookupTable[challengeRating];

            if (rewardLevels.Length < attackerLevel + 1) { return 0; }

            return rewardLevels[attackerLevel];
        }

        private void BuildLookup()
        {
            if (lookupTable != null) { return; }
            lookupTable = new Dictionary<string, int[]>();
            foreach (ChallengeProgression progression in challengeProgressions)
            {
                lookupTable[progression.challengeRating] = progression.level;
            }
        }

        [System.Serializable] class ChallengeProgression
        {
            public string challengeRating;
            public int[] level;
        }
    }
}