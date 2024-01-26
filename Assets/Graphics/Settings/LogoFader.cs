using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInObject : MonoBehaviour
{
    public float fadeInDuration = 2f; 
    private Material material; 

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }

        if (material != null)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogError("Womp");
        }
    }

    IEnumerator FadeIn()
    {
        Color startColor = material.color;
        startColor.a = 0f;
        material.color = startColor;

        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);

            material.color = new Color(material.color.r, material.color.g, material.color.b, newAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Color endColor = material.color;
        endColor.a = 1f;
        material.color = endColor;
    }
}
