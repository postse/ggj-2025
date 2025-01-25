using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public Button startButton;
    public Button exitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
