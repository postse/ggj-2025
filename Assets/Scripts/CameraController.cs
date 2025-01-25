using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraDistance = 10.0f;
    private GameObject player;
    private Transform playerTransform;
    private Vector3 playerForwardMovementDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerTransform = player.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Debug.Log(playerTransform.position);
        // Update the forward movement direction from the player

        // Set the camera's position to be the player's position offset by the camera distance
        transform.position = playerTransform.position - playerForwardMovementDirection.normalized * cameraDistance;

        // Optionally, you can set the camera to look at the player
        transform.forward = playerForwardMovementDirection;
    }
}
