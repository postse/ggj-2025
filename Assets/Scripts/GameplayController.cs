using System;
using System.Collections;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public int airReservoir = 10;
    public int maxAirReservoir = 10;
    public float bubblePopTimerFrequency = 2.0f;

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
            BubbleController bubble = other.gameObject.GetComponent<BubbleController>();
            bubble.Interact();
        } else if (other.gameObject.CompareTag("Obstacle")) {
            ObstacleController obstacle = other.gameObject.GetComponentInParent<ObstacleController>();
            RemoveBubbleFromReservoir(obstacle.damage);
            obstacle.Interact();
        }
    }

    IEnumerator BubblePopTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(bubblePopTimerFrequency);
            if (airReservoir > 0)
            {
                RemoveBubbleFromReservoir();
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

    public void RemoveBubbleFromReservoir(int bubblesToRemove = 1)
    {
        airReservoir = Math.Max(0, airReservoir - bubblesToRemove);
        for (int i = 0; i < bubblesToRemove; i++) {
            uiController.PopBubble();
        }
    }

    public void AddBubbleToReservoir(int bubblesToAdd = 1)
    {
        airReservoir = Math.Min(maxAirReservoir, airReservoir + bubblesToAdd);
        for (int i = 0; i < bubblesToAdd; i++)
        {
            uiController.AddBubble();
        }
    }

    void GameOver() {
        Debug.Log("GAME OVER");
        // var bubbles = GameObject.FindGameObjectsWithTag("Bubble");
        // foreach (var bubble in bubbles)
        // {
        //     Destroy(bubble);
        // }
    }
}
