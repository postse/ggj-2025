using UnityEngine;
using UnityEngine.UI;

public class InstructionsController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button goBackButton;
    void Start()
    {
        goBackButton.Select();
    }

    public void GoBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
