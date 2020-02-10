using RPG.Core;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISavable
    {
    // Variables
        [SerializeField] int strBonus = 0;
        [Tooltip("0 = Right hand; 1 = Left hand")]
        [SerializeField] Transform[] hands = new Transform[2];
        [SerializeField] string defaultWeaponName = "Unarmed Attack";
        [SerializeField] string defaultSpellName = "";

        float timeSinceLastAttack = Mathf.Infinity;

        Animator    animator = null;
        BaseStats   baseStats;
        GameObject  me;
        Spell       currentSpell = null;
        Transform   opponent = null;
        Weapon      currentWeapon = null;


    // Basic Methods
        private void Awake()
        {
            animator = GetComponent<Animator>();
            me = this.gameObject;
            baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            if (currentSpell == null && currentWeapon == null)
            {
                Weapon defaultWeapon = Resources.Load<Weapon>(ResourcePath.weaponResourcePath + defaultWeaponName);
                Spell defaultSpell = Resources.Load<Spell>(ResourcePath.spellResourcePath + defaultSpellName);

                if (defaultWeaponName != "") { EquipWeapon(defaultWeapon); }
                else if (defaultSpellName != "") { EquipSpell(defaultSpell); }
            }
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
        public bool GetIsArmed()        { return currentWeapon || currentSpell; }
        public Transform GetTarget()    { return opponent; }


        // Public Methods
        public bool isValidTarget(GameObject candidate)
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
                currentWeapon.DestoyWeapon(hands);
            }
            currentWeapon = null;
            if (currentSpell)
            {
                currentSpell.DestoySpell(hands, currentSpell);
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

            if (CanAttack())
            {
                animator.ResetTrigger("stopAttack");
                LookAt(opponent.position);
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        private bool CanAttack()
        {
            float weaponSpeed;
            if (currentWeapon) { weaponSpeed = currentWeapon.GetSpeed(); }
            else { weaponSpeed = currentSpell.GetSpeed(); }

            int numOfAttacks = baseStats.GetStat(CharacterStat.attacksPerRound);
            float timeBetweenAttacks = Constant.meleeRound / Mathf.Min(numOfAttacks, weaponSpeed);
            if (timeSinceLastAttack > timeBetweenAttacks) { return true; }
            else { return false; }
        }

        private bool IsInRange()
        {
            float dist2Tgt = Vector3.Distance(transform.position, opponent.position);
            float attackRange = 0f;
            if (currentWeapon)
            {
                attackRange = currentWeapon.GetRange();
            }
            else if (currentSpell)
            {
               attackRange = currentSpell.GetRange(baseStats.GetCasterLevel());
            }
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


    // ISavable Implamentation
        public object CaptureState()
        {
            if (currentWeapon) { return currentWeapon.name; }
            else if (currentSpell) { return currentSpell.name; }
            else { return "Unarmed Attack"; }
        }

        public void RestoreState(object state)
        {
            Weapon weapon = Resources.Load<Weapon>(state.ToString());
            if (weapon) { EquipWeapon(weapon); }
            else
            {
                Spell spell = Resources.Load<Spell>(state.ToString());
                if (spell) { EquipSpell(spell); }
                else { EquipWeapon(Resources.Load<Weapon>("Unarmed Attack")); }
            }
        }


    // Animation triggered events
        private void Hit()
        {
            if (opponent == null) { return; }
            if (currentSpell)
            {
               currentSpell.CastSpell(me, opponent, hands, baseStats.GetCasterLevel());
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
            enemyHealth.TakeDamage(me, currentWeapon.GetDamage(strBonus));
        }

        public void Shoot()
        {
            currentWeapon.FireProjectile(me, opponent, hands, strBonus);
        }
    }
}

