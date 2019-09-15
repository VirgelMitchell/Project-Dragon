using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasFader;
        Image canvasImage;

        void Awake()
        {
            canvasFader = GetComponent<CanvasGroup>();
            canvasImage = GetComponent<Image>();
        }

        public IEnumerator Fade2White(float fadeTime)
        {
            canvasImage.color = Color.white;
            while(canvasFader.alpha < 1f)
            {
                canvasFader.alpha += Time.deltaTime/fadeTime;
                yield return null;
            }
        }

        public IEnumerator Fade2Black(float fadeTime)
        {
            canvasImage.color = Color.black;
            while (canvasFader.alpha < 1f)
            {
                canvasFader.alpha += Time.deltaTime / fadeTime;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float fadeTime)
        {
            while(canvasFader.alpha > 0f)
            {
                canvasFader.alpha -= Time.deltaTime / fadeTime;
                yield return null;
            }
        }

        public void StartFadedOut()
        {
            canvasImage.color = Color.black;
            canvasFader.alpha = 1f;
        }
    }
}
