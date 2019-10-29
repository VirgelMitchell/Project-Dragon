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
            Debug.Log("XPRewardProgression.GetReward() recieves" +
                      "\n    Challenge Rating: " + challengeRating +
                      "\n    Attacker Level: " + attackerLevel);

            BuildLookup();

            if (!lookupTable.ContainsKey(challengeRating))
            {
                Debug.LogWarning("CR not Found");
                return 0;
            }

            int[] rewardLevels = lookupTable[challengeRating];

            if (attackerLevel > 20) { attackerLevel = 20; }
            if (rewardLevels.Length < attackerLevel + 1)
            {
                Debug.LogWarning("Attacker Level not found!");
                return 0;
            }

            Debug.Log("XP Reward = " + rewardLevels[attackerLevel]);
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