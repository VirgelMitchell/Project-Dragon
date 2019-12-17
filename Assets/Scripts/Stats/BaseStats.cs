using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] XPRewardProgression xPRewardProgression;
        [SerializeField] PlayerClass playerClass;
        [Range(0, 20)] [SerializeField] int pcClassStartLevel = 0;

        [Tooltip("Valid fractional CRs are 1/10, 1/8, 1/6, 1/5, 1/4, 1/3, 1/2.\n" +
                 "Leave blank for characters with PC Class Levels")]
        [SerializeField] string challengeRating = "";

        int baseHealth = 0;
        int currentLevel = -1;

        CharacterClass characterClass;

        public int GetHP()                  { return baseHealth; }
        public int GetLevel()               { return currentLevel; }
        public int GetCasterLevel()         { return currentLevel; }
        public bool GetNeedsToLevelUp()     { return currentLevel < GetXPLevel(); }

        public int GetXPReward(int attackerLevel)
        {
            if (challengeRating == "") { challengeRating = currentLevel.ToString(); }
            return xPRewardProgression.GetReward(challengeRating, attackerLevel);
        }

        public int GetAttacksPerRound()
        {
            return characterClass.GetAttacksPerRound(currentLevel);
        }

        public int GetSave(SaveType saveType)
        {
            switch (saveType)
            {
                case SaveType.fortitude:
                    return characterClass.GetFortitudeBaseSave(currentLevel);
                case SaveType.reflex:
                    return characterClass.GetReflexBaseSave(currentLevel);
                case SaveType.will:
                    return characterClass.GetReflexBaseSave(currentLevel);
                default:
                    Debug.LogError("Invalid Save Type!!");
                    return 0;
            };
        }

        private void Awake()
        {
            characterClass = Resources.Load<CharacterClass>("Character Classes/" + playerClass.ToString());

            if (currentLevel < 0) { currentLevel = pcClassStartLevel; }
            if (baseHealth <= 0) { baseHealth = GenerateHP(); }
        }

        private int GenerateHP()
        {
            int hitDie = characterClass.GetHitDieSize();
            if (currentLevel == 0) { return hitDie / 2; }
            else if (currentLevel == 1) { return hitDie; }
            else
            {
                RNG generator = GameObject.FindObjectOfType<RNG>();
                int hitPoints = hitDie;
                for (int hd = 2; hd <= currentLevel; hd++)
                {
                    hitPoints += generator.GenerateNumber(hitDie);
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