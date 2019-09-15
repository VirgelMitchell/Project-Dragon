using UnityEngine;
using RPG.Core;
using System.Collections.Generic;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
    // Variables
        [SerializeField] string uniqueID = "";
        
        static Dictionary<string, SavableEntity> gLookup = new Dictionary<string, SavableEntity>();


    // Basic Methods
#if UnityEditor
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) { return; }
            if (string.IsNullOrEmpty(gameObject.scene.path)) { return; }
            else
            {
                print("editing");
                SerializedObject thisObject = new SerializedObject(this);
                SerializedProperty property = thisObject.FindProperty("uniqueID");

                if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
                {
                    property.stringValue = System.Guid.NewGuid().ToString();
                    thisObject.ApplyModifiedProperties();
                }
                
                gLookup[property.stringValue] = this;
            }
        }
#endif


    // Getter Methods
        public string GetUID() { return uniqueID; }


    // Public Methods
        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISavable savable in GetComponents<ISavable>())
            {
                state[savable.GetType().ToString()] = savable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> restoredState = (Dictionary<string, object>)state;
            foreach (ISavable savable in GetComponents<ISavable>())
            {
                string typeString = savable.GetType().ToString();
                if (restoredState.ContainsKey(typeString))
                {
                    savable.RestoreState(restoredState[typeString]);
                }
            }
        }


    // Private Methods
        private bool IsUnique(string candidate)
        {
            if (!gLookup.ContainsKey(candidate)) { return true; }
            else
            {
                if (gLookup[candidate] == this) { return true; }

                // check to see if previous owner of uID has been destroyed
                if (gLookup[candidate] == null)
                {
                    gLookup.Remove(candidate);
                    return true;
                }

                // check to see if uID is outdated?
                if (gLookup[candidate].GetUID() != candidate)
                {
                    gLookup.Remove(candidate);
                    return true;
                }

                return false;
            }
        }
    }
}