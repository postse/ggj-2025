using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;

    private List<GameObject> environmentObjects = new List<GameObject>();

    public GameObject straightPipePrefab;

    readonly private float pipeLength = 85f;
    public int pipeCount = 3;
    bool connectorSpawned = false;

    void Start()
    {
        for (int i = 0; i < pipeCount; i++)
        {
            InstantiateEnvironment(i);
        }
    }

    void Update()
    {
        List<GameObject> objectsToRemove = new List<GameObject>();
        // Move each environment object along the global z-axis
        foreach (GameObject obj in environmentObjects)
        {
            obj.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
            if (obj.transform.position.z < -pipeLength)
            {
                objectsToRemove.Add(obj);
            }
        }

        // Remove objects that have moved off the screen
        for(int i = 0; i < objectsToRemove.Count; i++)
        {
            var obj = objectsToRemove[i];
            environmentObjects.Remove(obj);
            Destroy(obj);
            InstantiateEnvironment(pipeCount - 1);
        }
    }

    public void InstantiateEnvironment(int offset = 0) {
        var straightPipe = Instantiate(straightPipePrefab, offset * new Vector3(0, 0, pipeLength), Quaternion.Euler(90, 0, 0), transform);
        environmentObjects.Add(straightPipe);
    }
}