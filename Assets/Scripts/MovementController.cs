using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float forwardMoveSpeed = 5.0f;

    [SerializeField]
    private float lateralMoveSpeed = 5.0f;

    private Vector3 moveDirection;
    private Rigidbody rb;

    void Start()
    {
        moveDirection = Vector3.forward;
        rb = GetComponentInChildren<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Always move forward
        Vector3 forwardMovement = Vector3.forward * forwardMoveSpeed;

        // Apply input-based movement
        Vector3 lateralMovement = moveDirection.x * transform.right * lateralMoveSpeed;
        Vector3 verticalMovement = moveDirection.y * transform.up * lateralMoveSpeed;

        // Combine movements
        Vector3 movement = forwardMovement + lateralMovement + verticalMovement;
        rb.AddForce(movement, ForceMode.Force);

        // Rotate the capsule to point in the moveDirection
        if (moveDirection != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(forwardMovement + lateralMovement + verticalMovement, Vector3.up);
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        // Update moveDirection based on input
        moveDirection = new Vector3(input.x, input.y, 0); // Update x for lateral and y for vertical movement
    }
}