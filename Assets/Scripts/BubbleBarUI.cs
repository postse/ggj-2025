using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleBarUI : MonoBehaviour
{
    public GameObject BubbleUIPrefab;

    private float bubbleRadius = 25.0f;

    private List<GameObject> bubbles = new List<GameObject>();
    private AudioSource popSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popSound = GetComponent<AudioSource>();

    }

    public void SetBubbles(int bubbleCount)
    {
        DestroyAllBubbles();

        float startX = -bubbleRadius * bubbleCount + bubbleRadius;
        Vector3 pos = new Vector3(startX + 2 * bubbleRadius * bubbleCount, 0.0f, 0.0f);

        for (int i = 0; i < bubbleCount; i++)
        {
            Debug.Log("Creating bubble: " + i);
            pos.x -= 2 * bubbleRadius;
            GameObject bubble = Instantiate(BubbleUIPrefab, transform.position + pos, Quaternion.identity, transform);
            bubbles.Add(bubble);
        }
    }

    public void DestroyAllBubbles()
    {
        foreach (GameObject bubble in bubbles)
        {
            Destroy(bubble);
        }
        bubbles.Clear();
    }
}
