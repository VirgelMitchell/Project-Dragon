using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.GUI
{
    public class PlayerXPDisplay : MonoBehaviour
    {
        int currentXP;
        int nextLevelGoal;
        float xPDecimal;

        BaseStats playerBaseStats;
        Text healthText;

        private void Awake()
        {
            playerBaseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            healthText = GetComponent<Text>();
        }

        private void Update()
        {
            currentXP = playerBaseStats.GetCurrentXP();
            nextLevelGoal = GetGoal();
            xPDecimal = (float)currentXP/nextLevelGoal;
            //if (currentXP > nextLevelGoal) { xPDecimal = 1; }

            print("Current XP: " + currentXP);
            print("XP Goal: " + nextLevelGoal);
            print("XP Decimal: " + xPDecimal);

            healthText.text = string.Format("XP: {0:p0}", xPDecimal);
        }

        int GetGoal()
        {
            CharacterClass characterClass = playerBaseStats.GetCharacterClass();
            return characterClass.GetNextLevel(playerBaseStats.GetLevel());
        }
    }
}
