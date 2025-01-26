using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Collectible {
    public GameObject obj;
    public float weight;
}

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;
    public float defaultMoveSpeed;

    public float superBubbleSpeedMultiplier = 1.0f;

    private List<GameObject> environmentObjects = new List<GameObject>();
    public List<GameObject> connectors = new List<GameObject>();
    public List<Collectible> collectibles = new List<Collectible>();

    public GameObject straightPipePrefab;
    public float pipeLength;
    public float pipeRadius = 4.0f;
    public int pipeCount = 4;
    public int collectiblesPerPipe = 8;
    // bool connectorActive = false;
    [SerializeField]
    private bool connectorActive = false;
    int totalPipes = 0;

    void Start()
    {
        pipeLength = straightPipePrefab.transform.GetChild(0).localScale.z;
        for (int i = 0; i <= pipeCount; i++)
        {
            InstantiateEnvironment(i);
        }
        defaultMoveSpeed = moveSpeed;
        ChangeSpeeds(new float[] { moveSpeed }, 3f);
    }

    void Update()
    {
        MoveObjects();
        if (RemoveObjects())
        {
            SpawnObjects();
        }
    }

    void MoveObjects() {
        // Move each environment object along the global z-axis
        foreach (GameObject obj in environmentObjects)
        {
            if (!obj) {
                continue;
            }
            // Don't move children of Connectors
            if (obj.transform.parent == null || !obj.transform.parent.CompareTag("Connector")) {   
                obj.transform.Translate(Vector3.back * moveSpeed * superBubbleSpeedMultiplier * Time.deltaTime, Space.World);
            }
        }
    }

    public void ChangeSpeeds(float[] newSpeed, float time) {
        StartCoroutine(ChangeSpeedAsync(newSpeed, time));
    }

    // Change the speed of environment to series of values. Each takes 'time' seconds
    IEnumerator ChangeSpeedAsync(float[] newSpeeds, float time) {
        moveSpeed = 0;
        foreach (float newSpeed in newSpeeds) {
            float timeElapsed = 0;
            float initialSpeed = moveSpeed;
            while (timeElapsed < time) 
            {
                timeElapsed += Time.deltaTime;
                moveSpeed = Mathf.Lerp(initialSpeed, newSpeed, timeElapsed / time);
                yield return null;
            }
            moveSpeed = newSpeed;
        }
    }

    public float GetSpeed() 
    {
        return moveSpeed;
    }


    bool RemoveObjects() {
        // Remove objects that have been deleted
        environmentObjects.RemoveAll(obj => obj == null);

        // Remove objects that have moved out of the camera view
        int numObjects = environmentObjects.Count;
        foreach (var obj in environmentObjects.ToArray())
        {
            if (obj.transform.position.z < -pipeLength) {
                Destroy(obj);
                environmentObjects.Remove(obj);

                if (obj.CompareTag("Connector")) {
                    // If the item removed was a connector, set connectorActive to false to start spawning again
                    connectorActive = false;
                }
            }
        }
        bool removedObjects = numObjects != environmentObjects.Count;

        return removedObjects;
    }

    void SpawnObjects() {
        if (!connectorActive) {
            // Only spawn items if there is no active connector
            if (Random.Range(0, 2) == 0) {
                InstantiateConnector();
            }
            else {
                // Instantiate a straight pipe at the end of the currently existing pipes
                InstantiateEnvironment(pipeCount);
            }
        }
    }

    public void InstantiateConnector() {
        var selectedConnector = connectors[UnityEngine.Random.Range(0, connectors.Count)];
        float randAngle = UnityEngine.Random.Range(0, 4) * 90.0f;
        var connector = Instantiate(selectedConnector, pipeCount * new Vector3(0, 0, pipeLength), Quaternion.Euler(0, 0, randAngle), transform);
        environmentObjects.Add(connector);
        connectorActive = true;
        connector.name = "Connector" + totalPipes++;

        // Generate 'pipeCount' pipes in each of the 4 directions attached to the connector
        for (int i = 0; i < 4; i++) {
            Direction dir = i switch {
                0 => Direction.Right,
                1 => Direction.Up,
                2 => Direction.Left,
                3 => Direction.Down,
                _ => Direction.Right
            };
            for (int j = 1; j < pipeCount + 1; j++) {
                float rightSelector = Mathf.Cos(i * Mathf.PI / 2);
                float upSelector = Mathf.Sin(i * Mathf.PI / 2);
                float rightOffset = rightSelector * j * pipeLength;
                float upOffset = upSelector * j * pipeLength;
                var straightPipe = Instantiate(straightPipePrefab, new Vector3(rightOffset, upOffset, pipeLength * pipeCount), Quaternion.Euler(upSelector * 90, rightSelector * 90, 0), connector.transform);
                straightPipe.name = "PipeDir" + dir + j; 
                environmentObjects.Add(straightPipe);
                PlaceCollectibles(straightPipe);
            }
        }
    }

    public void InstantiateEnvironment(int offset = 0) {
        var straightPipe = Instantiate(straightPipePrefab, offset * new Vector3(0, 0, pipeLength), Quaternion.Euler(0, 0, 0), transform);
        environmentObjects.Add(straightPipe);

        if (totalPipes != 0) PlaceCollectibles(straightPipe);
        straightPipe.name = "Pipe" + totalPipes++;
    }

    public void PlaceCollectibles(GameObject pipe) {
        float collectibleSpacing = pipeLength / collectiblesPerPipe;
        for (int i = -collectiblesPerPipe / 2; i < collectiblesPerPipe / 2; i++)
        {
            Collectible collectibleTemplate = GetRandomCollectible();
            GameObject collectible = Instantiate(collectibleTemplate.obj, pipe.transform);

            if (!collectible) break;

            if (collectible.name.StartsWith("Electrical Wires"))
            {
                float angle = Random.Range(0f, Mathf.PI * 2);
                collectible.transform.localPosition = new Vector3(pipeRadius * Mathf.Cos(angle), pipeRadius * Mathf.Sin(angle), collectibleSpacing * i);
                collectible.transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg - 90f);
            }
            else 
            {
                collectible.transform.localPosition = new Vector3(UnityEngine.Random.Range(-pipeRadius, pipeRadius), UnityEngine.Random.Range(-pipeRadius, pipeRadius), collectibleSpacing * i);
            }
        }
    }

    private Collectible GetRandomCollectible() {
        float totalWeight = collectibles.Sum(c => c.weight);
        float rand = Random.Range(0.0f, totalWeight);
        float thresh = 0.0f;
        for (int i = 0; i < collectibles.Count; i++)
        {
            thresh += collectibles[i].weight;
            if (rand < thresh)
            {
                return collectibles[i];
            }
        }

        return collectibles[0];
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

    public void SetSuperBubbleSpeedMultiplier(float seconds, float multiplier)
    {
        StartCoroutine(superBubbleSpeedMultiplierTimer(seconds, multiplier));
    }

    private IEnumerator superBubbleSpeedMultiplierTimer(float seconds, float multiplier)
    {
        superBubbleSpeedMultiplier = multiplier;
        yield return new WaitForSeconds(seconds);
        superBubbleSpeedMultiplier = 1.0f;
    }

}