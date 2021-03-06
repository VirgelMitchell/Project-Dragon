using System.Collections;
using RPG.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class GateGuardianControl : MonoBehaviour
    {
    // Variables
        public string[] availableDestinations = { "Sandbox01", "Sandbox02" };

        float timeSinceTeleportation = Mathf.Infinity; //TODO: make persistant
        float interactionRadius = 1f;
        int gateActiveTime = 2;

        GameObject player;
        Portal[] gates;
        RNG generator;
        Scene destination;

    // Constants
        const float teleportationFrequency = 10f;
        const float day = hour * 24f;
        const float hour = minute * 60f;
        const float minute = 60f;


    // Basic Methods
        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            gates = FindObjectsOfType<Portal>();
        }

        private void Start()
        {
            generator = GameObject.Find(Constant.generatorObjectName).GetComponent<RNG>();
        }

        private void Update()
        {
            if (DistanceToTarget() <= interactionRadius) { InteractWithPlayer(); }
        }


    // Private Methods
        private float DistanceToTarget()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        private void InteractWithPlayer()
        {
            // possibly create variable to replace number of days
            float frequency = teleportationFrequency / (1 * day);

            if (timeSinceTeleportation < frequency) { print("Can't Teleport"); }

            // NPC welcomes the player and asks what region they want to travel to.
            // the player must choose from a list generated by the games build index

            SetTargets(SceneManager.GetSceneByName(availableDestinations[0]));
        }

        private void SetTargets(Scene sIndex)
        {
            foreach (Portal gate in gates)
            {
                foreach (Portal portal in FindObjectsOfType<Portal>())
                {
                    if (portal == gate) { continue; }
                    if (portal.GetPortalIndex() == gate.GetPortalIndex())
                    {
                        gate.target = portal.GetPortalSpawn();
                    }
                }
            }
        }

        private IEnumerator ActivateGates()
        {
            timeSinceTeleportation = 0f;
            foreach (Portal gate in gates) { gate.SetActive(); }
            yield return new WaitForSeconds(gateActiveTime * minute);
            foreach (Portal gate in gates) { gate.SetActive(); }
        }
    }
}
