using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointRadius = 0.3f;

        public Vector3 GetWaypoint(int child)
        {
            return transform.GetChild(child).position;
        }

        public int GetNextIndex(int child)
        {
            if (child + 1 == transform.childCount) { return 0; }
            return child + 1;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int child = 0; child < transform.childCount; child++)
            {
                int nextChild = GetNextIndex(child);
                Gizmos.DrawSphere(GetWaypoint(child), waypointRadius);
                Gizmos.DrawLine(GetWaypoint(child), GetWaypoint(nextChild));
            }
        }
    }
}