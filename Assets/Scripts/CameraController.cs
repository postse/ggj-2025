using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    private Rigidbody player;

    [SerializeField]
    private float cameraParallaxMultiplier = 2.0f;
    private float cameraShakeIntensity = 0.1f;

    private bool isShaking = false;

    public float defaultCameraDistance = 5.0f;
    private float cameraDistance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Rigidbody>();
        cameraDistance = defaultCameraDistance;

        SetRenderResolution(640);
    }

    void LateUpdate()
    {
        Vector3 cameraPosition = new Vector3(player.transform.position.x / cameraParallaxMultiplier, player.transform.position.y / cameraParallaxMultiplier, -cameraDistance);

        if (isShaking)
        {
            cameraPosition.x += Random.Range(-1f * cameraShakeIntensity, cameraShakeIntensity);
            cameraPosition.y += Random.Range(-1f * cameraShakeIntensity, cameraShakeIntensity);
        }

        transform.position = cameraPosition;
        transform.LookAt(player.transform.position / (cameraParallaxMultiplier * 2));
    }

    public void ShakeCamera(float seconds, float intensity)
    {
        cameraShakeIntensity = intensity;
        isShaking = true;
        StartCoroutine(StopShakeAfterDelay(seconds));
    }

    private IEnumerator StopShakeAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isShaking = false;
    }

    private void SetRenderResolution(int horizontalPixels)
    {
        var urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.defaultRenderPipeline;
        float aspectRatio = (float)Screen.height / Screen.width;
        float renderScale = horizontalPixels / (float)Screen.width;
        urpAsset.renderScale = aspectRatio * renderScale;
    }
}
