using RPG.Combat;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
    // Variables
        Mover mover;
        Fighter fighter;
        private float timeSinceLastAttacked;


        // Basic Methods
        void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }
        
        void Update()
        {
            UpdateTimers();
            if (GetComponent<Health>().GetIsDead())     { return; }
            if (InteractWithCombat())                   { return; }
            if (InteractWithMovement())                 { return; }
        }


        // Public Methods
        public void ResetTimeSinceAttacked() { timeSinceLastAttacked = 0f; }
        public float GetTimeSinceLastAttacked() { return timeSinceLastAttacked; }


    // Private Methods
        private void UpdateTimers()
        {
            timeSinceLastAttacked += Time.deltaTime;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget candidate = hit.transform.GetComponent<CombatTarget>();

                if (candidate == null)                          {  continue;  }
                if (!fighter.CanAttack(candidate.gameObject))   {  continue;  }
                if (Input.GetMouseButton(0))
                {
                     mover.characterSpeed = "jog";
                     fighter.Attack(candidate.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetRay(), out hit);
            if (hasHit)
            {
                if (IsValidInput())
                {
                    int button = GetMouseButton();
                    if (button == 1)        { mover.characterSpeed = "run";    }
                    else if (button == 2)   { mover.characterSpeed = "walk";     }
                    else if (button == 3)   { mover.characterSpeed = "sneak";   }
                    else if (button == 4)   { mover.characterSpeed = "hustle";  }
                    else if (button == 5)   { mover.characterSpeed = "jog";     }
                    else                    { mover.characterSpeed = "idle";    }
                    mover.StartMoving(hit.point);
                }
                return true;
            }
            return false;
        }

        private int GetMouseButton()
        {
            if (Input.GetMouseButton(0))        { return 1; }
            else if (Input.GetMouseButton(1))   { return 2; }
            else if (Input.GetMouseButton(2))   { return 3; }
            else if (Input.GetMouseButton(3))   { return 4; }
            else if (Input.GetMouseButton(4))   { return 5; }
            else                                { return 0; }
        }

        private bool IsValidInput()
        {
            if (
                Input.GetMouseButton(0) ||
                Input.GetMouseButton(1) ||
                Input.GetMouseButton(2) ||
                Input.GetMouseButton(3) ||
                Input.GetMouseButton(4)
                )
                { return true; }
            else
                { return false; }
        }

        private static Ray GetRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}