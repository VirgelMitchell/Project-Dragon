using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float attackRange = 1.5f;
        [SerializeField] float numOfAttacks = 1f;
        [SerializeField] float weaponDamage = 1f;
        [SerializeField] float strBonus = 0f;
        
        Transform opponent;
        float timeSinceLastAttack = Mathf.Infinity;
        
        
        //Public Methods
        public bool CanAttack(GameObject candidate)
        {
            if (candidate == null) { return false; }
            Health health = candidate.GetComponent<Health>();
            return health != null && !health.GetIsDead();
        }

        public void Attack(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            opponent = target.transform;
        }

        public void LookAt(Vector3 position)
        {
            transform.LookAt(position);
        }

        public void CancelAction()
        {
            opponent = null;
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Mover>().CancelAction();

        }


        // Private Methods
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (opponent == null) { return; }
            
            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(opponent.position);
            }
            else
            {
                GetComponent<Mover>().CancelAction();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            float timeBetweenAttacks = 6f / numOfAttacks;

            GetComponent<Animator>().ResetTrigger("stopAttack");
            LookAt(opponent.position);
            if (timeSinceLastAttack < timeBetweenAttacks) { return; }
            GetComponent<Animator>().SetTrigger("attack");
            timeSinceLastAttack = 0f;
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, opponent.position) < attackRange;
        }

        // Animation triggered events
        private void Hit()
        {
            if (opponent == null) { return; }
            Health enemyHealth = opponent.GetComponent<Health>();
            DealDamage(enemyHealth);
            enemyHealth.GetComponent<Fighter>().LookAt(transform.position);
            if (enemyHealth.GetIsDead())
            {
                opponent.GetComponent<Animator>().SetTrigger("onDeath");
                CancelAction();
            }
        }

        private void DealDamage(Health enemyHealth)
        {
            float damage = weaponDamage + strBonus;
            enemyHealth.TakeDamage(damage);
        }
    }
}
