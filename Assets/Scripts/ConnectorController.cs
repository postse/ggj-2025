using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class ConnectorController : MonoBehaviour
{

    private GameObject player;
    public bool Right = false;
    public bool Up = false;
    public bool Left = false;
    public bool Down = false;

    private Quaternion ogRot = Quaternion.identity;
    private bool playerHasEntered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ogRot = transform.rotation;
        DetectDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHasEntered)
        {
            Turn("Right");
            Turn("Up");
            Turn("Down");
            Turn("Left");
        }
    }

    // Used to initialize the allowed turning directions
    void DetectDirection() {
        float rotZ = transform.rotation.eulerAngles.z;
        if (rotZ <= 45)
        {
        }
        else if (rotZ <= 135)
        {
            RotateTurnValues();
        }
        else if (rotZ <= 225)
        {
            RotateTurnValues();
            RotateTurnValues();
        }
        else if (rotZ <= 315)
        {
            RotateTurnValues();
            RotateTurnValues();
            RotateTurnValues();
        }
    }

    void RotateTurnValues()
    {
        bool oldDown = Down;
        Down = Left;
        Left = Up;
        Up = Right;
        Right = oldDown;
    }

    void Turn(string dir) 
    {
        float speed = 10f*( 1f - Mathf.Exp( -Time.deltaTime ) );
        Quaternion targetRot = Quaternion.identity;

        if (string.Equals(dir, "Right") && Right)
            targetRot = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        else if (string.Equals(dir, "Up") && Up)
            targetRot = Quaternion.Euler(90.0f, 0.0f, 90.0f);
        else if (string.Equals(dir, "Left") && Left)
            targetRot = Quaternion.Euler(0.0f, 90.0f, 180.0f);
        else if (string.Equals(dir, "Down") && Down)
            targetRot = Quaternion.Euler(-90.0f, 0.0f, -90.0f);
        else return;

        transform.rotation = Quaternion.Slerp( transform.rotation, targetRot, speed );

        // Vector3 targetRot = new Vector3(0.0f, 0.0f, 0.0f);

        // if (string.Equals(dir, "Right") && Right)
        //     targetRot = new Vector3(0.0f, -90.0f, 0.0f);
        // else if (string.Equals(dir, "Up") && Up)
        //     targetRot = new Vector3(90.0f, 0.0f, 0.0f);
        // else if (string.Equals(dir, "Left") && Left)
        //     targetRot = new Vector3(0.0f, 90.0f, 0.0f);
        // else if (string.Equals(dir, "Down") && Down)
        //     targetRot = new Vector3(-90.0f, 0.0f, 0.0f);
        // else return;

        // transform.Rotate(speed * targetRot, Space.World);
    }
    
    public void StartTurning() 
    {
        playerHasEntered = true;
    }

}
