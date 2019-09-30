using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] Progression progression = null;
        [SerializeField] CharacterClass characterClass;
        [Range(0, 20)] [SerializeField] int startingLevel = 0;

        int baseHealth = 50;
        int currentLevel = 5;

        public int GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, startingLevel);
        }
    }
}