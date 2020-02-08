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
                healthText.text = string.Format("Enemy: {0:p0}", target.GetComponent<Health>().GetHealth());
            }
        }
    }
}