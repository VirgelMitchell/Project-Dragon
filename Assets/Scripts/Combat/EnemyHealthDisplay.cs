using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour 
    {
        Fighter player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Text healthText = GetComponent<Text>();
            if (player.GetTarget() == null)
            {
                healthText.text = "N/A";
            }
            else
            {
                Health health = player.GetTarget().GetComponent<Health>();
                healthText.text =
                    string.Format("Enemy: {0:p0}", health.GetHP());
            }
        }
    }
}