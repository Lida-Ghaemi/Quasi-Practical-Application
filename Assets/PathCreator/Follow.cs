using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follow : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 1.0f;
    float distanceTravelled;

    // Update is called once per frame
    void Update()
    {
        Moving();
        
    }

    public void Moving()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
    }
}
