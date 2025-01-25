using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    [SerializeField]
    private float lateralAcceleration = 5.0f;

    [SerializeField]
    private float lateralMaxSpeed = 10.0f;

    private Vector3 lateralMoveDirection;

    private Rigidbody rb;

    private GameplayController gameplayController;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        gameplayController = GetComponentInChildren<GameplayController>();
    }

    void FixedUpdate()
    {
        // use force to move player
        rb.AddForce(lateralMoveDirection * lateralAcceleration, ForceMode.Acceleration);

        // clamp the velocity to max speed
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, lateralMaxSpeed);
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        // Update moveDirection based on input
        lateralMoveDirection = new Vector3(input.x, input.y, 0); // Update x for lateral and y for vertical movement
    }

    void OnJump(InputValue value) 
    {
        ConnectorController cc = gameplayController.GetTurningConnector();
        if (cc) {
            // Turn connector
            if (Vector3.Dot(lateralMoveDirection, Vector3.right) > 0) {
                Debug.Log("Right");
                cc.StartTurning("Right");
            } else if (Vector3.Dot(lateralMoveDirection, Vector3.left) > 0) {
                Debug.Log("Left");
                cc.StartTurning("Left");
            } else if (Vector3.Dot(lateralMoveDirection, Vector3.up) > 0) {
                Debug.Log("Up");
                cc.StartTurning("Up");
            } else if (Vector3.Dot(lateralMoveDirection, Vector3.down) > 0) {
                Debug.Log("Down");
                cc.StartTurning("Down");
            }
        } else {
            // Other stuff
        }
    }

    public void StopMovement()
    {
        lateralMoveDirection = Vector3.zero;
        lateralAcceleration = 0;
        rb.useGravity = true;
    }
}