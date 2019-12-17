using UnityEngine;
using RPG.Core;
using System.Collections.Generic;

namespace RPG.Stats
{

    [CreateAssetMenu(fileName = "StatProgression", menuName = "Progression/New Stat Progression", order = 0)]
    public class StatProgression : ScriptableObject
    {
        [SerializeField] StatProgressionRate[] statProgressionRates;

        Dictionary<string, Dictionary<string, float[]>> progressionLookup = null;

        public int GetStat(CharacterStat stat, ProgressionRate rate, int level)
        {
            BuildLookup();

            if (progressionLookup.ContainsKey(rate.ToString()))
            {
                if (progressionLookup[rate.ToString()].ContainsKey(stat.ToString()))
                {
                    float[] levels = progressionLookup[rate.ToString()][stat.ToString()];
                    if (level > levels.Length - 1)
                    {
                        Debug.LogWarning("Level out of Range");
                        level = (int)levels[levels.Length - 1];
                    }

                    return (int)levels[level];
                }
                Debug.LogError("Stat Not Found");
                return 0;
            }
            Debug.LogError("Progression Rate Not Found");
            return 0;
        }

        void BuildLookup()
        {
            if (progressionLookup != null) { return; }

            progressionLookup = new Dictionary<string, Dictionary<string, float[]>>();
            foreach (StatProgressionRate pRate in statProgressionRates)
            {
                var statLookup = new Dictionary<string, float[]>();
                foreach (CharacterStatProgression cStat in pRate.stats)
                {
                    statLookup[cStat.stat.ToString()] = cStat.levels;
                }

                progressionLookup[pRate.rate.ToString()] = statLookup;
            }
        }

        [System.Serializable] class StatProgressionRate
        {
            public ProgressionRate rate;
            public CharacterStatProgression[] stats;
        }

        [System.Serializable] class CharacterStatProgression
        {
            public CharacterStat stat;
            public float[] levels;
        }
    }
}