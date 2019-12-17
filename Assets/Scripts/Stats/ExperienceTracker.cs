using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceTracker : MonoBehaviour, ISavable
    {
        [SerializeField] int experiencePoints = 0;


    // Public Methods
        public void RewardXP(int xpReward)
        {
            experiencePoints += xpReward;
        }

        public int GetXP() { return experiencePoints; }


    // ISavable Implementation
        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (int)state;
        }
    }
}