using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        static bool hasSpawned = false;

        [Tooltip("Prefab")] [SerializeField] GameObject PersistantObjPrefab = null;

        private void Awake() {
            if (hasSpawned) { return; }
            else
            {
                GameObject persistantObjects = Instantiate(PersistantObjPrefab);
                DontDestroyOnLoad(persistantObjects);
                hasSpawned = true;
            }
        }
    }
}