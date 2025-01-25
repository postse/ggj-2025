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

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
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
}