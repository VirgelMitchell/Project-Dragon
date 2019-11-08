using RPG.Core;
using RPG.Magic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Spell", menuName = "Spell/New Spell", order = 0)]
    public class Spell : ScriptableObject
    {
        [Header("General Info")]
        [SerializeField] string spellName = "";
        [SerializeField] CasterClass[] casterClasses;
        [SerializeField] School school;
        [Range(0f, 10f)][SerializeField] int spellLevel = 0;

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
        [SerializeField] int maxDamagePerLevel = 0;

        [Header("Prefabs")]
        [SerializeField] GameObject effectPrefab = null;
        [SerializeField] AnimatorOverrideController animationController = null;

        CastSpell castSpell;
        Transform tempObject;
        RNG generator;
        string generatorObjName = "RandomeNumberGenerator";

        const float longRange = 121.92f;
        const float lRBonus = 12.192f;
        const float closeRange = 7.62f;
        const float cRBonus = 1.524f;
        const float mediumRange = 30.48f;
        const float mRBonus = 3.048f;
        const float touchRange = 1.25f;
        const float speed = 20f;

        const string tempGameObjName = "Temperary Objects";


    // Basic Methods
        private void Start()
        {}


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
        public void EquipSpell(Transform[] hands, Animator animator)
        {
            if (!effectPrefab || !animationController)
            {
                Debug.LogError("ORIO: Are you missing the Effect Prefab or Animation Override Controller");
                return;
            }
            GameObject spell = Instantiate(new GameObject(spellName), hands[0]);
            spell.transform.parent = hands[0].transform;
            spell.name = spellName;
            animator.runtimeAnimatorController = animationController;
        }

        public void CastSpell ( GameObject instigater, Transform target,
                                Transform[] hands, int level)
        {
            if (spellLevel < level) { return; }
            Vector3 startLoc = hands[0].position;
            GameObject spellObject = Instantiate(effectPrefab, startLoc, Quaternion.identity);
            spellObject.transform.parent = GameObject.Find(tempGameObjName).transform;
            CastSpell spellInstance = spellObject.GetComponent<CastSpell>();
            spellInstance.InitializeTargeting(target.GetComponent<Health>(), numberOfTargets, cantMiss, attackType, areaOfEffect);
            spellInstance.InitializeSpell(CalculateDamage(level), GetRange(level), speed, instigater);
            // TODO decrease mana/magical energy count in inventory
        }

        public void DestoySpell(Transform[] hands, Spell spell)
        {
            Transform oldSpell = hands[0].Find(spellName);
            if (oldSpell == null) { oldSpell = hands[1].Find(spellName); }
            if (oldSpell == null)
            {
                Debug.LogWarning(spellName + "Not Found");
                return;
            }
            oldSpell.name = "OldSpell";
            Destroy(oldSpell.gameObject);
        }


    // Private Methods
        private int CalculateDamage(int level)
        {
            generator = GameObject.Find(generatorObjName).GetComponent<RNG>();

            int damage = 0;
            for (int count = 0; count < level; count++)
            {
                damage += generator.GenerateNumber(maxDamagePerLevel);
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