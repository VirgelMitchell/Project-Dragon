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

        int baseHitPoints = 0;
        int currentLevel = -1;

        CharacterClass characterClass;
        RNG generator;


    // Standard Methods
        private void Awake()
        {
            characterClass = Resources.Load<CharacterClass>("Character Classes/" + playerClass.ToString());
            xPRewardProgression = Resources.Load<XPRewardProgression>(ResourcePath.xPRewardsPath);
            generator = GameObject.Find(Constant.generatorObjectName).GetComponent<RNG>();
            if (currentLevel < 0) { currentLevel = pcClassStartLevel; }
            if (baseHitPoints <= 0) { baseHitPoints = GenerateHP(); }
            if (gameObject.tag == "Player")
                { GetComponent<ExperienceTracker>().SetXPGoal(GetStat(CharacterStat.xPRequirement)); }
        }


    // Getter Methods
        public int GetCasterLevel()                 { return currentLevel; }
        public CharacterClass GetCharacterClass()   { return characterClass; }
        public int GetLevel()                       { return currentLevel; }
        public int GetXPReward(int attackerLevel)   { return GenerateXPReward(attackerLevel); }

        public bool NeedsToLevelUp()
            { return GetComponent<ExperienceTracker>().GetNeedsToLevelUp(); }

        public int GetStat(CharacterStat stat)
            { return characterClass.GetStat(stat, currentLevel); }

        public int GetSave(SaveType save)
            { return characterClass.GetBaseSave(save, currentLevel); }

        public int GetBaseHP()
        {
            if (baseHitPoints <= 0) { baseHitPoints = GenerateHP(); }
            return baseHitPoints;
        }

        public void AddLevel() { currentLevel++; }
        public void AddHealth(int points) { baseHitPoints += points; }


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
    }
}