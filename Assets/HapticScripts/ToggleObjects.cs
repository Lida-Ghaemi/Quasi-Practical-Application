using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject[] objects;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        foreach (GameObject go in objects)
        {
            bool active = GUILayout.Toggle(go.activeSelf, go.name);
            if (active != go.activeSelf)
                go.SetActive(active);
        }
    }

}

