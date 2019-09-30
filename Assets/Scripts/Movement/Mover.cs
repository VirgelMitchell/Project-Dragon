using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISavable
    {
    //Variables
        [SerializeField] float baseSpeed = 1.524f;
        
        public string  characterSpeed = "idle";

        NavMeshAgent agent;
        SpeedModifier speedMod = new SpeedModifier();


    // Basic Methods
        void Awake() { agent = GetComponent<NavMeshAgent>(); }

        void Update()
        {
            agent.enabled = !GetComponent<Health>().GetIsDead();
            UpdateAnimator();
        }


    // Public Methods
        public void StartMoving(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        
        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
            agent.speed = baseSpeed * speedMod.GetSpeedModifier(characterSpeed);
            agent.isStopped = false;
        }

        public void UpdatePlayer(Transform spawnPoint)
        {
            agent.enabled = false;
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            agent.enabled = true;
        }


    // Private Methods
        private void UpdateAnimator(){
            Vector3 velocity = agent.velocity;
            float speed = transform.InverseTransformDirection(velocity).z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }


    // IAction Implamentation
        public void CancelAction() { agent.isStopped = true; }


    // ISavable Implamentation
        public object CaptureState()
        {
            return new SVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SVector3 restoredPosition = (SVector3)state;
            agent.enabled = false;
            transform.position = restoredPosition.ToVector3();
            agent.enabled = true;
            GetComponent<ActionScheduler>().CancelAction();
        }
    }
}