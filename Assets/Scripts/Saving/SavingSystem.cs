using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
    // Public Methods
        public void Save(string fileName)
        {
            Dictionary<string, object> state = LoadFile(fileName);
            CaptureState(state);
            SaveFile(fileName, state);
        }

        public void Load(string fileName)
        {
            RestoreState(LoadFile(fileName));
        }

        public IEnumerator LoadLastScene(string fileName)
        {
            Dictionary<string, object> state = LoadFile(fileName);
            if (state.ContainsKey("LastScene"))
            {
                int scene = (int)state["LastScene"];
                if(scene != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(scene);
                }
                RestoreState(state);
            }
        }


    // Private Methods
        private void SaveFile(string fileName, Dictionary<string, object> state)
        {
            string path = GetPath(fileName);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private Dictionary<string, object> LoadFile(string fileName)
        {
            string path = GetPath(fileName);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SavableEntity entity in FindObjectsOfType<SavableEntity>())
            {
                state[entity.GetUID()] = entity.CaptureState();
            }
            state["LastScene"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SavableEntity entity in FindObjectsOfType<SavableEntity>())
            {
                string id = entity.GetUID();
                if (state.ContainsKey(id)) { entity.RestoreState(state[id]); }
            }
        }

        private string GetPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".pdg");
        }
    }
}