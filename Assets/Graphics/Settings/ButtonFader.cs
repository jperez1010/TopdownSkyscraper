using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeIn : MonoBehaviour
{
    [System.Serializable]
    public class UIElementInfo
    {
        public Graphic element;
        public float fadeInDuration;
    }

    public float delayBeforeStart = 1f; 
    public UIElementInfo[] elementsToFade;

    void Start()
    {
        SetElementsAlpha(0f);

        StartCoroutine(FadeInAllElements());
    }

    IEnumerator FadeInAllElements()
    {
        foreach (UIElementInfo elementInfo in elementsToFade)
        {
            yield return new WaitForSeconds(delayBeforeStart); 

            if (elementInfo.element != null)
            {
                StartCoroutine(FadeInElement(elementInfo.element, elementInfo.fadeInDuration));
            }
        }
    }

    IEnumerator FadeInElement(Graphic element, float fadeInDuration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);

            SetElementAlpha(element, newAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetElementAlpha(element, 1f);
    }

    void SetElementAlpha(Graphic element, float alpha)
    {
        if (element != null)
        {
            Color currentColor = element.color;
            currentColor.a = alpha;
            element.color = currentColor;
        }
    }

    void SetElementsAlpha(float alpha)
    {
        foreach (UIElementInfo elementInfo in elementsToFade)
        {
            if (elementInfo.element != null)
            {
                SetElementAlpha(elementInfo.element, alpha);
            }
        }
    }
}
