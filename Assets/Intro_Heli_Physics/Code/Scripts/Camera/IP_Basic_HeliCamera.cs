using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    public class IP_Basic_HeliCamera : IP_Base_HeliCamera, IP_IHeliCamera
    {
        #region Variables
        [Header("Basic Camera Properties")]
        public float height = 2f;//how heigh we are above the helicopter
        public float distance = 2f;//how far we are  back of the helicopter
        public float smoothSpeed = 0.35f;
        #endregion


        #region builtin Methods
        void OnEnable()
        {
            updateEvent.AddListener(UpdateCamera);
        }

        void OnDisable()// it will be called when we turn the script off
        {
            updateEvent.RemoveListener(UpdateCamera);
        }
        #endregion

        #region Interface Methods
        public void UpdateCamera()
        {
            //wanted position
            wantedPos = rb.position + (targetFlatFwd * distance) + (Vector3.up * height);

            //lets position the camera
            transform.position = Vector3.SmoothDamp(transform.position, wantedPos, ref refVelocity, smoothSpeed);
            transform.LookAt(lookAtTarget);
        }
        #endregion
    }
}
