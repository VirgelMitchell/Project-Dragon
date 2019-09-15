using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") { return; }
            Fighter fighter = other.GetComponent<Fighter>();
            if (fighter.GetIsArmed()) { fighter.DestroyWeapon(); }
            fighter.EquipWeapon(weapon);
            Destroy(gameObject);
        }
    }
}