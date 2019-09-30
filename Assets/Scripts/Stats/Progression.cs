using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Progression/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] CharacterClassProgression[] characterClasses = null;

        public int GetStat(Stat stat, CharacterClass cClass, int levelIndex)
        {
            foreach (CharacterClassProgression progressionClass in characterClasses)
            {
                if (progressionClass.characterClass != cClass) { continue; }
                foreach (StatProgression statProgression in progressionClass.stats)
                {
                    if (statProgression.stat != stat) { continue; }
                    if (statProgression.level.Length < levelIndex + 1) { continue; }
                    return statProgression.level[levelIndex];
                }
            }
            return 0;
        }

        [System.Serializable] class CharacterClassProgression
        {
            public CharacterClass characterClass;
            public StatProgression[] stats;
        }

        [System.Serializable] class StatProgression
        {
            public Stat stat;
            public int[] level;
        }
    }
}