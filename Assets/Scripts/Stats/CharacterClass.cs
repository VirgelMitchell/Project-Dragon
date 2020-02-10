using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "CharacterClass", menuName = "Progression/New Character Class", order = 0)]
    public class CharacterClass : ScriptableObject
    {
        // RPG.Core.Enumerators.PlayerClass
        [SerializeField] PlayerClass characterClass = PlayerClass.barbarian;
        [SerializeField] bool isCaster = false;
        [SerializeField] int hitDieSize = 4;
        [SerializeField] int skillPointsPerLevel = 2;

        [Header("Progression Rates")]   // RPG.Core.Enumerators.ProgressionRate
        [SerializeField] ProgressionRate attackProgressionRate;
        [SerializeField] ProgressionRate fortitudeSaveProgressionRate;
        [SerializeField] ProgressionRate reflexSaveProgressionRate;
        [SerializeField] ProgressionRate willSaveProgressionRate;

        int[] spellsKnown;
        int[] spellsPerDay;

        CasterClass casterClass;
        SpellProgression spellProgression = null;
        StatProgression statProgression = null;


    // Standard Methods
        private void Awake()
        {
            statProgression = Resources.Load<StatProgression>(ResourcePath.statProgresssionPath);
            if (isCaster)
            {
                SetCasterClass();
                spellProgression = Resources.Load<SpellProgression>(ResourcePath.spellProgresssionPath);
                if (spellsKnown == null || spellsPerDay == null) { ResetSpellCounts(0); }
            }
        }


    // Getter Methods
        public PlayerClass GetClass()   { return characterClass; }
        public int GetHitDieSize()      { return hitDieSize; }
        public int GetSkillPoints()     { return skillPointsPerLevel; }
        public int[] GetSpellsKnown()   { return spellsKnown; }
        public int[] GetSpellsPerDay()  { return spellsPerDay; }

        public int GetStat(CharacterStat stat, int level)
        {
            ProgressionRate rate;
            switch (stat)
            {
                case CharacterStat.attackBonus:
                    rate = attackProgressionRate;
                    break;
                case CharacterStat.attacksPerRound:
                    rate = attackProgressionRate;
                    break;
                default:
                    rate = ProgressionRate.good;
                    break;
            }
            return statProgression.GetStat(stat, rate, level);
        }

        public int GetBaseSave(SaveType save, int level)
        {
            CharacterStat stat = CharacterStat.baseSave;
            ProgressionRate rate;
            switch (save)
            {
                case SaveType.fortitude:
                    rate = fortitudeSaveProgressionRate;
                    break;
                case SaveType.reflex:
                    rate = reflexSaveProgressionRate;
                    break;
                case SaveType.will:
                    rate = willSaveProgressionRate;
                    break;
                default:
                    Debug.LogError("Invalid Save Type!");
                    return 0;
            }
            return statProgression.GetStat(stat, rate, level);
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