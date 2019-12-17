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
        [SerializeField] bool isCaster = false;
        [SerializeField] int hitDieSize;
        [SerializeField] int skillPointsPerLevel;

        [Header("Progression Rates")]   // RPG.Core.Enumerators.ProgressionRate
        [SerializeField] ProgressionRate attackProgressionRate;
        [SerializeField] ProgressionRate fortitudeSaveProgressionRate;
        [SerializeField] ProgressionRate reflexSaveProgressionRate;
        [SerializeField] ProgressionRate willSaveProgressionRate;

        int[] spellsKnown;
        int[] spellsPerDay;

        CasterClass casterClass;
        SpellProgression spellProgression;
        StatProgression statProgression;


    // Standard Methods
        private void Start()
        {
            statProgression = Resources.Load<StatProgression>(Constant.statProgresssionPath);
            if (isCaster)
            {
                SetCasterClass();
                spellProgression = Resources.Load<SpellProgression>(Constant.spellProgresssionPath);
                if (spellsKnown == null || spellsPerDay == null) { ResetSpellCounts(0); }
            }
        }


    // Getter Methods
        public PlayerClass GetClass()   { return characterClass; }
        public int GetHitDieSize()      { return hitDieSize; }
        public int[] GetSpellsKnown()   { return spellsKnown; }
        public int[] GetSpellsPerDay()  { return spellsPerDay; }

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

        public int GetNextLevel(int level)
        {
            return statProgression.GetStat(CharacterStat.xPRequirement, ProgressionRate.good, level);
        }

        public int GetMaxClassSkillPoints(int level)
        {
            return statProgression.GetStat(CharacterStat.maxClassSkill, ProgressionRate.good, level);
        }

        public int GetMaxNumberOfFeats(int level)
        {
            return statProgression.GetStat(CharacterStat.feats, ProgressionRate.good, level);
        }

        public int GetCumulativeAbilityScoreBonus(int level)
        {
            return statProgression.GetStat(CharacterStat.abilityScoreBonus, ProgressionRate.good, level);
        }


    // Public Methods
        public void ResetSpellCounts(int casterLevel)
        {
            SetSpellsKnown(casterLevel);
            SetSpellsPerDay(casterLevel);
        }


    // Private Methods
        private void SetCasterClass()
        {
            foreach (CasterClass cClass in Enum.GetValues(typeof(CasterClass)))
            {
                if (characterClass.ToString() == cClass.ToString()) { casterClass = cClass; }
            }
        }

        private void SetSpellsKnown(int casterLevel)
        {
            if (!isCaster) { return; }

            spellsKnown = new int[Constant.maxSpellLevel + 1];
            for (int level = 0; level < Constant.maxSpellLevel; level++)
            {
                spellsKnown[level] = spellProgression.GetSpellCount(SpellCountType.spellsKnown, casterClass, casterLevel, level);
            }
        }

        private void SetSpellsPerDay(int casterLevel)
        {
            if (!isCaster) { return; }

            spellsPerDay = new int[Constant.maxSpellLevel + 1];
            for (int level = 0; level < Constant.maxSpellLevel; level++)
            {
                spellsPerDay[level] = spellProgression.GetSpellCount(SpellCountType.spellsPerDay, casterClass, casterLevel, level);
            }
        }
    }
}