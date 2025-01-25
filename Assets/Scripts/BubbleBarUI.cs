using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleBarUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject BubbleUIPrefab;
    public int maxBubbleCnt = 0;

    private int bubbleCnt = 0;
    private float bubbleRadius = 25.0f;

    private List<GameObject> bubbles = new List<GameObject>();
    private AudioSource popSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create bubble UI
        for (int i = 0; i < maxBubbleCnt; i++) {
            AddBubble();
        }

        popSound = GetComponent<AudioSource>();
        
    }

    public void AddBubble()
    {
        if (bubbleCnt >= maxBubbleCnt) return;

        float startX = -bubbleRadius * maxBubbleCnt + bubbleRadius;
        Vector3 pos = new Vector3(startX + 2 * bubbleRadius * bubbleCnt, 0.0f, 0.0f);
        GameObject bubble = Instantiate(BubbleUIPrefab, transform.position + pos, Quaternion.identity, transform);
        bubbles.Add(bubble);
        bubbleCnt++;
    }

    public void PopBubble()
    {
        GameObject bubble = bubbles[bubbles.Count-1];
        bubbles.Remove(bubble);
        Destroy(bubble);
        bubbleCnt--;
        popSound.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            PopBubble();
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            AddBubble();
        }
    }

}
