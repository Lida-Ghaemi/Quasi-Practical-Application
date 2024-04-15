using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRottaion : MonoBehaviour
{
    float x;
    bool left_right = true;
    // Update is called once per frame
    void Update()
    {
        
        if (left_right & x > -1)
        {
            x -= Time.deltaTime * 0.9f;
            //transform.rotation = Quaternion.Euler(-90, x, 0);
            transform.position = new Vector3(x, 0, 0);
        }
        else
        {
            left_right = false;
        }
        if (!left_right & x < 1)
        {
            x += Time.deltaTime * 0.9f;
            //transform.rotation = Quaternion.Euler(-90, x, 0);
            transform.position = new Vector3(x, 0, 0);
        }
        else
        {
            left_right = true;
        }
       
    }
    }
