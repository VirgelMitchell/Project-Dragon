using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] bool isSpell = false;
        [SerializeField] float respawnTime = 5f;
        [SerializeField] Weapon weapon = null;
        [SerializeField] Spell spell = null;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") { return; }
            Fighter fighter = other.GetComponent<Fighter>();
            if(fighter.GetIsArmed()) { fighter.DestroyWeapon(); }
            if (isSpell) { fighter.EquipSpell(spell); }
            else { fighter.EquipWeapon(weapon); }
            StartCoroutine(HideCollectable(respawnTime));
        }

        private IEnumerator HideCollectable(float respawnTime)
        {
            HideItem();
            yield return new WaitForSeconds(respawnTime);
            ShowItem();
        }

        private void HideItem()
        {
            GetComponent<Collider>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        private void ShowItem()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            GetComponent<Collider>().enabled = true;
        }
    }
}