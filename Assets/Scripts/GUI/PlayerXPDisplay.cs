using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.GUI
{
    public class PlayerXPDisplay : MonoBehaviour
    {
        ExperienceTracker xPTracker;
        Text xPText;

        private void Awake()
        {
            xPTracker = GameObject.FindWithTag("Player").GetComponent<ExperienceTracker>();
            xPText = GetComponent<Text>();
        }

        private void Update()
        {
            int currentXP = xPTracker.GetXP();
            int nextLevelGoal = xPTracker.GetXPGoal();
            float xPDecimal = (float)currentXP/nextLevelGoal;
            if (currentXP > nextLevelGoal) { xPDecimal = 1f; }
            xPText.text = string.Format("XP: {0:p0}", xPDecimal);
        }
    }
}
