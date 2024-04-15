using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    ////public GameObject objectToActivate; // The object to activate/deactivate
    //public GameObject MainCamera; // First camera
    //public GameObject CookpitCamera; // Second camera

    //void Start()
    //{
    //    MainCamera = GameObject.Find("MainCamera");
    //    CookpitCamera = GameObject.Find("CookpitCamera");
    //    // Disable the second camera initially
    //    //if (camera2 != null)
    //    //{
    //    MainCamera.enabled = true;
    //    MainCamera.gameObject.SetActive(true);
    //    CookpitCamera.gameObject.SetActive(false);
    //    //}
    //}
    //public void SwitchCamera()
    //{
    //    if (MainCamera != null && CookpitCamera != null)
    //    {
    //        MainCamera.gameObject.SetActive(!MainCamera.gameObject.activeSelf);
    //        CookpitCamera.gameObject.SetActive(!CookpitCamera.gameObject.activeSelf);
    //    }
    //}

    ///////////////////////
    ///public GameObject objectToActivate; // The object to activate/deactivate
    public Camera camera1; // First camera
    public GameObject Heli; // Second camera
    public GameObject grayCover;
    public GameObject blackCover, heliCopter;
    public Transform objectToFollow1, objectFollow1;
    public float test;
    public Vector3 heliInitialPosition;
    void Start()
    {
        heliCopter = GameObject.Find("R22_GRP");
        heliInitialPosition = heliCopter.transform.position;
        test = 0;
        // Disable the second camera initially
        if (Heli != null)
        {
            Heli.gameObject.SetActive(false);
        }
    }

    public void SwitchCamera()
    {
        if (camera1 != null && Heli != null)
        {
            camera1.gameObject.SetActive(!camera1.gameObject.activeSelf);
            Heli.gameObject.SetActive(!Heli.gameObject.activeSelf);
            grayCover.gameObject.SetActive(!grayCover.gameObject.activeSelf);
            blackCover.gameObject.SetActive(!blackCover.gameObject.activeSelf);
            //UpdatePosition.FindObjectOfType<UpdatePosition>().initialOffsetX=objectToFollow1.position.x - objectFollow1.position.x;
            heliCopter.transform.position = heliInitialPosition;// UpdatePosition.FindObjectOfType<UpdatePosition>().heliInitialPosition;
            print("switch tooooooooooooo    " + heliCopter.transform.position);
        }
    }
}
