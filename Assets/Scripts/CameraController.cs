using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Rigidbody player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Rigidbody>();

        // I don't know why we have to do this
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        transform.LookAt(player.transform.position / 5f);
    }
}
