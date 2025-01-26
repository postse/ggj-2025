using System.Collections;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int damage = 1;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    [Tooltip("Leave 0 for default audio duration")]
    public float audioDurationOverride = 0.0f;
    AudioSource destructionSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CameraController cameraController;

    void Start()
    {
        destructionSound = GetComponent<AudioSource>();
        cameraController = FindFirstObjectByType<CameraController>();
    }

    public void Interact()
    {
        Debug.Log(gameObject);
        StartCoroutine(PlayAudioForDuration(audioDurationOverride));
        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in meshRenderers)
        {
            renderer.enabled = false;
        }
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        cameraController.ShakeCamera(shakeDuration, shakeMagnitude);
        Destroy(gameObject, destructionSound.clip.length);
    }

    private IEnumerator PlayAudioForDuration(float duration)
    {
        if (duration == 0.0f)
        {
            duration = destructionSound.clip.length;
        }

        destructionSound.Play();
        yield return new WaitForSeconds(duration);
        destructionSound.Stop();
    }
}
