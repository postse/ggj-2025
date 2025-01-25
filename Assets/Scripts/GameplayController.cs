using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public int airReservoir = 10;
    public int maxAirReservoir = 10;
    public float bubblePopTimerFrequency = 2.0f;

    public int score = 0;
    public int scorePerSecond = 1;
    public GameObject gameOverUI;
    public GameObject airUi;

    private BubbleBarUI airUiController;

    private AudioSource bubblePopSound;

    private bool isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(BubblePopTimer());
        StartCoroutine(ScoreTimer());

        airUiController = airUi.GetComponentInChildren<BubbleBarUI>();

        bubblePopSound = GetComponent<AudioSource>();
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
        } else if (other.gameObject.CompareTag("Connector")) {
            Debug.Log("Connector!");
            other.gameObject.GetComponentInParent<ConnectorController>().StartTurning();
        }
    }

    IEnumerator BubblePopTimer()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(bubblePopTimerFrequency);
            if (airReservoir > 0)
            {
                RemoveBubbleFromReservoir();
            }
        }
    }

    IEnumerator ScoreTimer()
    {
        while (!isGameOver)
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
            airUiController.PopBubble();
        }

        if (airReservoir == 0)
        {
            GameOver();
        }
    }

    public void AddBubbleToReservoir(int bubblesToAdd = 1)
    {
        airReservoir = Math.Min(maxAirReservoir, airReservoir + bubblesToAdd);
        for (int i = 0; i < bubblesToAdd; i++)
        {
            airUiController.AddBubble();
        }
    }

    void GameOver() {
        if (isGameOver) return;

        isGameOver = true;
        
        airUi.SetActive(false);
        gameOverUI.SetActive(true);
        GameObject.Find("GameOverScoreText").GetComponent<TextMeshProUGUI>().SetText("Score: " + score);
    }
}
