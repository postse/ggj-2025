using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int damage = 1;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    AudioSource destructionSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        destructionSound = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        destructionSound.Play();
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
        CameraController.ShakeCamera(shakeDuration, shakeMagnitude);
        Destroy(gameObject, destructionSound.clip.length);
    }
}
