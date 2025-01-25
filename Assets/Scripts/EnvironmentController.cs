using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;

    private List<GameObject> environmentObjects = new List<GameObject>();

    public GameObject straightPipePrefab;

    void Start()
    {
        InstantiateEnvironment();
    }

    void Update()
    {
        // Move each environment object along the global z-axis
        foreach (GameObject obj in environmentObjects)
        {
            obj.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void InstantiateEnvironment() {
        var straightPipe = Instantiate(straightPipePrefab, new Vector3(0, 0, 0), Quaternion.Euler(90, 0, 0), transform);
        environmentObjects.Add(straightPipe);
    }
}