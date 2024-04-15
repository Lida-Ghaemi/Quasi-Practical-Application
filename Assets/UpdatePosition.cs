using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosition : MonoBehaviour
{
    public Transform objectToFollow; // Reference to the object whose position we want to synchronize
    //private Vector3 initialOffset; // Initial offset between the two objects' positions
    public Vector3 heliInitialPosition;
    public float initialOffsetX, initialOffsetY;

    void Start()
    {
        // Calculate the initial offset between the two objects' positions
        initialOffsetX = objectToFollow.position.x - transform.position.x;
        //initialOffsetY = objectToFollow.position.y - transform.position.y;
        heliInitialPosition =  new Vector3(6.2f, -7.5f, -25.45f); //transform.position;
        //initialOffsetY = objectToFollow.position.y - transform.position.y;
    }

    void Update()
    {
        ////// Calculate the new position for this object along the x-axis
        //float newXPosition = objectToFollow.position.x - initialOffsetx;

        ////// Update the position of this object to match the new x-position
        //transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);


        // Calculate the change in the x position of the followed object
        //print("initialOffsetX     " + initialOffsetX);
        float deltaX = objectToFollow.position.x - (transform.position.x + initialOffsetX);
        //float deltaY = objectToFollow.position.y - (transform.position.y + initialOffsety);
        //print("deltaX     " + deltaX);
        //print("position:   " + heliInitialPosition);
        // Update the y position of this object based on the change in the x position of the followed object
        transform.position += new Vector3(0f,0f , deltaX / 80.0f);

        //transform.position.y += 3.0f*Time.deltaTime;

    }
}
