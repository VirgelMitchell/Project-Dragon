using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement

{
    public class Portal : MonoBehaviour
    {
        [SerializeField] bool isGate = false;
        [SerializeField] int portalIndex = 0;
        [SerializeField] Transform portalSpawn = null;
        [SerializeField] int sceneToLoad = -1;

        public Transform target = null;

        bool isActive = true;

        public int GetPortalIndex()         { return portalIndex; }
        public Transform GetPortalSpawn()   { return portalSpawn; }
        public void SetActive()             { isActive = !isActive; }

        void OnTriggerEnter(Collider collider)
        {
            if(!isGate || isGate && isActive)
            {
                if (collider.tag != "Player") { return; }
                if (isGate && target == null) { return; }
                StartCoroutine(LoadNextScene());
            }
        }

        private IEnumerator LoadNextScene()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            target = SetTarget();
            UpdatePlayer(target);

            Destroy(gameObject);
        }

        public Transform SetTarget()
        {
            Portal[] portals = FindObjectsOfType<Portal>();
            foreach (Portal portal in portals)
            {
                if (portal == this) { continue; }

                if (portal.portalIndex == portalIndex)
                {
                    return portal.portalSpawn;
                }
            }
            return null;
        }

        void UpdatePlayer(Transform spawnPoint)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
        }
    }
}