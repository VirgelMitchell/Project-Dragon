using System.Collections;
using RPG.Core;
using RPG.Magic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Spell", menuName = "Spell/New Spell", order = 0)]
    public class Spell : ScriptableObject
    {
        [Header("General Info")]
        [SerializeField] string spellName = "";
        [SerializeField] string[] casterClass;
        [SerializeField] int spellLevel = 0;

        [Header("Stats")]
        [SerializeField] CastingSpeed castingSpeed = CastingSpeed.other;
        [Tooltip("in seconds")] [SerializeField] float timeToCast = 0f;
        [SerializeField] Range range = Range.personal;
        [SerializeField] float givenRange = 0f;
        [SerializeField] AttackType attackType = AttackType.ranged;
        [Tooltip("Radius of spell effect")][SerializeField] float areaOfEffect = 0f;
        [SerializeField] int numberOfTargets = 0;
        [SerializeField] bool cantMiss = false;
        [SerializeField] bool isInstant = true;
        [SerializeField] float durration = 0f;
        [SerializeField] int minDamagePerLevel = 0;
        [SerializeField] int maxDamagePerLevel = 0;

        [Header("Prefabs")]
        [SerializeField] GameObject effectPrefab = null;
        [SerializeField] AnimatorOverrideController animationController = null;

        enum AttackType { area, ranged, touch };
        enum CasterClass{cleric, druid, palidine, ranger, sorceror, wizard}
        enum CastingSpeed { full, instant, standard, other };
        enum Range { close, far, given, medium, personal, touch };
        enum School { evokation };
        
        Fighter caster = null;
        CastSpell castSpell;
        Transform tempObject;

        const float longRange = 121.92f;
        const float lRBonus = 12.192f;
        const float closeRange = 7.62f;
        const float cRBonus = 1.524f;
        const float mediumRange = 30.48f;
        const float mRBonus = 3.048f;
        const float touchRange = 1.25f;
        const float speed = 20f;


        // Basic Methods
        private void Awake()
        {
            castSpell = effectPrefab.GetComponent<CastSpell>();
            tempObject = FindObjectOfType<TempObject>().transform;
        }


        // Getter Methods
        public float GetRange(int level)
        {
            switch (range)
            {
                case Range.close:
                    return closeRange + (cRBonus * (level / 2));

                case Range.far:
                    return longRange + (lRBonus * level);

                case Range.given:
                    return givenRange;

                case Range.medium:
                    return mediumRange + (mRBonus * level);

                case Range.touch:
                    return touchRange;

                case Range.personal:
                    return 0f;

                default:
                    Debug.LogWarning("Invalid Range passed to CastSpell()!!");
                    return 0f;
            }
        }

        public float GetSpeed() { return GetWaitTime(); }


        // Public Methods
        public void SetCaster(Fighter fighter) { caster = fighter; }

        public void EquipSpell(Transform[] hands, Animator animator)
        {
            if (!effectPrefab || !animationController)
            {
                Debug.LogError("ORIO: Are you missing the Effect Prefab or Animation Override Controller");
                return;
            }
            GameObject spell = Instantiate(new GameObject(spellName), hands[0]);
            spell.transform.parent = hands[0].transform.parent;
            animator.runtimeAnimatorController = animationController;
        }

        public void CastSpell(Transform[] hands, Transform target, int level)
        {
            if (spellLevel < level) { return; }
            Vector3 startLoc = hands[0].position;
            GameObject spellObject = Instantiate(effectPrefab, startLoc, Quaternion.identity);
            spellObject.transform.parent = tempObject;
            CastSpell spellInstance = spellObject.GetComponent<CastSpell>();
            spellInstance.SetTarget(target.GetComponent<Health>(), cantMiss);
            spellInstance.SetArea(areaOfEffect);
            spellInstance.SetRange(GetRange(level));
            spellInstance.SetSpeed(speed);
            spellInstance.SetDamage(CalculateDamage(level));
            spellInstance.SetType(attackType.ToString(), numberOfTargets);
            // TODO decrease mana/magical energy count in inventory
        }

        public void DestoySpell(Transform[] hands)
        {
            Transform oldSpell = hands[0].Find(spellName);
            if (oldSpell == null) { oldSpell = hands[1].Find(spellName); }
            if (oldSpell == null) { return; }
            oldSpell.name = "OldSpell";
            Destroy(oldSpell.gameObject);
        }


        // Private Methods
       private int CalculateDamage(int level)
        {
            int damage = 0;
            for (int count = 0; count < level; count++)
            {
                damage += (int)Random.Range(minDamagePerLevel, maxDamagePerLevel + 1f);
            }
            return damage;
        }

        private float GetWaitTime()
        {
            switch (castingSpeed)
            {
                case CastingSpeed.full:
                    return 6f;
                case CastingSpeed.instant:
                    return 0.1f;
                case CastingSpeed.standard:
                    return 3f;
                case CastingSpeed.other:
                    return timeToCast;
                default:
                    Debug.LogWarning("Invalid casting time passed to GetWaitTime()");
                    return Mathf.Infinity;
            }
        }
    }
}