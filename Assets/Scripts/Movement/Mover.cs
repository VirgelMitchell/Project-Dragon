using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] float baseSpeed = 1.524f;
        
        public string  characterSpeed = "idle";

        NavMeshAgent agent;
        SpeedModifier speedMod = new SpeedModifier();

        struct SpeedModifier
        {
            const float Amble = 0.75f;
            const float Hustle = 1.75f;
            const float Idle = 0f;
            const float Jog = 2.5f;
            const float Run = 3f;
            const float Sneak = 0.25f;
            const float Walk = 1f;
            
            public float GetSpeedModifier(string speed)
            {
                float speedMultiplier;

                if      (speed == "amble")      { speedMultiplier = Amble;  }
                else if (speed == "hustle")     { speedMultiplier = Hustle; }
                else if (speed == "jog")        { speedMultiplier = Jog;    }
                else if (speed == "run")        { speedMultiplier = Run;    }
                else if (speed == "sneak")      { speedMultiplier = Sneak;  }
                else if (speed == "walk")       { speedMultiplier = Walk;   }
                else                            { speedMultiplier = Idle;   }

                return speedMultiplier;
            }
        }

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            agent.enabled = !GetComponent<Health>().GetIsDead();
            UpdateAnimator();
        }

        public void StartMoving(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        
        public void CancelAction()    { agent.isStopped = true; }
        
        public void MoveTo(Vector3 destination)
        {
            float speedMult = speedMod.GetSpeedModifier(characterSpeed);

            agent.speed = baseSpeed * speedMult;
            agent.destination = destination;
            agent.isStopped = false;
        }
        
        private void UpdateAnimator(){
            Vector3 velocity = agent.velocity;
            float speed = transform.InverseTransformDirection(velocity).z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}