using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target = null;

        void LateUpdate()
        {
            if (SceneManager.GetActiveScene().name == SceneNames.camp) { return; }
            transform.position = target.position;
        }
    }
}