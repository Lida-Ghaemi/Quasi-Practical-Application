using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

//! This MonoBehavior can be attached to any object with a collider. It will apply a haptic "effect"
//! to any haptic stylus that is within the boundries of the collider.
//! The parameters can be adjusted on the fly.
public class HapticEffect : MonoBehaviour {

    public enum EFFECT_TYPE { CONSTANT };//, VISCOUS, SPRING, FRICTION, VIBRATE };
    public EFFECT_TYPE effectType = EFFECT_TYPE.CONSTANT;

    //public Text forceType;
    //public Text forceMagnitude;
    [DisplayOnlyAttribute] public float stylusX = 0; //public Text stylusX; 
    [DisplayOnlyAttribute] public float stylusY = 0; //public Text stylusY; 
    [DisplayOnlyAttribute] public float stylusZ = 0; //public Text stylusZ; 
    private float stylusXX;
    private float stylusYY;
    private float stylusZZ;

    // Public, User-Adjustable Settings
    //public EFFECT_TYPE effectType = EFFECT_TYPE.VISCOUS; //!< Which type of effect occurs within this zone?

    [Range(0.0f, 1.0f)] public double Magnitude = 0.34f;
    [Range(0.0f, 1.0f)] private double Gain = 0.333f;

    [Range(1.0f, 1000.0f)] private double Frequency = 200.0f;
    //private double Duration = 1.0f;
    private Vector3 Position = Vector3.zero;
    public Vector3 Direction = Vector3.down;


    // Keep track of the Haptic Devices
    HapticPlugin[] devices;
    bool[] inTheZone;       //Is the stylus in the effect zone?
    Vector3[] devicePoint;  // Current location of stylus
    float[] delta;          // Distance from stylus to zone collider.
    int[] FXID;             // ID of the effect.  (Per device.)

    // These are the user adjustable vectors, converted to world-space. 
    private Vector3 focusPointWorld = Vector3.zero;
    private Vector3 directionWorld = Vector3.down;

    //[Header("ReadOnly ForceDir")]
    //[DisplayOnlyAttribute] public Vector3 stylusPositionRaw;    //!< (Readonly) Stylus position, in device coordinates.
    //[DisplayOnlyAttribute] public float touchingDepth = 0;

    //! Start() is called at the beginning of the simulation.
    //!
    //! It will identify the Haptic devices, initizlize variables internal to this script, 
    //! and request an Effect ID from Open Haptics. (One for each device.)
    //!
    void Start()
    {
        //Initialize the list of haptic devices.
        devices = (HapticPlugin[])Object.FindObjectsOfType(typeof(HapticPlugin));
        inTheZone = new bool[devices.Length];
        devicePoint = new Vector3[devices.Length];
        delta = new float[devices.Length];
        FXID = new int[devices.Length];

        // Generate an OpenHaptics effect ID for each of the devices.
        for (int ii = 0; ii < devices.Length; ii++)
        {
            inTheZone[ii] = false;
            devicePoint[ii] = Vector3.zero;
            delta[ii] = 0.0f;
            FXID[ii] = HapticPlugin.effects_assignEffect(devices[ii].configName);
        }
    }


    //!  Update() is called once per frame.
    //! 
    //! This function 
    //! - Determines if a haptic stylus is inside the collider
    //! - Updates the effect settings.
    //! - Starts and stops the effect when appropriate.
    void Update()
    {
        // Find the pointer to the collider that defines the "zone". 
        Collider collider = gameObject.GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("This Haptic Effect Zone requires a collider");
            return;
        }

        // Update the World-Space vectors
        focusPointWorld = transform.TransformPoint(Position);
        //directionWorld =  transform.TransformDirection(Direction);

        // Update the effect seperately for each haptic device.
        for (int ii = 0; ii < devices.Length; ii++)
        {
            HapticPlugin device = devices[ii];
            bool oldInTheZone = inTheZone[ii];
            int ID = FXID[ii];

            // If a haptic effect has not been assigned through Open Haptics, assign one now.
            if (ID == -1)
            {
                FXID[ii] = HapticPlugin.effects_assignEffect(devices[ii].configName);
                ID = FXID[ii];

                if (ID == -1) // Still broken?
                {
                    Debug.LogError("Unable to assign Haptic effect.");
                    continue;
                }
            }

            // Determine if the stylus is in the "zone". 
            Vector3 StylusPos = device.stylusPositionWorld; //World Coordinates
            Vector3 CP = collider.ClosestPoint(StylusPos);  //World Coordinates
            devicePoint[ii] = CP;
            delta[ii] = (CP - StylusPos).magnitude;

            //If the stylus is within the Zone, The ClosestPoint and the Stylus point will be identical.
            if (delta[ii] <= Mathf.Epsilon)
            {
                inTheZone[ii] = true;

                double Mag = Magnitude;
                if (device.isInSafetyMode())
                    Mag = 0;

                stylusXX = StylusPos.x;//stylusPositionRaw.x;//proxyPositionRaw
                stylusYY = StylusPos.y;
                stylusZZ = StylusPos.z;

                //string forceTypeName = System.Enum.GetName(typeof(EFFECT_TYPE), (int)effectType);
                //float m = map(stylusZZ, -60f, 0f, 0.2f, 0.0f); //float m = map(value, low1, high1, low2, high2);
                //prasdfnt(stylusXX);
                
                //Updatepos(Mag, stylusXX, stylusYY, stylusZZ);
                Vector3 cDirection = UpdateParameters(Mag, Direction, stylusXX, stylusYY, stylusZZ);
                Direction = cDirection; 
                directionWorld = transform.TransformDirection(Direction);

                // Convert from the World coordinates to coordinates relative to the haptic device.
                Vector3 focalPointDevLocal = device.transform.InverseTransformPoint(focusPointWorld);
                Vector3 rotationDevLocal = device.transform.InverseTransformDirection(directionWorld);
                double[] pos = { focalPointDevLocal.x, focalPointDevLocal.y, focalPointDevLocal.z };
                double[] dir = { rotationDevLocal.x, rotationDevLocal.y, rotationDevLocal.z };


                // Send the current effect settings to OpenHaptics.
                double hlStiffness = 0;
                double hlDamping = 0;
                double hlStaticFriction = 0;
                double hlDynamicFriction = 0;
                double hlPopThrough = 1;
                HapticPlugin.shape_settings(ID, hlStiffness, hlDamping, hlStaticFriction, hlDynamicFriction, hlPopThrough);
                HapticPlugin.effects_settings(
                    device.configName,
                    ID,
                    Gain,
                    Mag,
                    Frequency,
                    pos,
                    dir);
                HapticPlugin.effects_type(
                    device.configName,
                    ID,
                    (int)effectType);

            } else
            {
                inTheZone[ii] = false;

                // Note : If the device is not in the "Zone", there is no need to update the effect settings.
            }

            // If the on/off state has changed since last frame, send a Start or Stop event to OpenHaptics
            if (oldInTheZone != inTheZone[ii])
            {
                if (inTheZone[ii])
                {
                    HapticPlugin.effects_startEffect(device.configName, ID);
                } else
                {
                    HapticPlugin.effects_stopEffect(device.configName, ID);
                }
            }

        }
    }

    /*void Updatepos(double Mag, float stylusXX, float stylusYY, float stylusZZ)
    {
        //forceType.text = "ForceType: " + forceTypeName;
        //forceMagnitude.text = "ForceMag: " + Mag.ToString("0.000");
        Magnitude = Mag;
        stylusX = stylusXX;
        stylusY = stylusYY;
        stylusZ = stylusZZ;
    }*/
    private Vector3 UpdateParameters(double Mag, Vector3 Direction, float stylusXX, float stylusYY, float stylusZZ)
    {
        Mag = 0.2f;
        Magnitude = Mag;
        stylusX = stylusXX;
        stylusY = stylusYY;
        stylusZ = stylusZZ;
        float fenmu = Mathf.Sqrt(Mathf.Pow(Mathf.Cos(stylusX),2f) + Mathf.Pow(Mathf.Sin(stylusZ),2f) + 1);
        //Direction.x = 0; Mathf.Cos(stylusX) / fenmu; // - 0.68f;
        //Direction.y = 0;stylusY / fenmu; 
        //Direction.z = 0;-Mathf.Sin(stylusZ) / fenmu; //0.0f;
        Direction.x = Mathf.Cos(stylusX) / fenmu; // - 0.68f;
        Direction.y = -Mathf.Sin(stylusY) / fenmu; //1 / fenmu; 
        Direction.z = 1 / fenmu; //-Mathf.Sin(stylusZ) / fenmu; //0.0f;
        return Direction;
    }   
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
    void OnDestroy()
	{
		//For every haptic device, send a Stop event to OpenHaptics
		for (int ii = 0; ii < devices.Length; ii++)
		{
			HapticPlugin device = devices [ii];
			if (device == null)
				continue;
			int ID = FXID [ii];
			HapticPlugin.effects_stopEffect(device.configName, ID);
		}
	}
	void OnDisable()
	{
		//For every haptic device, send a Stop event to OpenHaptics
		for (int ii = 0; ii < devices.Length; ii++)
		{
			HapticPlugin device = devices [ii];
			if (device == null)
				continue;
			int ID = FXID [ii];
			HapticPlugin.effects_stopEffect(device.configName, ID);
			inTheZone [ii] = false;
		}
	}


	//! OnDrawGizmos() is called only when the Unity Editor is active.
	//! It draws some hopefully useful wireframes to the editor screen.
	 
	void OnDrawGizmos()
	{
		Gizmos.matrix = Matrix4x4.identity;
		Gizmos.matrix = this.transform.localToWorldMatrix;

		Gizmos.color = Color.white;

		Ray R = new Ray(); 
		R.direction = Direction;

		if (effectType == EFFECT_TYPE.CONSTANT)
		{
			Gizmos.DrawRay(R);
		}

		Vector3 focusPointWorld = transform.TransformPoint(Position);


		Gizmos.matrix = Matrix4x4.identity;
		Gizmos.color = Color.white;
		/*if (effectType == EFFECT_TYPE.SPRING)
		{
			Gizmos.DrawIcon(focusPointWorld, "anchor_icon.tiff");
		}*/

		if (devices == null)
			return;

		// If the device is in the zone, draw a red marker. 
		// And draw a line indicating the spring force, if we're in that mode.
		for (int ii = 0; ii < devices.Length; ii++)
		{
			if (delta [ii] <= Mathf.Epsilon)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(devicePoint[ii], 1.0f);
				//if(effectType == EFFECT_TYPE.SPRING)
				//	Gizmos.DrawLine(focusPointWorld, devicePoint [ii]);
			}
		}

	}


}

/*
#if UNITY_EDITOR
[CustomEditor(typeof(HapticEffect))]
public class HapticEffectEditor : Editor 
{
	override public void OnInspectorGUI()
	{
		HapticEffect HE = (HapticEffect)target;

		if (HE.gameObject.gameObject.GetComponent<Collider>() == null)
		{
			EditorGUILayout.LabelField("*********************************************************");
			EditorGUILayout.LabelField("   Haptic Effect must be assigned to an object with a collider.");
			EditorGUILayout.LabelField("*********************************************************");

		} else
		{
			HE.effectType = (HapticEffect.EFFECT_TYPE)EditorGUILayout.EnumPopup("Effect Type", HE.effectType);

            string configName = "Default Device";
            double[] posInput = new double[3]; Vector3 pRaw;
            HapticPlugin.getProxyPosition(configName, posInput);
            pRaw.x = (float)posInput[0];
            pRaw.y = (float)posInput[1];
            pRaw.z = (float)posInput[2];

            switch (HE.effectType)
			{
			case HapticEffect.EFFECT_TYPE.CONSTANT:                
				HE.Magnitude = EditorGUILayout.Slider("Force Magnitude", (float)HE.Magnitude, 0.0f, 1.0f);                
                HE.Direction = EditorGUILayout.Vector3Field("Force Direction", HE.Direction);
                HE.Position = EditorGUILayout.Vector3Field("XYZ Coordinates", HE.Position);
                HE.Position.x = pRaw.x;
				break;
			/*case HapticEffect.EFFECT_TYPE.FRICTION:
				HE.Gain = EditorGUILayout.Slider("Gain", (float)HE.Gain, 0.0f, 1.0f);
				HE.Magnitude = EditorGUILayout.Slider("Magnitude", (float)HE.Magnitude, 0.0f, 1.0f);
				break;
			case HapticEffect.EFFECT_TYPE.SPRING:
				HE.Gain = EditorGUILayout.Slider("Gain", (float)HE.Gain, 0.0f, 1.0f);
				HE.Magnitude = EditorGUILayout.Slider("Magnitude", (float)HE.Magnitude, 0.0f, 1.0f);
				HE.Position = EditorGUILayout.Vector3Field("Position", HE.Position);
				break;
			case HapticEffect.EFFECT_TYPE.VIBRATE:
				HE.Gain = EditorGUILayout.Slider("Gain", (float)HE.Gain, 0.0f, 1.0f);
				HE.Magnitude = EditorGUILayout.Slider("Magnitude", (float)HE.Magnitude, 0.0f, 1.0f);
				HE.Frequency = EditorGUILayout.Slider("Frequency", (float)HE.Frequency, 1.0f, 1000.0f);
				HE.Direction = EditorGUILayout.Vector3Field("Direction", HE.Direction);
				break;
			case HapticEffect.EFFECT_TYPE.VISCOUS:
				HE.Gain = EditorGUILayout.Slider("Gain", (float)HE.Gain, 0.0f, 1.0f);
				HE.Magnitude = EditorGUILayout.Slider("Magnitude", (float)HE.Magnitude, 0.0f, 1.0f);
				break;

			}
		}

	}

}

#endif
*/





