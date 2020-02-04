using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] PlayerClass playerClass;
        [Range(0, 20)] [SerializeField] int pcClassStartLevel = 0;

        [Tooltip("Valid fractional CRs are 1/10, 1/8, 1/6, 1/5, 1/4, 1/3, 1/2.\n" +
                 "Leave blank for characters with PC Class Levels")]
        [SerializeField] string challengeRating = "";

        [Header("References")]
        [SerializeField] XPRewardProgression xPRewardProgression;

        int baseHealth = 0;
        int currentLevel = -1;

        CharacterClass characterClass;
        RNG generator;


    // Standard Methods
        private void Awake()
        {
            characterClass = Resources.Load<CharacterClass>("Character Classes/" + playerClass.ToString());
            xPRewardProgression = Resources.Load<XPRewardProgression>(Constant.xPRewardsPath);
            generator = GameObject.Find(Constant.generatorObjectName).GetComponent<RNG>();
            if (currentLevel < 0) { currentLevel = pcClassStartLevel; }
            if (baseHealth <= 0) { baseHealth = GenerateHP(); }
        }


    // Getter Methods
        public int GetLevel()                       { return currentLevel; }
        public int GetCasterLevel()                 { return currentLevel; }
        public bool GetNeedsToLevelUp()             { return currentLevel < GetXPLevel(); }
        public int GetXPReward(int attackerLevel)   { return GenerateXPReward(attackerLevel); }

        public int GetHP()
        {
            if (baseHealth <= 0) { baseHealth = GenerateHP(); }
            return baseHealth;
        }

        public int GetAttackBonus()
        {
            int bonus = characterClass.GetAttackBonus(currentLevel);
            return bonus;
        }

        public int GetAttacksPerRound()
        {
            int bonus = characterClass.GetAttacksPerRound(currentLevel);
            return bonus;
        }

        public int GetSave(SaveType saveType)
        {
            switch (saveType)
            {
                case SaveType.fortitude:
                    int fortbonus = characterClass.GetFortitudeBaseSave(currentLevel);
                    return fortbonus;
                case SaveType.reflex:
                    int reflexbonus = characterClass.GetReflexBaseSave(currentLevel);
                    return reflexbonus;
                case SaveType.will:
                    int willbonus = characterClass.GetWillBaseSave(currentLevel);
                    return willbonus;
                default:
                    Debug.LogError(gameObject.name + ": Invalid Save Type!!");
                    return 0;
            };
        }


        // Private Methods
        private int GenerateXPReward(int attackerLevel)
        {
            if (challengeRating == "") { challengeRating = currentLevel.ToString(); }
            return xPRewardProgression.GetReward(challengeRating, attackerLevel);
        }

        private int GenerateHP()
        {
            int hitDie = characterClass.GetHitDieSize();
            if (currentLevel == 0) { return hitDie / 2; }
            else if (currentLevel == 1) { return hitDie; }
            else
            {
                int hitPoints = hitDie;
                int roll;
                for (int hd = 2; hd <= currentLevel; hd++)
                {
                    roll = generator.GenerateNumber(hitDie);
                    //print("roll " + hd + ": " + roll);
                    hitPoints += roll;
                }
                return hitPoints;
            }
        }

        private int GetXPLevel()
        {
            if (currentLevel > 19) { return 20; }
            int currentXP = GetComponent<ExperienceTracker>().GetXP();
            int level = currentLevel;

            bool levelIsMaxed = false;
            while (!levelIsMaxed)
            {
                int goal = characterClass.GetNextLevel(level);
                if (goal < currentXP)
                {
                    levelIsMaxed = true;
                    continue;
                }
                level++;
            }
            return level;
        }
    }

}