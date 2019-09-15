using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
    // Variables
        [SerializeField] int numOfAttacks = 1;
        [SerializeField] int strBonus = 0;
        [Tooltip("0 =Right hand; 1 = Left hand")]
        [SerializeField] Transform[] hands = new Transform[2];
        [SerializeField] Weapon defaultWeapon = null;

        float timeSinceLastAttack = Mathf.Infinity;

        Animator animator = null;
        Weapon currentWeapon = null;
        Transform opponent = null;

        // Constants
        const float meleeRound = 6f;


    // Basic Methods        
        private void Awake() { animator = GetComponent<Animator>(); }
        private void Start() { EquipWeapon(defaultWeapon); }

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


    // Getter Methods
        public bool GetIsArmed() { return currentWeapon; }


    // Public Methods
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

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) { return; }
            weapon.EquipWeapon(hands, animator);
            currentWeapon = weapon;
        }

        public void DestroyWeapon()
        {
            currentWeapon.DestoyWeapon(hands);
            currentWeapon = null;
        }
        
        
    // Private Methods
        private void AttackBehavior()
        {
            float weaponSpeed= currentWeapon.GetSpeed();
            float timeBetweenAttacks = meleeRound / Mathf.Min(numOfAttacks, weaponSpeed);

            animator.ResetTrigger("stopAttack");
            LookAt(opponent.position);
            if (timeSinceLastAttack < timeBetweenAttacks) { return; }
            animator.SetTrigger("attack");
            timeSinceLastAttack = 0f;
        }

        private bool IsInRange()
        {
            float dist2Tgt = Vector3.Distance(transform.position, opponent.position);
            return dist2Tgt < currentWeapon.GetRange();
        }


    // IAction Implamentation
        public void CancelAction()
        {
            opponent = null;
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Mover>().CancelAction();
        }


    // Animation triggered events
        private void Hit()
        {
            if (opponent == null) { return; }
            Health enemyHealth = opponent.GetComponent<Health>();
            DealDamage(enemyHealth);
            enemyHealth.GetComponent<Fighter>().LookAt(transform.position);
            if (enemyHealth.GetIsDead()) { CancelAction(); }
        }

        private void DealDamage(Health enemyHealth)
        {
            enemyHealth.TakeDamage(currentWeapon.GetDamage(strBonus));
        }

        public void Shoot()
        {
            currentWeapon.FireProjectile(hands, opponent, strBonus);
            if (opponent.GetComponent<Health>().GetIsDead()) { CancelAction(); }
        }
    }
}
