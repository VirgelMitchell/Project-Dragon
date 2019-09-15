using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{

    public class CinematicTrigger : MonoBehaviour, ISavable
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

    // ISavable Implamentation
        public object   CaptureState()              { return hasBeenTriggered; }
        public void     RestoreState(object state)  { hasBeenTriggered = (bool)state; }
    }
}