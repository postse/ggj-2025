using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public struct Collectible {
    public GameObject obj;
    public float weight;
}

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;

    private List<GameObject> environmentObjects = new List<GameObject>();
    public List<GameObject> connectors = new List<GameObject>();
    public List<Collectible> collectibles = new List<Collectible>();

    public GameObject straightPipePrefab;
    public float pipeLength;
    public float pipeRadius = 4.0f;
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

        // Normalize collectible weights
        float totalWeight = 0;
        foreach (var collectible in collectibles)
            totalWeight += collectible.weight;
        for (int i = 0; i < collectibles.Count; i++) {
            var tempCollectible = collectibles[i];
            tempCollectible.weight /= totalWeight;
            collectibles[i] = tempCollectible;
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
                if (UnityEngine.Random.Range(0, 2) == 0)
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
        var selectedConnector = connectors[UnityEngine.Random.Range(0, connectors.Count)];
        float randAngle = UnityEngine.Random.Range(0, 4) * 90.0f;
        var connector = Instantiate(selectedConnector, pipeCount * new Vector3(0, 0, pipeLength), Quaternion.Euler(0, 0, randAngle), transform);
        environmentObjects.Add(connector);
        activeConnector = connector;
        connector.name = "Connector" + totalPipes++;

        // Generate 'pipeCount' pipes in each of the 4 directions attached to the connector
        for (int i = 0; i < 4; i++) {
            for (int j = 1; j < pipeCount + 1; j++) {
                float rightSelector = Mathf.Cos(i * Mathf.PI / 2);
                float upSelector = Mathf.Sin(i * Mathf.PI / 2);
                float rightOffset = rightSelector * j * pipeLength;
                float upOffset = upSelector * j * pipeLength;
                var straightPipe = Instantiate(straightPipePrefab, new Vector3(rightOffset, upOffset, pipeLength * pipeCount), Quaternion.Euler(upSelector * 90, rightSelector * 90, 0), connector.transform);
                environmentObjects.Add(straightPipe);
                PlaceBubbleCollectibles(straightPipe);
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
        float collectibleSpacing = pipeLength / bubblesPerPipe;
        for (int i = -bubblesPerPipe / 2; i < bubblesPerPipe / 2; i++)
        {
            // Randomly generate items based on their weights
            float rand = UnityEngine.Random.Range(0.0f, 1.0f);
            GameObject collectible = null;
            float thresh = 0;
            for (int j = 0; j < collectibles.Count; j++)
            {
                thresh += collectibles[j].weight;
                if (rand < thresh)
                {
                    collectible = Instantiate(collectibles[j].obj, pipe.transform);
                    break;
                }
            }
            if (!collectible) break;
            collectible.transform.localPosition = new Vector3(UnityEngine.Random.Range(-pipeRadius, pipeRadius), UnityEngine.Random.Range(-pipeRadius, pipeRadius), collectibleSpacing * i);
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