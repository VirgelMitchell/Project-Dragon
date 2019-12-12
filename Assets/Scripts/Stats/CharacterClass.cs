using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "CharacterClass", menuName = "Progression/New Character Class", order = 0)]
    public class CharacterClass : ScriptableObject
    {
        // RPG.Core.Enumerators.PlayerClass
        [SerializeField] PlayerClass characterClass;

        // RPG.Core.Enumerators.ProgressionRate
        [SerializeField] ProgressionRate attackProgressionRate;
        [SerializeField] ProgressionRate fortitudeSaveProgressionRate;
        [SerializeField] ProgressionRate reflexSaveProgressionRate;
        [SerializeField] ProgressionRate willSaveProgressionRate;
        [SerializeField] StatProgression statProgression; // reference to Stat Progression SO

        [SerializeField] int hitDieSize;
        [SerializeField] int skillPointsPerLevel;

        bool isCaster = false;

        public PlayerClass GetClass() { return characterClass; }
        public int GetHitDieSize() { return hitDieSize; }

        public int GetAttacksPerRound(int level)
        {
            return statProgression.GetStat(CharacterStat.attacks, attackProgressionRate, level);
        }

        public int GetFortitudeBaseSave(int level)
        {
            return statProgression.GetStat(CharacterStat.baseSave, fortitudeSaveProgressionRate, level);
        }

        public int GetReflexBaseSave(int level)
        {
            return statProgression.GetStat(CharacterStat.baseSave, reflexSaveProgressionRate, level);
        }

        public int GetWillBaseSave(int level)
        {
            return statProgression.GetStat(CharacterStat.baseSave, willSaveProgressionRate, level);
        }
    }
}