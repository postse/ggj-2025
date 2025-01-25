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

    private float pipeLength;
    public int pipeCount = 4;
    public int bubblesPerPipe = 8;
    bool connectorSpawned = false;
    int totalPipes = 0;

    void Start()
    {
        pipeLength = straightPipePrefab.transform.localScale.z;
        for (int i = 0; i <= pipeCount; i++)
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
            if (!connectorSpawned) {
                // 1 in 8 chance of spawning a connector
                if (Random.Range(0, 3) == 0)
                {
                    InstantiateConnector();
                }
                else
                {
                    InstantiateEnvironment(pipeCount);
                }
            } 
            else if (connectorSpawned && obj.CompareTag("Connector"))
            {
                // If a connector has been spawned, don't spawn anything else until it has been removed
                connectorSpawned = false;
                InstantiateEnvironment();
            }
        }
    }

    public void InstantiateConnector() {
        var selectedConnector = connectors[Random.Range(0, connectors.Count)];
        var connector = Instantiate(selectedConnector, pipeCount * new Vector3(0, 0, pipeLength), Quaternion.Euler(0, 90, 0), transform);
        environmentObjects.Add(connector);
        connectorSpawned = true;
        connector.name = "Connector" + totalPipes++;
    }

    public void InstantiateEnvironment(int offset = 0) {
        var straightPipe = Instantiate(straightPipePrefab, offset * new Vector3(0, 0, pipeLength), Quaternion.Euler(90, 0, 0), transform);
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