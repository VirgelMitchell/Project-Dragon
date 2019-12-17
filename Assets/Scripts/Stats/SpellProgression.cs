using UnityEngine;
using RPG.Core;
using System.Collections.Generic;

namespace RPG.Stats
{

    [CreateAssetMenu(fileName = "SpellProgression", menuName = "Progression/New Spell Progression", order = 0)]
    public class SpellProgression : ScriptableObject
    {
        [SerializeField] SpellCountTypes[] countTypes;

        Dictionary<string, Dictionary<string, Dictionary<string, float[]>>> progressionLookup = null;

        public int GetSpellCount(SpellCountType spellCountType, CasterClass casterClass, int casterLevel, int spellLevel)
        {
            BuildProgressionLookup();

            if (!progressionLookup.ContainsKey(spellCountType.ToString()))
            {
                Debug.LogError("Invalid List");
                return -1;
            }

            if (!progressionLookup[spellCountType.ToString()].ContainsKey(casterClass.ToString()))
            {
                Debug.LogError("Invalid Caster Class");
                return -1;
            }

            if (!progressionLookup[spellCountType.ToString()][spellCountType.ToString()].ContainsKey(casterLevel.ToString()))
            {
                Debug.LogError("Invalid Caster Level");
                return -1;
            }

            float[] spellLevels = progressionLookup[spellCountType.ToString()][spellCountType.ToString()][casterLevel.ToString()];
            if (spellLevel > spellLevels.Length - 1)
            {
                Debug.LogError(spellCountType + "Cannot Cast Spells of this Level");
                return -1;
            }

            return (int)spellLevels[spellLevel];
        }

        void BuildProgressionLookup()
        {
            if (progressionLookup != null) { return; }

            progressionLookup = new Dictionary<string, Dictionary<string, Dictionary<string, float[]>>>();
            foreach(SpellCountTypes spellCountType in countTypes)
            {
                progressionLookup[spellCountType.ToString()] = BuildCasterClassLookup(spellCountType);
            }
        }

        [System.Serializable]
        public class SpellCountTypes
        {
            public SpellCountType spellCountType;
            public CasterClassProgression[] casterClasses;
        }

        private Dictionary<string, Dictionary<string, float[]>> BuildCasterClassLookup(SpellCountTypes spellCountType)
        {
            var casterClassLookup = new Dictionary<string, Dictionary<string, float[]>>();
            foreach (CasterClassProgression casterClass in spellCountType.casterClasses)
            {
                casterClassLookup[casterClass.ToString()] = BuildCasterLevelLookup(casterClass);
            }
            return casterClassLookup;
        }

        [System.Serializable]
        public class CasterClassProgression
        {
            public CasterClass casterClass;
            public CasterLevelProgression[] casterLevelProgressions;
        }

        private Dictionary<string, float[]> BuildCasterLevelLookup(CasterClassProgression casterClass)
        {
            var casterLevelLookup = new Dictionary<string, float[]>();
            foreach (CasterLevelProgression casterLevel in casterClass.casterLevelProgressions)
            {
                casterLevelLookup[casterLevel.ToString()] = casterLevel.spellLevel;
            }
            return casterLevelLookup;
        }

        [System.Serializable]
        public class CasterLevelProgression
        {
            public float casterLevel;
            public float[] spellLevel;
        }
    }
}