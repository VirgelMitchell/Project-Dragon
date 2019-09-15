using RPG.Core;
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

        enum Hand { left, right };
        
    // Constants
        const string weapName = "Weapon";


    // Getter Methods
        public int GetDamage(int strBonus)  { return CalculateDamage(strBonus); }
        public float GetRange()             { return attackRange; }
        public float GetSpeed()             { return weaponSpeed; }


    // Public Methods
        public void EquipWeapon(Transform[] hands, Animator animator)
        {
            if (!equippedPrefab || !weapAnimControl) { return; }
            Transform weapLocation = GetWeapTransform(hands);
            GameObject weapon = Instantiate(equippedPrefab, weapLocation);
            weapon.name = weapName;
            animator.runtimeAnimatorController = weapAnimControl;
        }

        public void FireProjectile(Transform[] hands, Transform target, int strBonus)
        {
            Vector3 startLoc = GetWeapTransform(hands).position;
            Projectile projectileInst = Instantiate(projectile, startLoc, Quaternion.identity);
            projectileInst.transform.parent = GameObject.Find("TempObjetcts").transform;
            projectileInst.SetTarget(target.GetComponent<Health>());
            projectileInst.SetRange(attackRange);
            projectileInst.SetDamage(CalculateDamage(strBonus));
            projectileInst.SetSpeed(projectileSpeed);
            // TODO decrease projectile count in inventory
        }

        public void DestoyWeapon(Transform[] hands)
        {
            GameObject oldWeap;
            foreach (Transform hand in hands)
            {
                if(GameObject.Find(weapName))
                {
                    oldWeap = GameObject.Find(weapName);
                    if(oldWeap)
                    {
                        oldWeap.name = "OldWeapon";
                        Destroy(oldWeap);
                    }
                }
            }
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
            int damage = (int)Random.Range(1f, maxDamage + 1f);
            int bonusDamage = strBonus;
            return damage + bonusDamage;
        }
    }
}