using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float hitPoints;
        bool isDead = false;

        public bool GetIsDead()     { return isDead; }

        public void TakeDamage(float damage)
        {
            if (!isDead)
            {
                hitPoints = Mathf.Max(hitPoints - damage, 0);
                if (hitPoints <= 0)
                {
                    GetComponent<Animator>().SetTrigger("onDeath");
                    GetComponent<ActionScheduler>().CancelAction();
                    isDead = true;
                }
            }
        }
    }

}
