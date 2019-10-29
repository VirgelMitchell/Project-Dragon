using UnityEngine;

namespace RPG.Core
{
    public class RNG : MonoBehaviour
    {
        public int GenerateNumber(int die)
        {
            int value = 0;
            while (value < 1) { value = Mathf.RoundToInt(Random.value * die); }
            return value;
        }
    }
}
