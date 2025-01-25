using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnMove(InputValue inputValue)
    {
        Debug.Log(inputValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
