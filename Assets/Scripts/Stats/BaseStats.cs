using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, ISavable
    {
        [SerializeField] PlayerClass playerClass = PlayerClass.barbarian;
        [SerializeField] XPRewardProgression xpRewardProgression = null;

        [Range(0, 20)] [SerializeField] int pcClassStartLevel = 0;

        [Tooltip("Valid fractional CRs are 1/10, 1/8, 1/6, 1/5, 1/4, 1/3, 1/2.\n" +
                 "Leave blank for characters with PC Class Levels")]
        [SerializeField] string challengeRating = "";

        int baseHealth = 0;
        int currentLevel = -1;

        CharacterClass characterClass;
        RNG generator;

        void Awake()
        {
            characterClass = Resources.Load<CharacterClass>("Character Classes/" + playerClass.ToString());

            if (currentLevel < 0) { currentLevel = pcClassStartLevel; }
            if (baseHealth <= 0) { baseHealth = GenerateHP(); }
        }

        private void Start()
        {
            generator = GameObject.Find("RandomeNumberGenerator").GetComponent<RNG>();
        }

        public int GetLevel()               { return currentLevel; }
        public int GetHealth()              { return baseHealth; }

        public int GetAttacksPerRound()
        {
           return characterClass.GetAttacksPerRound(currentLevel);
        }

        public int GetXPReward(int attackerLevel)
        {
            if (challengeRating == "") { challengeRating = currentLevel.ToString(); }
            return xpRewardProgression.GetReward(challengeRating, attackerLevel);
        }

        public void LevelUp()
        {
            currentLevel++;
            baseHealth += generator.GenerateNumber(characterClass.GetHitDieSize());
        }

        private int GenerateHP()
        {
            int hp;
            if (currentLevel < 0) { hp = 0; }
            else if (currentLevel <= 1) { hp = characterClass.GetHitDieSize(); }
            else
            {
                int hitDie = characterClass.GetHitDieSize();
                hp = hitDie;
                for (int dice = 2; dice <= currentLevel; dice++)
                {
                    hp+= generator.GenerateNumber(hitDie);
                }
            }
            return hp;
        }


    // ISavable Implamentation
        public object CaptureState()
        {
            Dictionary<string, int> state = new Dictionary<string, int>();

            state["playerClass"] = (int)playerClass;
            state["baseHealth"] = baseHealth;
            state["currentLevel"] = currentLevel;

            Debug.Log("BaseStats saving playerClass as " + state["playerClass"] +
                      ", baseHealth as " + state["baseHealth"] +
                      ", and currentLevel as " + state["currentLevel"]);

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, int> restoredState = (Dictionary<string, int>)state;

            Debug.Log("Save File returned playerClass as " + (PlayerClass)restoredState["playerClass"] +
                      ", baseHealth as " + restoredState["baseHealth"] +
                      ", and currentLevel as " + restoredState["currentLevel"]);

            playerClass = (PlayerClass)restoredState["playerClass"];
            baseHealth = restoredState["baseHealth"];
            currentLevel = restoredState["currentLevel"];
        }
    }
}