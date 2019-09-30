using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
    // Variables
        [Header("Behavior")]
        [SerializeField] float chaseRadius = 5f;
        [SerializeField] float dwellTime = 2f;

        [Header("Duty")]
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] int currentWaypointIndex = 0;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceLastAttacked = Mathf.Infinity;
        float dwellingAtWaypoint = 0f;
        float paranoia;
        
        GameObject player;
        Fighter fighter;
        Mover mover;
        Vector3 guardPosition;

        const float waypointTollerance = 0.5f;


    //Basic Methods
        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            guardPosition = transform.position;
            paranoia = Random.Range(1f, 10f);
        }

        private void Start() { ResumeRoutine(); }

        private void Update()
        {
            if (GetComponent<Health>().GetIsDead()) { return; }
            timeSinceLastSawPlayer += Time.deltaTime;
            
            if (DistanceToTarget() < chaseRadius)
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < paranoia)
            {
                SuspicionBehavior();
            }
            else if (!HasDwealtEnough() && AtWaypoint())
            {
                DwellingBehaviour();
            }
            else
            {
                ResumeRoutine();
            }
        }


    // Public Methods
        public void ResetTimeSinceAttacked()    { timeSinceLastAttacked = 0; }
        public float GetTimeSinceLastAttacked() { return timeSinceLastAttacked; }


    // Private Methods
        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0f;
            mover.characterSpeed = "jog";
            fighter.Attack(player);
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelAction();
        }

        private void DwellingBehaviour()
        {
            dwellingAtWaypoint += Time.deltaTime;
        }

        private void ResumeRoutine()
        {
            Vector3 nextposition;

            if (!patrolPath) { nextposition = guardPosition; }
            else
            {
                if (AtWaypoint())
                {
                    dwellingAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextposition = GetCurrentWaypoint();
            }

            mover.characterSpeed = "amble";
            mover.StartMoving(nextposition);
        }

        private float DistanceToTarget()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTollerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool HasDwealtEnough()
        {
            return dwellingAtWaypoint >= dwellTime;
        }
        
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }
        
        
    // Game Engine Visualizations
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
