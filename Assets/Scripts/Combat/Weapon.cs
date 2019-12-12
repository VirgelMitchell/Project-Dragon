using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
    // Variables
        [Header("Prefabs")]
        [SerializeField] AnimatorOverrideController weapAnimControl = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] Projectile projectile = null;

        [Header("Stats")]
        [SerializeField] bool is2Handed = false;
        [SerializeField] float attackRange = 3f;
        [SerializeField] int maxDamage = 6;
        [SerializeField] float projectileSpeed = 0f;
        [Tooltip("number of attacks per melee round")]
        [SerializeField] int weaponSpeed = 0;


    // Getter Methods
        public int GetDamage(int strBonus)  { return CalculateDamage(strBonus); }
        public float GetRange()             { return attackRange; }
        public float GetSpeed()             { return weaponSpeed; }


    // Public Methods
        public void EquipWeapon(Transform[] hands, Animator animator)
        {
            if (!equippedPrefab || !weapAnimControl)
            {
                Debug.LogError("ORIO: Are you missing the Equipped Weapon Prefab or Animation Override Controller");
                return;
            }
            Transform weapLocation = GetWeapTransform(hands);
            GameObject weapon = Instantiate(equippedPrefab, weapLocation);
            weapon.transform.parent = weapLocation;
            weapon.name = Constant.weapName;
            animator.runtimeAnimatorController = weapAnimControl;
        }

        public void FireProjectile(GameObject instigator, Transform target, Transform[] hands, int strBonus)
        {
            Vector3 startLoc = GetWeapTransform(hands).position;
            Projectile projectileInst = Instantiate(projectile, startLoc, Quaternion.identity);
            projectileInst.transform.parent = GameObject.Find(Constant.tempGameObjName).transform;
            projectileInst.SetTarget(target.GetComponent<Health>());
            projectileInst.SetRange(attackRange);
            projectileInst.SetDamage(CalculateDamage(strBonus));
            projectileInst.SetSpeed(projectileSpeed);
            projectileInst.SetInstigater(instigator);
            // TODO decrease projectile count in inventory
        }

        public void DestoyWeapon(Transform[] hands)
        {
            Transform oldWeap = hands[0].Find(Constant.weapName);
            if (oldWeap == null) { oldWeap = hands[1].Find(Constant.weapName); }
            if (oldWeap == null)
            {
                Debug.LogWarning("No Weapon Found");
                return;
            }
            oldWeap.name = "OldWeapon";
            Destroy(oldWeap.gameObject);
        }


        // Private Methods
        private Transform GetWeapTransform(Transform[] hands)
        {
            Transform weapLocation;
            if (is2Handed) { weapLocation = hands[1]; }
            else { weapLocation = hands[0]; }
            return weapLocation;
        }

        private int CalculateDamage(int strBonus)
        {
            RNG generator = GameObject.Find(Constant.generatorObjName).GetComponent<RNG>();

            int damage = generator.GenerateNumber(maxDamage);
            int bonusDamage = strBonus;
            return damage + bonusDamage;
        }
    }
}