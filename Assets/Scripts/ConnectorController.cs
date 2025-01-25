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

    public float turningSpeed = 3.0f;
    private Quaternion ogRot = Quaternion.identity;
    private string turnDir = string.Empty;

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
        // if (Time.time < 3) return;
        if (turnDir != string.Empty)
        {
            Turn(turnDir);
        }

        if (transform.childCount < 3) {
            Destroy(gameObject);
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

    public void Turn(string dir) 
    {
        float speed = turningSpeed * ( 1f - Mathf.Exp( -Time.deltaTime ) );
        Vector3 ogEuler = transform.rotation.eulerAngles;
        Quaternion targetRot = Quaternion.identity;

        if (string.Equals(dir, "Right") && Right)
            targetRot = Quaternion.Euler(ogEuler.x, -90.0f, ogEuler.z);
        else if (string.Equals(dir, "Up") && Up)
            targetRot = Quaternion.Euler(90.0f, ogEuler.y, ogEuler.z);
        else if (string.Equals(dir, "Left") && Left)
            targetRot = Quaternion.Euler(ogEuler.x, 90.0f, ogEuler.z);
        else if (string.Equals(dir, "Down") && Down)
            targetRot = Quaternion.Euler(-90.0f, ogEuler.y, ogEuler.z);
        else return;

        transform.rotation = Quaternion.Slerp( transform.rotation, targetRot, speed );
    }
    
    public void StartTurning(string dir) 
    {
        turnDir = dir;
    }

}
