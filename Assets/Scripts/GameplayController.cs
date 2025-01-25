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

    private ConnectorController activeConnector;

    private HurtOverlayController hurtOverlayController;
    private EnvironmentController environmentController;
    private MovementController movementController;

    public bool isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(BubblePopTimer());
        StartCoroutine(ScoreTimer());

        airUiController = airUi.GetComponentInChildren<BubbleBarUI>();
        hurtOverlayController = FindFirstObjectByType<HurtOverlayController>();
        environmentController = FindFirstObjectByType<EnvironmentController>();
        movementController = FindFirstObjectByType<MovementController>();

        airUiController.SetBubbles(airReservoir);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.gameObject.CompareTag("Bubble"))
        {
            AddBubbleToReservoir();
            BubbleController bubble = other.gameObject.GetComponent<BubbleController>();
            bubble.Interact();
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            ObstacleController obstacle = other.gameObject.GetComponentInParent<ObstacleController>();
            RemoveBubbleFromReservoir(obstacle.damage, true);
            obstacle.Interact();
            hurtOverlayController.FlashOverlay(0.2f, 0.2f);
        } else if (other.gameObject.CompareTag("Connector")) {
            activeConnector = other.gameObject.GetComponentInParent<ConnectorController>();
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.CompareTag("Connector")) {
            activeConnector = null;
        }
    }

    public ConnectorController GetActiveConnector() {
        return activeConnector;
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

    public void RemoveBubbleFromReservoir(int bubblesToRemove = 1, bool fromObstacle = false)
    {
        airReservoir = Math.Max(0, airReservoir - bubblesToRemove);
        for (int i = 0; i < bubblesToRemove; i++)
        {
            airUiController.SetBubbles(airReservoir);

            if (fromObstacle)
            {
                hurtOverlayController.FlashOverlay(0.2f, 0.2f);
            }
        }

        if (airReservoir <= 0)
        {
            GameOver();
        }
    }

    public void AddBubbleToReservoir(int bubblesToAdd = 1)
    {
        airReservoir = Math.Min(maxAirReservoir, airReservoir + bubblesToAdd);
        airUiController.SetBubbles(airReservoir);
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        if (isGameOver) return;

        isGameOver = true;

        hurtOverlayController.FadeOverlay(1.0f, 0.3f);
        environmentController.StopMovement(3.0f);
        movementController.StopMovement();

        airUi.SetActive(false);
        gameOverUI.SetActive(true);
        GameObject.Find("GameOverScoreText").GetComponent<TextMeshProUGUI>().SetText("Score: " + score);
    }
}
