using System.Collections;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public int airReservoir = 10;
    public float bubblePopTimerFrequency = 1.0f;

    public int score = 0;
    public int scorePerSecond = 1;

    private BubbleBarUI uiController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(BubblePopTimer());
        StartCoroutine(ScoreTimer());
        uiController = GameObject.Find("AirUI").GetComponentInChildren<BubbleBarUI>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bubble"))
        {
            AddBubbleToReservoir();
            Destroy(other.gameObject);
        }
    }

    IEnumerator BubblePopTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(bubblePopTimerFrequency);
            if (airReservoir > 0)
            {
                airReservoir--;
            }

            if (airReservoir == 0)
            {
                GameOver();
            }
        }
    }

    IEnumerator ScoreTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            AddScore(scorePerSecond);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void RemoveBubbleFromReservoir()
    {
        airReservoir--;
        uiController.PopBubble();
    }

    public void AddBubbleToReservoir()
    {
        airReservoir++;
        uiController.AddBubble();
    }

    void GameOver() {
        Debug.Log("GAME OVER");
        var bubbles = GameObject.FindGameObjectsWithTag("Bubble");
        foreach (var bubble in bubbles)
        {
            Destroy(bubble);
        }
    }
}
