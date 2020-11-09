using UnityEngine;

namespace Buriola.CameraUtils
{
    public class CameraShake : MonoBehaviour
    {
        [Header("Shake Camera")] 
        public static float ShakeTimer;
        public static float ShakeAmount;

        private Vector3 _position;

        private void Start()
        {
            _position = new Vector3(0f, 0f, -10f);
            ResetCameraPos();
        }

        private void FixedUpdate()
        {
            if (ShakeTimer >= 0)
            {
                Vector2
                    shakePos = Random.insideUnitCircle *
                               ShakeAmount;
                transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y,
                    transform.position.z);
                ShakeTimer -= Time.deltaTime;
            }
            else
            {
                ResetCameraPos();
            }
        }
        
        public static void ShakeCamera(float shakePower, float shakeDuration)
        {
            ShakeAmount = shakePower;
            ShakeTimer = shakeDuration;
        }

        private void ResetCameraPos()
        {
            transform.position = _position;
        }
    }
}
