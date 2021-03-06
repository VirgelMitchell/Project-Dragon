using System.Collections.Generic;
using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.Magic
{
    public class CastSpell : MonoBehaviour
    {
        [SerializeField] ParticleSystem castingEffect = null;
        [SerializeField] ParticleSystem damageEffect = null;

        bool    isHoming = false;

        int     damage = 0;
        int     numberOfTarges = 0;

        float   areaOfEffct = 0f;
        float   distanceTraveled = 0;
        float   range = 0f;
        float   speed = 0f;

        AttackType  attackType;
        GameObject  instigater = null;
        Health      target = null;
        Transform   tempObject = null;


    // Basic Methods
        void Start()
        {
            Debug.Log("initializing spell: " + this.name);
            if (!castingEffect)
            {
                Debug.LogError("ORIO: No Casting Effect Loaded!");
                return;
            }
            if (target) { transform.LookAt(GetPosition()); }
        }

        void Update()
        {
            if (isHoming) { transform.LookAt(GetPosition()); }
            Vector3 pos1 = transform.position;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            Vector3 pos2 = transform.position;
            distanceTraveled += Vector3.Distance(pos2, pos1);
            if (distanceTraveled > range)
            {
                if(attackType != AttackType.area) {Destroy(gameObject); }
                else { DealAreaDamage(); }
            }
        }


    // Setter Methods
        public void InitializeTargeting(Health health, int targets, bool homing, AttackType attack, float area)
        {
            target = health;
            isHoming = homing;
            attackType = attack;
            areaOfEffct = area;
            numberOfTarges = targets;
        }

        public void InitializeSpell(int dmg, float rng, float spd, GameObject caster)
        {
            damage = dmg;
            range = rng;
            speed = spd;
            instigater = caster;
        }


    // Private Methods
        private Vector3 GetPosition()
        {
            // TODO Allow for targeting AoE spells without specifying an opponent
            CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
            Vector3 offset;
            if (collider) { offset = Vector3.up * collider.height * 0.66f; }
            else { offset = new Vector3(0f, 1f, 0f); }
            return target.transform.position + offset;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!target) { return; }
            if (other.gameObject.tag == "Player") { return; }

            Health candidate = other.GetComponent<Health>();
            if (candidate != target || candidate.GetIsDead()) { return; }
            InstantiateDamageEffect();
            castingEffect.Stop();
            if (attackType == AttackType.area) { DealAreaDamage(); }
            else { candidate.TakeDamage(instigater, damage); }
            Destroy(gameObject);
        }

        private void DealAreaDamage()
        {
            // TODO add method to choose a number of random targets up to the numberOfTargets
            foreach(Health target in GetTargets())
            {
                Vector3 pos1 = transform.position;
                Vector3 pos2 = target.transform.position;
                float damageModifier = 1 - (Vector3.Distance(pos1, pos2)/areaOfEffct);
                target.TakeDamage(instigater, (int)(damageModifier * damage));
            }
        }

        private List<Health> GetTargets()
        {
            List<Health> targets = new List<Health>();
            foreach (Health candidate in FindObjectsOfType<Health>())
            {
                float rangeToTarget = Vector3.Distance(candidate.transform.position, transform.position);
                if (rangeToTarget <= areaOfEffct)
                {
                    targets.Add(candidate);
                }
            }
            return targets;
        }

        private void InstantiateDamageEffect()
        {
            if (!damageEffect)
            {
                Debug.LogError("ORIO: No Damage Effect Loaded!");
                return;
            }
            ParticleSystem fx = Instantiate(damageEffect, transform.position,Quaternion.identity);
            fx.transform.parent = tempObject;
        }
    }
}