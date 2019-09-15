using System.Collections;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement

{
    public class Portal : MonoBehaviour
    {
    // Variables
        [Header("General")]
        [SerializeField] bool isGate = false;
        [SerializeField] float fadeTime = 1.5f;
        [SerializeField] Transform portalSpawn = null;

        [Header("Targeting")]
        [SerializeField] int portalIndex = 0;
        [SerializeField] int sceneToLoad = -1;
        [Tooltip("Leave Blank")]public Transform target = null;

        bool isActive = true;


    // Getter Methods
        public int GetPortalIndex()         { return portalIndex; }
        public Transform GetPortalSpawn()   { return portalSpawn; }
        public void SetActive()             { isActive = !isActive; }


    // Public Mehtods
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


    // Private Methods
        private void OnTriggerEnter(Collider collider)
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
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            
            DontDestroyOnLoad(gameObject);

            yield return fader.Fade2White(fadeTime);
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            savingWrapper.Load();
            if (!target) { target = SetTarget(); }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Mover>().UpdatePlayer(target);
            
            savingWrapper.Save();
            yield return new WaitForSeconds(fadeTime);
            yield return fader.FadeIn(fadeTime);

            Destroy(gameObject);
        }
    }
}