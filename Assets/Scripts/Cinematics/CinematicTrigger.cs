using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{

    public class CinematicTrigger : MonoBehaviour
    {
        bool hasBeenTriggered = false;

        void OnTriggerEnter(Collider other) 
        {
            if(!hasBeenTriggered && other.gameObject.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                hasBeenTriggered = true;
            }
        }
    }
}