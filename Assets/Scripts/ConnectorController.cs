using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public enum Direction {
    Right,
    Up,
    Left,
    Down,
    None
}

public class ConnectorController : MonoBehaviour
{

    private GameObject player;
    public bool AllowRight = false;
    public bool AllowUp = false;
    public bool AllowLeft = false;
    public bool AllowDown = false;

    public float turningSpeed = 5.0f;
    private Quaternion ogRot = Quaternion.identity;
    private Direction turnDir = Direction.None;


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
        // if (turnDir != Direction.None)
        // {
        //     Turn(turnDir);
        // }

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
        bool oldDown = AllowDown;
        AllowDown = AllowLeft;
        AllowLeft = AllowUp;
        AllowUp = AllowRight;
        AllowRight = oldDown;
    }

    public void Turn(Direction dir) 
    {
        Vector3 ogEuler = transform.rotation.eulerAngles;
        Quaternion targetRot = Quaternion.identity;

        if (dir == Direction.Right && AllowRight)
            targetRot = Quaternion.Euler(ogEuler.x, -90.0f, ogEuler.z);
        else if (dir == Direction.Up && AllowUp)
            targetRot = Quaternion.Euler(90.0f, ogEuler.y, ogEuler.z);
        else if (dir == Direction.Left && AllowLeft)
            targetRot = Quaternion.Euler(ogEuler.x, 90.0f, ogEuler.z);
        else if (dir == Direction.Down && AllowDown)
            targetRot = Quaternion.Euler(-90.0f, ogEuler.y, ogEuler.z);
        else return;

        StopAllCoroutines();
        StartCoroutine(TurnToTarget(targetRot));
    }

    IEnumerator TurnToTarget(Quaternion targetRot) {
        // float speed = turningSpeed * ( 1f - Mathf.Exp( -Time.deltaTime ) );
        float speed = turningSpeed * Time.deltaTime;
        // float speed = turningSpeed * Time.deltaTime * Time.deltaTime;
        while (Quaternion.Angle(transform.rotation, targetRot) > 0.1f) 
        {
            transform.rotation = Quaternion.Lerp( transform.rotation, targetRot, speed );
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, speed);
            yield return null;
        }
        transform.rotation = targetRot;
    }
    
    // public void StartTurning(Direction dir) 
    // {
    //     turnDir = dir;
    // }

}
