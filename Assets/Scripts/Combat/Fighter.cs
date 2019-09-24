using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        // Variables
        [SerializeField] int characterLevel = 0;
        [SerializeField] int numOfAttacks = 1;
        [SerializeField] int strBonus = 0;
        [Tooltip("0 =Right hand; 1 = Left hand")]
        [SerializeField] Transform[] hands = new Transform[2];
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Spell defaultSpell = null;

        float timeSinceLastAttack = Mathf.Infinity;

        Animator animator = null;
        Weapon currentWeapon = null;
        Spell currentSpell = null;
        Transform opponent = null;

        // Constants
        const float meleeRound = 6f;


    // Basic Methods        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            EquipWeapon(defaultWeapon);
            EquipSpell(defaultSpell);
        }

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
        public bool GetIsArmed() { return currentWeapon || currentSpell; }


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

        public void EquipSpell(Spell spell)
        {
            if (spell == null) { return; }
            spell.EquipSpell(hands, animator);
            currentSpell = spell;
        }
        
        public void DestroyWeapon()
        {
            if (currentWeapon)
            {
                Debug.Log("destroying" + currentWeapon);
                currentWeapon.DestoyWeapon(hands);
            }
            currentWeapon = null;
            if (currentSpell)
            {
                Debug.Log("destroying" + currentSpell);
                currentSpell.DestoySpell(hands);
            }
            currentSpell = null;
        }
        
        
    // Private Methods
        private void AttackBehavior()
        {
            if (opponent.GetComponent<Health>().GetIsDead())
            {
                CancelAction();
                return;
            }

            float weaponSpeed;
            if (currentWeapon) { weaponSpeed = currentWeapon.GetSpeed(); }
            else { weaponSpeed = currentSpell.GetSpeed(); }
            float timeBetweenAttacks = meleeRound / Mathf.Min(numOfAttacks, weaponSpeed);
            if (timeSinceLastAttack < timeBetweenAttacks) { return; }
            
            animator.ResetTrigger("stopAttack");
            LookAt(opponent.position);
            animator.SetTrigger("attack");
            timeSinceLastAttack = 0f;
        }

        private bool IsInRange()
        {
            float dist2Tgt = Vector3.Distance(transform.position, opponent.position);
            float attackRange = 0f;
            if (currentWeapon) { attackRange = currentWeapon.GetRange(); }
            else if (currentSpell) { attackRange = currentSpell.GetRange(characterLevel); }
            return dist2Tgt < attackRange;
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
            if (currentSpell)
            {
                currentSpell.CastSpell(hands, opponent, characterLevel);
            }
            else
            {
                Health enemyHealth = opponent.GetComponent<Health>();
                DealDamage(enemyHealth);
                enemyHealth.GetComponent<Fighter>().LookAt(transform.position);
            }
        }

        private void DealDamage(Health enemyHealth)
        {
            enemyHealth.TakeDamage(currentWeapon.GetDamage(strBonus));
        }

        public void Shoot()
        {
            currentWeapon.FireProjectile(hands, opponent, strBonus);
        }
    }
}
