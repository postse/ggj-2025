using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum Direction {
    Right = 0,
    Up = 1,
    Left = 2,
    Down = 3
}

public class ConnectorController : MonoBehaviour
{

    private GameObject player;
    public bool AllowRight = false;
    public bool AllowUp = false;
    public bool AllowLeft = false;
    public bool AllowDown = false;

    private float turningSpeed = 200.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        DetectDirection();
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
        StartCoroutine(TurnToTarget(targetRot, dir));
    }

    IEnumerator TurnToTarget(Quaternion targetRot, Direction dir) {
        // float speed = turningSpeed * ( 1f - Mathf.Exp( -Time.deltaTime ) );
        float speed = turningSpeed * Time.deltaTime;
        // float speed = Quaternion.Angle(transform.rotation, targetRot) / 180 * Time.deltaTime;
        while (Quaternion.Angle(transform.rotation, targetRot) > 0.1f) 
        {
            // transform.rotation = Quaternion.Lerp( transform.rotation, targetRot, speed );
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, speed);
            yield return null;
        }
        transform.rotation = targetRot;
        RemovePipes(dir);
    }

    void RemovePipes(Direction dir) {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform) {
            children.Add(child);
        }
        foreach (Transform child in children) {
            if (child.name.StartsWith("PipeDir" + dir)) {
                // This is a pipe in the correct direction
                child.transform.SetParent(child.transform.parent.parent);
                child.tag = "Environment";
            } else if (child.name.StartsWith("PipeDir" + dir)) {
                // This is a pipe in the wrong direction
                child.gameObject.SetActive(false);
            }
            // Otherwise, let our EnvironmentController garbage collect it
        }
    }

}
