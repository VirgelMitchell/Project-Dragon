using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIControler : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] int currentWaypointIndex = 0;
        [SerializeField] float dwellTime = 2f;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float dwellingAtWaypoint = 0f;
        float paranoia;
        
        GameObject player;
        Fighter fighter;
        Mover mover;
        Vector3 guardPosition;

        const float waypointTollerance = 0.5f;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            guardPosition = transform.position;
            paranoia = Random.Range(1f, 10f);
            ResumeRoutine();
        }

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
            Vector3 nextposition = GetCurrentWaypoint();
            mover.characterSpeed = "amble";

            if (patrolPath == null)
            {
                nextposition = guardPosition;
            }
            else if (AtWaypoint())
            {
                dwellingAtWaypoint = 0f;
                CycleWaypoint();
                nextposition = GetCurrentWaypoint();
            }

            GetComponent<Mover>().StartMoving(nextposition);
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
        
        Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        //Game Engine Visualizations
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
