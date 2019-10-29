using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "AttackProgression", menuName = "Progression/New AttackProgression", order = 0)]
    public class AttackProgression : ScriptableObject
    {
        [SerializeField] AttackProgressionRate[] attackProgressions = null;

        Dictionary<ProgressionRate, float[]> lookupTable = null;

        public int GetAttacks(ProgressionRate rate, int levelIndex)
        {
            BuildLookup();
            float[] levels = lookupTable[rate];
            return (int)levels[levelIndex];
        }

        private void BuildLookup()
        {
            if (lookupTable != null) { return; }

            lookupTable = new Dictionary<ProgressionRate, float[]>();
            foreach (AttackProgressionRate attackProgressionRate in attackProgressions)
            {
                lookupTable[attackProgressionRate.rate] = attackProgressionRate.level;
            }
        }

        [System.Serializable] class AttackProgressionRate
        {
            public ProgressionRate rate;
            public float[] level;
        }
    }
}