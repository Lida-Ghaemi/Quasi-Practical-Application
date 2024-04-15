using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRenderor : MonoBehaviour
{
    public void ToggleVisibity()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();

        if (rend.enabled)
            rend.enabled = false;
        else
            rend.enabled = true;
    }
}
