using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Rigidbody player;

    [SerializeField]
    private float cameraParallaxMultiplier = 2.0f;
    private static float cameraShakeIntensity = 0.1f;

    private static bool isShaking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Rigidbody>();
    }

    void LateUpdate()
    {
        Vector3 cameraPosition = new Vector3(player.transform.position.x / cameraParallaxMultiplier, player.transform.position.y / cameraParallaxMultiplier, transform.position.z);

        if (isShaking)
        {
            cameraPosition.x += Random.Range(-1f * cameraShakeIntensity, cameraShakeIntensity);
            cameraPosition.y += Random.Range(-1f * cameraShakeIntensity, cameraShakeIntensity);
        }

        transform.position = cameraPosition;
        transform.LookAt(player.transform.position / (cameraParallaxMultiplier * 2));
    }

    public static void ShakeCamera(float seconds, float intensity)
    {
        cameraShakeIntensity = intensity;
        isShaking = true;
        CameraController instance = FindAnyObjectByType<CameraController>();

        if (instance != null)
        {
            instance.StartCoroutine(instance.StopShakeAfterDelay(seconds));
        }
    }

    private IEnumerator StopShakeAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isShaking = false;
    }
}
