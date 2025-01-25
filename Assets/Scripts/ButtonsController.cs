using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour 
{
    public void RestartGame() 
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu() 
    {
        Debug.Log("Going to main menu...");
        SceneManager.LoadScene("MainMenu");
    }
}