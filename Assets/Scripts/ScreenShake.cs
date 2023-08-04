using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    private Vector3 initialPosition;
    public float dampingSpeed = 1.0f;
    public float shakeDuration = 0.0f;
    public float shakeMagnitude = 0.0f;

    private void Awake()
    {
        Instance = this;
        initialPosition = transform.position; //Set the camera's initial position
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            //Shake the camera in random directions for the defined duration
            Vector3 shakeDir = Random.insideUnitCircle;
            transform.localPosition = initialPosition + shakeDir * shakeMagnitude* shakeDuration;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            //Reset the camera to its initial position
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    private void OnShake(float duration, float strength)
    {
        shakeDuration = duration;
        shakeMagnitude = strength;
    }

    public static void Shake(float duration, float strength) => Instance.OnShake(duration, strength);
}
