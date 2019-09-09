using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class ControlRemover : MonoBehaviour
    {
        GameObject player;
        
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }
        
        void DisableControl(PlayableDirector pd)
        {
            Debug.Log("Disabling Control");
            player.GetComponent<ActionScheduler>().CancelAction();
            player.GetComponent<PlayerControler>().enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            Debug.Log("Re-enabling Control");
            player.GetComponent<PlayerControler>().enabled = true;
        }
    }    
}