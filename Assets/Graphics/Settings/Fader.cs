using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenFader : MonoBehaviour
{
    public Image fadeImagePrefab; 
    public RectTransform uiPanel; 
    public Graphic[] elementsToHide; 
    public float fadeInSpeed = 2f;
    public float fadeOutSpeed = 2f;

    private bool isFadingOut = false;
    private Image currentFadeImage;

    void Start()
    {
        currentFadeImage = InstantiateFadeImage();
        currentFadeImage.color = new Color(0f, 0f, 0f, 1f);

        RectTransform rectTransform = currentFadeImage.rectTransform;
        rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        StartCoroutine(FadeIn(currentFadeImage, fadeInSpeed));
    }

    public void StartFadeIn()
    {
        currentFadeImage = InstantiateFadeImage();
        StartCoroutine(FadeIn(currentFadeImage, fadeInSpeed));
    }

    public void StartFadeOut()
    {
        if (!isFadingOut)
        {
            currentFadeImage = InstantiateFadeImage();
            StartCoroutine(FadeOut(currentFadeImage, fadeOutSpeed));
        }
    }

    Image InstantiateFadeImage()
    {
        if (currentFadeImage != null)
        {
            Destroy(currentFadeImage.gameObject);
        }

        Image fadeImage = Instantiate(fadeImagePrefab, uiPanel);

        return fadeImage;
    }

    IEnumerator FadeIn(Image fadeImage, float duration)
    {
        float elapsedTime = 0f;
        float startAlpha = fadeImage.color.a;
        float targetAlpha = 0f;

        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            fadeImage.color = new Color(0f, 0f, 0f, newAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(0f, 0f, 0f, 0f);

        Destroy(fadeImage.gameObject);
    }

    IEnumerator FadeOut(Image fadeImage, float duration)
    {
        isFadingOut = true;

        float elapsedTime = 0f;
        float startAlpha = fadeImage.color.a;
        float targetAlpha = 1f;

        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            fadeImage.color = new Color(0f, 0f, 0f, newAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }


        SetElementsVisibility(false);

        Destroy(fadeImage.gameObject);
        isFadingOut = false;
    }

    void SetElementsVisibility(bool isVisible)
    {
        foreach (Graphic element in elementsToHide)
        {
            element.gameObject.SetActive(isVisible);
        }
    }
}
