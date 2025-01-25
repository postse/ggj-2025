using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Rigidbody player;

    [SerializeField]
    private float cameraParallaxMultiplier = 2.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Rigidbody>();

        // I don't know why we have to do this
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x / cameraParallaxMultiplier, player.transform.position.y / cameraParallaxMultiplier, transform.position.z);
        transform.LookAt(player.transform.position / (cameraParallaxMultiplier * 2));
    }
}
