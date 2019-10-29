using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "CharacterClass", menuName = "Progression/New Character Class", order = 0)]
    public class CharacterClass : ScriptableObject
    {
        [SerializeField] PlayerClass characterClass;
        [SerializeField] AttackProgression attackProgressionTable;
        [SerializeField] ProgressionRate attackProgressionRate;   // RPG.Core.Enumerators.ProgressionRate
        [SerializeField] int hitDieSize;
        [SerializeField] int skillPointsPerLevel;

        bool isCaster = false;

        private void Start()
        {
            SetIsCaster();
        }

        public PlayerClass GetClass() { return characterClass; }
        public int GetHitDieSize() { return hitDieSize; }

        public int GetAttacksPerRound(int level)
        {
            return attackProgressionTable.GetAttacks(attackProgressionRate, level);
        }

        private bool SetIsCaster()
        {
            foreach (CasterClass cClass in Enum.GetValues(typeof (CasterClass)))
            {
                if (cClass.ToString() == characterClass.ToString()) { return true; }
            }
            return false;
        }
    }
}