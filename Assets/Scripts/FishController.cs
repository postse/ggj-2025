using UnityEngine;

public class FishController : MonoBehaviour
{
    // Update is called once per frame
    public float moveSpeed;
    void Start() {
        moveSpeed = Random.Range(-10, 20);
        if (moveSpeed < 0) {
            transform.Rotate(0, 180, 0);
        }
    }
    void Update()
    {
        if (transform.parent.CompareTag("Environment"))
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.back, Space.World);
        }
    }
}
