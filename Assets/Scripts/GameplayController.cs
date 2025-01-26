using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public int airReservoir = 10;
    public int maxAirReservoir = 10;
    public float bubblePopTimerFrequency = 2.0f;
    public int bubbleValue = 5;

    public int score = 0;
    public int scorePerSecond = 1;
    private GameObject gameOverUI;
    private GameObject airUi;
    private GameObject scoreUI;

    private BubbleBarUI airUiController;

    private ConnectorController activeConnector;

    private HurtOverlayController hurtOverlayController;
    private EnvironmentController environmentController;
    private MovementController movementController;
    private CameraController cameraController;
    private AirTextController airTextController;
    public AudioSource wallBonkSound;

    public bool isGameOver = false;

    public GameObject hampter;
    public float wiggleSpeed = 1.0f;
    public float maxWiggle = 90.0f;
    public float wiggleOffset = 0.0f;
    public float bounceOffWallTime = 0.5f;
    public float superBubbleInvincibilityTime = 5.0f;
    public bool superBubbleEnabled = false;
    public bool collisionDisabled = false;
    public int wallBounceDmg = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(BubblePopTimer());
        StartCoroutine(ScoreTimer());

        GameObject canvas = GameObject.Find("Canvas");
        gameOverUI = canvas.transform.Find("GameOverUI").gameObject;
        airUi = canvas.transform.Find("AirUI").gameObject;
        scoreUI = canvas.transform.Find("ScoreUI").gameObject;

        airUiController = airUi.GetComponentInChildren<BubbleBarUI>();
        hurtOverlayController = FindFirstObjectByType<HurtOverlayController>();
        environmentController = FindFirstObjectByType<EnvironmentController>();
        movementController = FindFirstObjectByType<MovementController>();
        cameraController = FindFirstObjectByType<CameraController>();
        airTextController = FindFirstObjectByType<AirTextController>();

        airUiController.SetBubbles(airReservoir);
    }

    void Update()
    {
        if (isGameOver) return;
        hampter.transform.rotation = Quaternion.Euler(maxWiggle * Mathf.Sin(Time.time * wiggleSpeed) + wiggleOffset, 0, 90);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (string.Equals(collision.gameObject.name, "WallTrigger") && !collisionDisabled)
        {
            collisionDisabled = true;
            StartCoroutine(CollisionDisablerAsync(bounceOffWallTime));
            float moveSpeed = environmentController.GetSpeed();
            environmentController.ChangeSpeeds(new float[] {-moveSpeed, moveSpeed}, bounceOffWallTime);
            cameraController.ShakeCamera(0.2f, 0.5f);
            RemoveBubbleFromReservoir(wallBounceDmg, true);
            wallBonkSound.Play();
        }
    }

    IEnumerator CollisionDisablerAsync(float invincibilityTime)
    {
        yield return new WaitForSeconds(invincibilityTime);
        collisionDisabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.gameObject.CompareTag("Bubble"))
        {
            if (other.gameObject.name.Contains("SuperBubble"))
            {
                AddBubbleToReservoir(maxAirReservoir);
                BubbleController bubble = other.gameObject.GetComponent<BubbleController>();
                bubble.Interact();
                AddScore(bubbleValue * 10);
                superBubbleEnabled = true;
                airTextController.SetInvincibilityText(superBubbleInvincibilityTime);
                environmentController.superBubbleSpeedMultiplier = 2.0f;

                StartCoroutine(DisableSuperBubble());
                environmentController.SetSuperBubbleSpeedMultiplier(superBubbleInvincibilityTime - .5f, 2f);
            }
            else
            {
                AddBubbleToReservoir();
                BubbleController bubble = other.gameObject.GetComponent<BubbleController>();
                bubble.Interact();
                AddScore(bubbleValue);
            }
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            ObstacleController obstacle = other.gameObject.GetComponentInParent<ObstacleController>();
            RemoveBubbleFromReservoir(obstacle.damage, true);
            obstacle.Interact();
        }
        else if (other.gameObject.CompareTag("Connector"))
        {
            activeConnector = other.gameObject.GetComponentInParent<ConnectorController>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Connector"))
        {
            activeConnector = null;
        }
    }

    public ConnectorController GetActiveConnector()
    {
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
            yield return new WaitForSeconds(.5f);
            AddScore(scorePerSecond);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        if (isGameOver) return;

        score += scoreToAdd;
        scoreUI.GetComponentInChildren<TextMeshProUGUI>().SetText("Score: " + score);
    }

    public void RemoveBubbleFromReservoir(int bubblesToRemove = 1, bool fromObstacle = false)
    {
        if (superBubbleEnabled) return;

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

    private IEnumerator DisableSuperBubble()
    {
        yield return new WaitForSeconds(superBubbleInvincibilityTime);
        superBubbleEnabled = false;
    }
}
