using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;

    private GameObject[] environmentObjects;

    void Start()
    {
        // Cache the environment objects at the start
        environmentObjects = GameObject.FindGameObjectsWithTag("Environment");
    }

    void Update()
    {
        // Move each environment object along the global z-axis
        foreach (GameObject obj in environmentObjects)
        {
            obj.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}