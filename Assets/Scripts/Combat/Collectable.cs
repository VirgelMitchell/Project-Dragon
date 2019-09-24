using UnityEngine;

namespace RPG.Combat
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] bool isSpell = false;
        [SerializeField] Weapon weapon = null;
        [SerializeField] Spell spell = null;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") { return; }
            Fighter fighter = other.GetComponent<Fighter>();
            if(fighter.GetIsArmed()) { fighter.DestroyWeapon(); }
            if (isSpell) { fighter.EquipSpell(spell); }
            else { fighter.EquipWeapon(weapon); }
            Destroy(gameObject);
        }
    }
}