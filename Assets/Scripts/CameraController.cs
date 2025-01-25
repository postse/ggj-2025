using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        // I don't know why we have to do this
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
