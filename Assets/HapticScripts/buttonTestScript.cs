using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class buttonTestScript : MonoBehaviour {

    /*public GameObject panel = null;
    public GameObject ans1 = null;
    public GameObject ans2;
    public GameObject ans3;
    public GameObject ans4;
    public GameObject ans5;

    public void testButton() {
		Debug.Log("BUTTON PRESSED!");
	}*/

    public void ToggleVisibity()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();

        if (rend.enabled)
            rend.enabled = false;
        else
            rend.enabled = true;
    }
    /*
    public void testButtonNext()
    {
        ans1.GetComponent<Image>().color = Color.white;
        //panel.GetComponent<Image>().color = Color.red;
    }
    public void testButtonRed()
	{
		if (panel != null)
			panel.GetComponent<Image>().color = Color.red;
    }
	public void testButtonGreen()
	{
		if (panel != null)
			panel.GetComponent<Image>().color = Color.green;
	}
	public void testButtonBlue()
	{
		if (panel != null)
			panel.GetComponent<Image>().color = Color.blue;
	}*/
}
