using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            GetComponent<Text>().text =
                string.Format("Player: {0:p0}", health.GetHealth());
        }
    }
}