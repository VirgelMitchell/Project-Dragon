using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        Health health;
        Text healthText;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthText = GetComponent<Text>();
        }

        private void Update()
        {
            int currentHP = health.GetHealth();
            int baseHP = health.GetBaseHP();
            float hPDecimal = (float)currentHP / baseHP;
            healthText.text = string.Format("Player: {0:p0}", hPDecimal);
        }
    }
}