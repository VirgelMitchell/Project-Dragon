// private void Start()                                                //AIController Line 35
// {
//     player = GameObject.FindWithTag("Player");
//     fighter = GetComponent<Fighter>();
//     mover = GetComponent<Mover>();
//     guardPosition = transform.position;                             //AIController Line 40
//     paranoia = Random.Range(1, 10);
//     ResumeRoutine();    
// }

// private void ResumeRoutine()                                        //AIController Line 84
// {
//     Vector3 nextposition;
//     if (patrolPath == null) { nextposition = guardPosition; }
//     else { nextposition = GetNextWaypoint(); }
//     mover.characterSpeed = "amble";
//                                                                     //AIController Line 90
//     if (patrolPath != null)
//     {
//         if (AtWaypoint())
//         {
//             dwellingAtWaypoint = 0f;
//             CycleWaypoint();
//         }
//         nextposition = GetNextWaypoint();
//     }
//                                                                     //AIController Line 100
//     GetComponent<Mover>().StartMoving(nextposition);
// }

// void Start()                                                        //Mover Line 42
// {
//     agent = GetComponent<NavMeshAgent>();
// }

// public void StartMoving(Vector3 destination)                        //Mover Line 53
// {
//     GetComponent<ActionScheduler>().StartAction(this);
//     MoveTo(destination);
// }

// public void MoveTo(Vector3 destination)                             //Mover Line 61
// {
//     float speedMult = speedMod.GetSpeedModifier(characterSpeed);

//     agent.speed = baseSpeed * speedMult;
//     agent.destination = destination;
//     agent.isStopped = false;    // NullReference Error thrown on this Line (Object not set to an
//                                 // instance of an Object)
// }
