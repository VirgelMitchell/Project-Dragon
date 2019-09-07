using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    
    public class GateControl : MonoBehaviour
    {
        [SerializeField] Object targetScene;
        [SerializeField] Transform spawnPoint;
        
        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag != "Player") { return; }
            StartCoroutine(LoadNextScene());
        }

        private IEnumerator LoadNextScene()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(targetScene.name);

            GateControl other = GetTargetPortal();
            UpdatePlayer(other);
            
            Destroy(gameObject);
        }

        GateControl GetTargetPortal()
        {
            foreach (GateControl portal in FindObjectsOfType<GateControl>())
            {
                if (portal == this) { continue; }
                return portal;
            }
            return null;
        }

        void UpdatePlayer(GateControl other)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = other.spawnPoint.position;
            player.transform.rotation = other.spawnPoint.rotation;
        }
    }
}