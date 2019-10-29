using RPG.Combat;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{
    public class Health : MonoBehaviour, ISavable
    {
    // Variables
        [SerializeField] int baseHP = 0;
        [SerializeField] int currentHP;
        [SerializeField] float rememberLastAttackerTime = 5f;

        GameObject attacker = null;
        PlayerController playerController = null;
        AIController enemyController = null;
        bool isDead = false;


    // Basic Methods
        private void Awake()
        {
            if (gameObject.tag == "Player")
            {
                playerController = GetComponent<PlayerController>();
            }
            else
            {
                enemyController = GetComponent<AIController>();
            }
        }

        private void Start()
        {
            baseHP = GetComponent<BaseStats>().GetHealth();
            if (currentHP == 0 && !isDead) { currentHP = baseHP; }
        }

        private void Update()
        {
            if(playerController)
            {
                if(playerController.GetTimeSinceLastAttacked() > rememberLastAttackerTime)
                {
                    attacker = null;
                }
            }
            else if (enemyController)
            {
                if (enemyController.GetTimeSinceLastAttacked() > rememberLastAttackerTime)
                {
                    attacker = null;
                }
            }
            else { Debug.LogWarning("Warning! Uncontrolled Attacker!!"); }
        }


    // Getter Methods
        public bool GetIsDead()         { return isDead; }

        public float GetHP()
        {
            return (float)currentHP / GetComponent<BaseStats>().GetHealth();
        }


    // Public Methods
        public void TakeDamage(GameObject instigator, int damage)
        {
            if (isDead) { return; }

            UpdateAttacker(instigator);
            currentHP = Mathf.Max(currentHP - damage, 0);

            if (currentHP <= 0)
            {
                Die();
                AwardExperience();
            }
        }


    // Private Methods
        private void UpdateAttacker(GameObject instigator)
        {
            if (attacker == null) { attacker = instigator; }
            if (attacker == instigator)
            {
                if (playerController)
                {
                    playerController.ResetTimeSinceAttacked();
                }
                else if (enemyController)
                {
                    enemyController.ResetTimeSinceAttacked();
                }
                else
                {
                    Debug.LogWarning("WARNING! WARNING! UNCONTOLLED ENTITY DETECTED");
                }
            }
        }

        private void AwardExperience()
        {
            if (!attacker.GetComponent<ExperienceTracker>()) { return; }
            int instigatorLevel = attacker.GetComponent<BaseStats>().GetLevel();
            int xpReward = GetComponent<BaseStats>().GetXPReward(instigatorLevel);
            attacker.GetComponent<ExperienceTracker>().RewardXP(xpReward);
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("onDeath");
            GetComponent<ActionScheduler>().CancelAction();
            isDead = true;
        }


    // ISavable Implamentation

        public object CaptureState()
        {
            return currentHP;
        }

        public void RestoreState(object state)
        {
            currentHP = (int)state;
            if (currentHP <= 0) { Die(); }
        }
    }
}
