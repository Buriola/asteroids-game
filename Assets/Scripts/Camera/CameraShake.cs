using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Useful class to make a shake camera effect and reset the position afterwards.
/// </summary>
public class CameraShake : MonoBehaviour
{
    [Header("Shake Camera")]
    public static float shakeTimer; //Time to shake the camera
    public static float shakeAmount; //Force to be applied on the camera

    Vector3 pos;
    
    void Start()
    {
        pos = new Vector3(0f, 0f, -10f);
        ResetCameraPos();
    }

    void FixedUpdate()
    {
        //Shake the camera during some time
        if (shakeTimer >= 0)
        {
            Vector2 shakePos = Random.insideUnitCircle * shakeAmount; //Chooses a random position in a circle based on a force (shakeAmount)
            transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y, transform.position.z); //Moves the camera to the position
            shakeTimer -= Time.deltaTime; //Decreases the timer
        }
        else
        {
            ResetCameraPos();
        }
    }

    //Static function to shake the camera
    //@param shakePower - Force to be applied on the camera
    //@param shakeDuration - How much time you want to shake the camera
    public static void ShakeCamera(float shakePower, float shakeDuration)
    {
        shakeAmount = shakePower;
        shakeTimer = shakeDuration;
    }

    void ResetCameraPos()
    {
        transform.position = pos;
    }
}
