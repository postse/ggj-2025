using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    enum SelectedMenuItem
    {
        Start,
        Instructions,
        Exit
    }

    public Button startButton;

    void Start()
    {
        AudioSource audioSource = startButton.GetComponent<AudioSource>();
        audioSource.enabled = false;
        startButton.Select();
        audioSource.enabled = true;

    }

    // Update is called once per frame
    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
