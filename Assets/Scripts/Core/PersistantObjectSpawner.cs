using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        static bool hasSpawned = false;

        [Tooltip("Prefab")] [SerializeField] GameObject faderPrefab = null;
        [Tooltip("Prefab")] [SerializeField] GameObject saveSysPrefab = null;

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
            GameObject fader = Instantiate(faderPrefab);
            DontDestroyOnLoad(fader);
            GameObject saveSystem = Instantiate(saveSysPrefab);
            DontDestroyOnLoad(saveSystem);
        }
    }
}