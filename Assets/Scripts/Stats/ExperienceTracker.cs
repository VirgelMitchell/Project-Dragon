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


    // ISabable Implementation
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