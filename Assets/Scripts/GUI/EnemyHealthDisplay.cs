using RPG.Combat;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.GUI
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        GameObject player;
        Text healthText;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            healthText = GetComponent<Text>();
        }

        private void Update()
        {
            Transform target = player.GetComponent<Fighter>().GetTarget();
            if (target == null)
            {
                healthText.text = "N/A";
            }
            else
            {
                Health health = target.GetComponent<Health>();
                int currentHP = health.GetHealth();
                int baseHP = health.GetBaseHP();
                float hPDecimal = (float)currentHP / baseHP;
                healthText.text = string.Format("Enemy: {0:p0}", hPDecimal);
            }
        }
    }
}