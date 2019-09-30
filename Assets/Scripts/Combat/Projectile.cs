using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        float distanceTraveled = 0f;
        int damage = 0;
        float range = 0f;
        float speed = 0f;

        Health target = null;
        GameObject instigater;

        // Basic Methods
        void Start()
        {
            if (target) { transform.LookAt(GetPosition()); }
        }

        void Update()
        {
            Vector3 pos1 = transform.position;
            transform.Translate(Vector3.forward*speed*Time.deltaTime);
            Vector3 pos2 = transform.position;
            distanceTraveled = distanceTraveled + Vector3.Distance(pos2, pos1);
            if (distanceTraveled > range) { Destroy(gameObject); }
        }


        // Public Methods
        public void SetDamage(int dmg)          { damage = dmg; }
        public void SetTarget(Health victim)    { target = victim; }
        public void SetRange(float rng)         { range = rng; }
        public void SetSpeed(float spd)         { speed = spd; }

        public void SetInstigater(GameObject shooter)
        {
            instigater = shooter;
        }


        // Private Methods
        private Vector3 GetPosition()
        {
            CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
            Vector3 offset = Vector3.up * collider.height * 0.66f;
            return target.transform.position + offset;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health candidate = other.GetComponent<Health>();
            if (candidate != target || candidate.GetIsDead()) { return; }
            target.TakeDamage(instigater, damage);
            Destroy(gameObject);
        }
    }
}