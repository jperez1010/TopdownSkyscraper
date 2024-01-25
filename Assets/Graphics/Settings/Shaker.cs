using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraShake : MonoBehaviour
{
    public Transform cameraTransform;
    public float shakeDuration = 1f;
    public float shakeMagnitude = 0.5f;

    private Vector3 originalPosition;
    private float elapsedShakeTime = 0f;
    private bool isShaking = false;

    void Start()
    {
        originalPosition = cameraTransform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            if (elapsedShakeTime < shakeDuration)
            {
                ShakeCamera();
            }
            else
            {
                StopShaking();
            }
        }
    }

    void ShakeCamera()
    {
        elapsedShakeTime += Time.deltaTime;

        float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
        float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

        float perlinX = Mathf.PerlinNoise(Time.time * 10f, 0f) - 0.5f;
        float perlinY = Mathf.PerlinNoise(0f, Time.time * 10f) - 0.5f;

        Vector3 shakeOffset = new Vector3(offsetX + perlinX, offsetY + perlinY, 0f) * shakeMagnitude;
        cameraTransform.localPosition = originalPosition + shakeOffset;
    }

    void StopShaking()
    {
        isShaking = false;
        cameraTransform.localPosition = originalPosition;
    }

    public void StartCinematicShake()
    {
        isShaking = true;
        elapsedShakeTime = 0f;
    }
}