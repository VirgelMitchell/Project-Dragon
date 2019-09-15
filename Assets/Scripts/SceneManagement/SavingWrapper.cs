using System.Collections;
using RPG.Core;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultFile = "Save";

        float waitTime = 1f;
        float fadeSpeed = 1f;
        SavingSystem sSystem;

        private void Awake()
        {
            sSystem = GetComponent<SavingSystem>(); 
        }

        private IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();

            fader.StartFadedOut();

            yield return StartCoroutine(sSystem.LoadLastScene(defaultFile));
            yield return new WaitForSeconds(waitTime);

            yield return fader.FadeIn(fadeSpeed);
        }
        
        private void Update()
         {
            if (Input.GetKeyDown(KeyCode.F1)) { Save(); }
            if (Input.GetKeyDown(KeyCode.F2)) { Load(); }
        }

        public void Save()
        {
            sSystem.Save(defaultFile);
        }

        public void Load()
        {
            sSystem.Load(defaultFile);
        }
    }
}