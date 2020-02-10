using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceTracker : MonoBehaviour, ISavable
    {
        [SerializeField] int experiencePoints = 0;

        int xPGoal;
        bool needsToLevelUp = false;


        // Public Methods
        public int GetXP()                  { return experiencePoints; }
        public int GetXPGoal()              { return xPGoal; }
        public bool GetNeedsToLevelUp()     { return needsToLevelUp; }

        public void RewardXP(int xpReward)
        {
            experiencePoints += xpReward;
            CheckNeedToLevelUp();
        }

        public void SetXPGoal(int goal)
        {
            xPGoal = goal;
            CheckNeedToLevelUp();
        }

        // Private Methods
        private void CheckNeedToLevelUp()   { needsToLevelUp = experiencePoints > xPGoal; }


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