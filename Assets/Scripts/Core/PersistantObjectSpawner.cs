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
                SpawnObjects();
                hasSpawned = true;
            }
        }

        private void SpawnObjects()
        {
            GameObject persistantObjects = Instantiate(PersistantObjPrefab);
            DontDestroyOnLoad(persistantObjects);
        }
    }
}