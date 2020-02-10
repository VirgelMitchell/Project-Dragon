using RPG.Core;
using RPG.Stats;
using UnityEngine;

 namespace RPG.GUI
 {
    public class LevelUpRoutine : MonoBehaviour
    {
        [SerializeField] GameObject rNGPrefab;

        float restNeeded = 0f;

        GameObject player;
        ExperienceTracker xPTracker;
        BaseStats baseStats;
        Health health;
        CharacterClass characterClass;
        RNG generator;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            xPTracker = player.GetComponent<ExperienceTracker>();
            baseStats = player.GetComponent<BaseStats>();
            health = player.GetComponent<Health>();
            characterClass = baseStats.GetCharacterClass();
            generator = GameObject.Instantiate(rNGPrefab).GetComponent<RNG>();
            MainLoop();
        }

        private void MainLoop()
        {
            int numberOfLevelsToAdd = CalculateLevels();
            restNeeded = numberOfLevelsToAdd * TimeDefination.hour;
            for (int levelsAdded = 0; levelsAdded < numberOfLevelsToAdd; levelsAdded++)
            {
                baseStats.AddLevel();
                AddHealth();
                AddSkillPoints(characterClass.GetSkillPoints());
                AddFeats();
                AddAbilityBonus();
            }
            xPTracker.SetXPGoal(baseStats.GetStat(CharacterStat.xPRequirement));

            // TODO: add restNeeded to elapsedGameTime
        }

        private int CalculateLevels()
        {
            int level = baseStats.GetLevel();
            int goal = baseStats.GetStat(CharacterStat.xPRequirement);
            int currentXP = xPTracker.GetXP();

            int levels = 0;
            while (goal < currentXP)
            {
                levels++;
                goal = baseStats.GetStat(CharacterStat.xPRequirement);
            }

            return levels;
        }

        private void AddHealth()
        {
            int points = generator.GenerateNumber(characterClass.GetHitDieSize());
            health.AddHealth(points);
            baseStats.AddHealth(points);
        }

        private void AddSkillPoints(int skillPoints)
        {
            // TODO: AddSkillPoints()

            // load skill list (probably in inventory section of course)
            // allow player to add points to existing skills or new ones
                // should contain check to max skill points per level is not exceeded for each skill
                // needs to include functionality to give only 1/2 point to Cross Class Skills per SP
        }

        private void AddFeats()
        {
            // TODO: AddFeats()

            // check to see if any feats are available this level (probably in inventory section of course)
            // if feat is available player picks new feat an bonuses added
        }

        private void AddAbilityBonus()
        {
            // TODO: AddAbilityBonus()

            // if level is devisible by 4 add 1 point to any one ability score (STR, DEX, CON, INT, WIS, CHR)
            // suggest primary ability
            // (probably in inventory section of course)
        }
    }
}