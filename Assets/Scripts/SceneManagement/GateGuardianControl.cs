using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    
public class GateGuardianControl : MonoBehaviour
{
        public string[] availableDestinations = {"Sandbox01", "Sandbox02"};

        float timeSinceTeleportation = Mathf.Infinity; //TODO: make persistant
        float interactionRadius = 1f;
        int gateActiveTime = 2;

        GameObject player;
        Portal[] gates;

        Scene destination;

        const float teleportationFrequency = 10f;
        const float day = hour * 24f;
        const float hour = minute * 60f;
        const float minute = 60f;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            gates = FindObjectsOfType<Portal>();
        }

        private void Update()
        {
            if (DistanceToTarget() <= interactionRadius) { InteractWithPlayer(); }
        }

        private float DistanceToTarget()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        void InteractWithPlayer()
        {
            // possibly create variable to replace number of days
            float frequency = teleportationFrequency / (1 * day); 

            if (timeSinceTeleportation < frequency) { print("Can't Teleport"); }
            
            // NPC welcomes the player and asks what region they want to travel to.
            // the player must choose from a list generated by the games build index

            SetTargets(SceneManager.GetSceneByName(availableDestinations[0]));
        }

        void SetTargets(Scene sIndex)
        {
            foreach(Portal gate in gates)
            {
                foreach (Portal portal in FindObjectsOfType<Portal>())
                {
                    if (portal == gate) { continue; }
                    if(portal.GetPortalIndex() == gate.GetPortalIndex())
                    {
                        gate.target = portal.GetPortalSpawn();
                    }
                }
            }
        }

        IEnumerator ActivateGates()
        {
            timeSinceTeleportation = 0f;
            foreach (Portal gate in gates) { gate.SetActive(); }
            yield return new WaitForSeconds(gateActiveTime * minute);
            foreach (Portal gate in gates) { gate.SetActive(); }
        }
    }
}