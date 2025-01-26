using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;

    private List<GameObject> environmentObjects = new List<GameObject>();
    public List<GameObject> connectors = new List<GameObject>();
    public List<GameObject> collectibles = new List<GameObject>();

    public GameObject straightPipePrefab;
    public float pipeLength;
    public int pipeCount = 4;
    public int bubblesPerPipe = 8;
    // bool connectorActive = false;
    private GameObject activeConnector;
    int totalPipes = 0;

    void Start()
    {
        pipeLength = straightPipePrefab.transform.GetChild(0).localScale.z;
        for (int i = 0; i <= pipeCount; i++)
        {
            InstantiateEnvironment(i);
        }
    }

    void Update()
    {
        List<GameObject> objectsToRemove = new List<GameObject>();
        // Move each environment object along the global z-axis
        bool shouldISpawn = true;
        foreach (GameObject obj in environmentObjects)
        {
            if (!obj) {
                objectsToRemove.Add(obj);
                continue;
            }
            // Don't move children of Connectors
            if (obj.transform.parent == null || !obj.transform.parent.CompareTag("Connector"))
                obj.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
            if (obj.transform.position.z < -pipeLength)
                objectsToRemove.Add(obj);
        }

        // Remove objects that have moved off the screen
        for(int i = 0; i < objectsToRemove.Count; i++)
        {
            var obj = objectsToRemove[i];
            if (!obj) {
                environmentObjects.Remove(obj);
                continue;
            }
            if (!activeConnector && shouldISpawn) {
                if (Random.Range(0, 2) == 0)
                    InstantiateConnector();
                else 
                    InstantiateEnvironment(pipeCount);
                shouldISpawn = false;
            } else if (obj == activeConnector)
            {
                activeConnector = null;
            }
            
            if (obj.CompareTag("Connector")) continue;

            environmentObjects.Remove(obj);
            Destroy(obj);
        }
    }

    public void InstantiateConnector() {
        var selectedConnector = connectors[Random.Range(0, connectors.Count)];
        float randAngle = Random.Range(0, 4) * 90.0f;
        var connector = Instantiate(selectedConnector, pipeCount * new Vector3(0, 0, pipeLength), Quaternion.Euler(0, 0, randAngle), transform);
        environmentObjects.Add(connector);
        activeConnector = connector;
        connector.name = "Connector" + totalPipes++;

        // Generate 'pipeCount' pipes in each of the 4 directions attached to the connector
        for (int i = 0; i < 4; i++) {
            for (int j = 1; j < pipeCount + 1; j++) {
                // int j = 1;
                float rightSelector = Mathf.Cos(i * Mathf.PI / 2);
                float upSelector = Mathf.Sin(i * Mathf.PI / 2);
                float rightOffset = rightSelector * j * pipeLength;
                float upOffset = upSelector * j * pipeLength;
                var straightPipe = Instantiate(straightPipePrefab, new Vector3(rightOffset, upOffset, pipeLength * pipeCount), Quaternion.Euler(upSelector * 90, rightSelector * 90, 0), connector.transform);
                environmentObjects.Add(straightPipe);
            }
        }
    }

    public void InstantiateEnvironment(int offset = 0) {
        var straightPipe = Instantiate(straightPipePrefab, offset * new Vector3(0, 0, pipeLength), Quaternion.Euler(0, 0, 0), transform);
        environmentObjects.Add(straightPipe);

        PlaceBubbleCollectibles(straightPipe);
        straightPipe.name = "Pipe" + totalPipes++;
    }

    public void PlaceBubbleCollectibles(GameObject pipe) {
        float collectibleSpacing = 1f / bubblesPerPipe;
        for (int i = -bubblesPerPipe / 2; i < bubblesPerPipe / 2; i++)
        {
            var collectible = Instantiate(collectibles[Random.Range(0, collectibles.Count)], pipe.transform);
            collectible.transform.localPosition = new Vector3(Random.Range(-.1f, 0.1f), collectibleSpacing * i, Random.Range(-.1f, 0.1f));
        }
    }

    public void StopMovement(float secondsToStop)
    {
        StartCoroutine(GraduallyStopMovement(secondsToStop));
    }

    private IEnumerator GraduallyStopMovement(float secondsToStop)
    {
        float initialSpeed = moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < secondsToStop)
        {
            moveSpeed = Mathf.Lerp(initialSpeed, 0, elapsedTime / secondsToStop);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moveSpeed = 0;
    }
}