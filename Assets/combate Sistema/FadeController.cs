using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        Color c = fadeImage.color;
        c.a = 1;
        fadeImage.color = c;

        while (c.a > 0)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = c;
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        Color c = fadeImage.color;
        c.a = 0;
        fadeImage.color = c;

        while (c.a < 1)
        {
            c.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = c;
            yield return null;
        }
    }
}

