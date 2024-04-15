using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class HapticHand : MonoBehaviour {

    HapticPlugin[] devices;
    public path_show path_show;
    private HapticPlugin Haptic = null;
	public Camera Camera = null;
	public Image cursor = null;
    //private bool isdragging = false;
    //private bool buttonStatus = false;          //!< Is the button currently pressed?
    //private GameObject touching = null;         //!< Reference to the object currently touched
    //private GameObject grabbing = null;         //!< Reference to the object currently grabbed

    //GameObject slider_upText = null; 
    //GameObject slider_doText = null; 

    // Use this for initialization
    void Start () 
	{
        // find the Haptic Device
        Haptic = gameObject.GetComponent(typeof(HapticPlugin)) as HapticPlugin;
		if (Haptic == null)
			Debug.LogError("HapticMouse script must be attached to the same object as the HapticPlugin script.");

		// Find the camera
		if (Camera == null)
            Camera = FindObjectOfType<Camera>();
        //slider_upText = GameObject.Find("Slider_upText");
        //slider_doText = GameObject.Find("Slider_doText");
    }
    
	public bool buttonHoldDown = false; //state, so we can determine between a click and a button that's held down since last frame

	void Update () 
	{
		//This is a "click" if we're pressed now, but weren't last frame.
		bool click = false;
		if (buttonHoldDown == false && Haptic.Buttons [0] != 0)
			click = true;
		buttonHoldDown = (Haptic.Buttons [0] != 0);
			

		//Determine the screen position using the stylus position and the camera matrix transforms.
		Vector3 screenPos = Camera.WorldToScreenPoint(Haptic.stylusPositionWorld);

		// In this example, the "cursor" is just a UI element.
		// Moving the system mouse cursor is more dificult, and not really recomended.
		if (cursor != null)
			cursor.rectTransform.position = screenPos;
	
		PointerEventData pointerData = new PointerEventData (EventSystem.current);
		List<RaycastResult> results = new List<RaycastResult> ();

		// Perform a raycast to get a list of all elements under the cursor.
		pointerData.position = screenPos;
		EventSystem.current.RaycastAll(pointerData, results);
        
        // Now that we've found the things. Let's select them ...
        bool selectedAtLeastOneThing = false;
		foreach(RaycastResult R in results)
		{
			GameObject go = R.gameObject;
			Selectable S = go.GetComponent <Selectable>(); 
            if (S != null)
			{
				S.Select();
				selectedAtLeastOneThing = true;
			}

            // If we've found a button, we can click on it.
            
            Button B = go.GetComponent<Button>();
            Slider slider = go.GetComponent<Slider>();
            
            if (B != null)
			{
				if (click)
				{
					B.onClick.Invoke(); 
                    B.Invoke("OnPointerDown",0.0f);
				} 
				if (buttonHoldDown == false)
				{
					B.Invoke("OnPointerUp",0.0f); 
                }
			}
            else
            {
                
                if (buttonHoldDown & (slider != null) & (sinHEffect.FindObjectOfType<sinHEffect>().is_touching == true))
                {//print("enddddd");//Lida//it does not print everything
                    float tem_v = sinHEffect.FindObjectOfType<sinHEffect>().stylPosition.x;                    
                    tem_v = Mathf.Abs(tem_v) *  10;
                    //slider.GetComponent<Slider>().value = tem_v;
                    if (slider.name == "Slider_no")
                    {
                        tem_v = Mathf.Round(Mathf.Abs((tem_v) - 40) * 100 / 30); 
                        slider.value = tem_v;
                        //sinHEffect.FindObjectOfType<sinHEffect>().slider_noTextNo = tem_v; 
                    }
                    if (slider.name == "Slider_yes")
                    {
                        tem_v = Mathf.Round(Mathf.Abs((tem_v) - 10) * 100 / 30);
                        slider.value = tem_v;
                        //sinHEffect.FindObjectOfType<sinHEffect>().slider_yesTextNo = tem_v; 
                        //slider_doText.GetComponent<Text>().text = tem_v.ToString();
                    }
                }
            }

            

                }

                //If we're not hovering over anything, deselect everything.
                // NOTE : This is pretty crude, and will interfere with anyone trying to operate the UI via keyboard.
                if (selectedAtLeastOneThing == false)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}
    

}
