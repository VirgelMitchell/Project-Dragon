using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISavable
    {
    // Variables
        [SerializeField] int maximumHP = 1;

        int currentHP;
        bool isDead = false;


    // Basic Methods
        private void Start() { currentHP = maximumHP; }


    // Getter Methods
        public bool GetIsDead() { return isDead; }


    // Public Methods
        public void TakeDamage(int damage)
        {
            if (!isDead)
            {
                currentHP = Mathf.Max(currentHP - damage, 0);
                if (currentHP <= 0) { Die(); }
            }
        }
    

    // Private Methods
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
