using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using Assets.LSL4Unity.Scripts;
using System.Collections.Generic;
#if UNITY_EDITOR
#endif
using PathCreation.Examples;
//using System;
using System.Collections;
//lida//putting markers
//using Assets;
using LSL;
using System.Linq;

//lida//putting markers

//! This MonoBehavior can be attached to any object with a collider. It will apply a haptic "effect"
//! to any haptic stylus that is within the boundries of the collider.
//! The parameters can be adjusted on the fly.
public class sinHEffect : MonoBehaviour
{
    ///************************************************
    // Lida
    //public GameObject LRef_Plane;
    //public Vector3 Lnormal1;
    //public Plane Lplane1;
    /*public static float LPre_dis_ref=0.0f, Ldis_ref_top=0.0f;
    public static bool Lflag_first_var = true;
    public static bool Lflag_first = true;
    public static bool Lflag_second = true;
    public static bool Lflag_third = true;
    public static float Lfirst_var=0.0f;
    public static bool flag_peak = true;*/
    public static bool flag_concave = false, flag_convex = false, flag_flat = false, flag_flat_b = false, flag_flat_h = false, flag_bump_f = false, flag_hole_f = false;
    private Text textsys;
    private float Error_rate = 0.0f, Error_rate_s = 0.0f;
    public bool[] Flag_Markers_s;
    //lida//putting markers
    public StreamInfo streamInfo;
    public StreamOutlet outlet;
    public int[] currentSample;
    //public int[] nextcurrentSample;
    public int hapticcounter;
    //lida//putting markers
    //lida putting timer
    private float ResponsewaitTime = 2.0f;
    public int[] Session_Number,MixedRe;
    private GameObject[] myObjects;
    public BoxCollider myboxCollider;
    public bool ReliableFlag = false, UnReliableFlag = false,MixedReliableFlag = false;
    /*public int[] bump= { 0, 1 };
    public int[] hole = { 0, 1 };
    public int[] flat = { 0, 1 };*/
    public int[] bump, hole, flat;
    string feedback;
    public bool[] FlagMixRe;
    int[] trial_type;
    //public GameObject mytimer = null;
    GameObject timer_text=null;
    GameObject plane = null;
    public Text disvar;
    // Lida
    //**************************************
    public int sessionID;//lida//type of the surface
    public string subID;
    public bool isTest = false; public bool preTest = false; //public bool isnoSession12 = false; 
    //private float temXZ = 1.0f;//0.5f;
    public string out_result = ".txt"; private string out_result_mid = ".txt";
    public string marker_result = ".txt";
    public string Machine_Performance = ".txt", Machine_Performance_h = ".txt";
    public int subjectID;
    public PathFollower PathFollower;
    public log_info log_info;
    //public LSLMarkerStream marker;

    private List<int> alpha = new List<int> { 0, 1, 2, 3 };
    //private int[,] multiDimensionalArray2 = { { 90, 180, 0 }, { 340, 90, 270 }, { 0, 90, 270 }, { 20, 90, 270 } };
    private string sur_init, sur_sel, use_anw; private int cor_num; private float cor_rate;
    private bool first_show_sur1, first_show_sur2, first_show_sur3, first_show_sur4;
    private bool first_show_end1, first_show_end2, first_show_end3, first_show_end4;
    public string eeg_text, click_text;
    public System.DateTime start_ref_time, start_each_path_time;
    System.DateTime cur_time, clikcur_time, first_sur_time; System.TimeSpan dur_cur_time, dur_clickcur_time, dur_ref_time, descion_sur_time, dur_ref_time1;
    //private enum HLTOUCH_MODEL { HL_CONTACT, HL_CONSTRAINT };
    //private HLTOUCH_MODEL hlTouchModel = HLTOUCH_MODEL.HL_CONTACT;  //!< HL_CONTACT is a normal object, HL_CONSTRAINT will force the stylus to stick to the surface of the mesh.
    //private enum HLFACING { HL_FRONT, HL_BACK, HL_FRONT_AND_BACK };
    //private HLFACING hlTouchable = HLFACING.HL_FRONT; //!< Which surface will be touchable? Front, Back, or Both?
    //private float snapDistance = 5.0f; //!< When in HL_CONTRAINT mode, the maximum distance the stylus will "snap" to the surface.
    //private bool Flip_Normals = false;  //!< There isn't areally a reason to use this. 
    //private float[] threshod_value = { 0.073f, 0.065f, 0.057f, 0.049f, 0.041f, 0.033f, 0.025f, 0.017f, 0.009f, 0.001f, 0.005f};//{0.09f, 0.06f, 0.01f, 0.04f, 0.02f, 0.08f, 0.07f, 0.03f, 0.10f, 0.05f };
    //private float[] threshod_value = { 0.073f, 0.073f, 0.065f, 0.065f, 0.057f, 0.057f, 0.049f, 0.049f, 0.041f, 0.041f, 0.033f, 0.033f, 0.025f, 0.025f, 0.005f };//{0.09f, 0.06f, 0.01f, 0.04f, 0.02f, 0.08f, 0.07f, 0.03f, 0.10f, 0.05f };
    //Lida//one_height
    /*private float[] threshod_value = {         1.000f, 1.000f, 1.000f, +//0.073f, +
                                               0.065f, 0.065f, 0.065f, +//0.065f, +
                                               0.057f, 0.057f, 0.057f, +//0.057f, +
                                               0.049f, 0.049f, 0.049f, +//0.049f, +
                                               0.041f, 0.041f, 0.041f, +//0.041f, +
                                               0.033f, 0.033f, 0.033f, +//0.033f, +
                                               1.000f, 1.000f, 1.000f, +//0.025f, + 
                                      0.005f};*/
    //Lida//one_height
    private float threshod_value_init_up;//0.001f; 
    private float threshod_value_init_do;// = 0.073f; 
    private float threshod_value_curr;
    private float responsetimer = 0.0f; private float showtimer_dur = 2.0f, showtimerS_dur = 5.0f, showtimer = 0.0f; private float waitTime; private float waitTimeA; private float scrollBar = 1.0f; private bool overdely = false;
    private float waittimer = 0.0f;
    private float[] ans_value = { 1, 0, 0, 1, 0, 1, 1, 1, 0, 0 };
    public int sinhorizontal_number; public int sinvertical_number;
    public bool clickflg;
    //private int[] squ_index = { 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 1, 0, 1};
    private int[] squ_index = { 0, 1, 2, 3, 1, 3, 2, 0, 2, 1, 0, 3, 1, 0, 3, 2, 2, 3, 0, 1, 3, 2, 0, 1, 3, 0, 1, 2, 0 };
    private int[] squ_index_training = { 1, 3, 2, 0, 1, 0, 1, 2, 3, 1, 3, 2, 0, 2, 1, 0, 3, 1, 0, 3, 2, 2, 3, 0, 3, 0, 1, 2, 0 };
    private int[,] squ_index_angle = { { 0, 90, 270 }, { 90, 180, 0 }, { 20, 90, 270 }, { 340, 90, 270 } };
    private string[] out_threshod_value = new string[60];
    private string[] out_squ_index = new string[60];
    private string[] out_corr_index = new string[60];
    private string[] out_corr_rate = new string[60];
    //public string session_flg = "FF";
    //private bool session_flg_1 = false;
    //private bool session_flg_2 = false;
    //private bool yes_flag = false;
    //private int current_session = 0;
    private float x_d, y_d, z_d;
    public enum EFFECT_TYPE { CONSTANT };// , VISCOUS, SPRING, FRICTION, VIBRATE };//lida//it was constant before I did it like this
    public EFFECT_TYPE effectType = EFFECT_TYPE.CONSTANT;
    private GameObject ans;


    public int Total_trial;//lida//defines the # of trials for each block
    //public int Bump_trial;
    //public int hole_trial;
    //public int flat_trial;
    //public int Nobump_trial;
    //public int Nohole_trial;
    public int i_trial1 = 0;
    public bool is_touching = false;
    GameObject welcome = null; //GameObject plane = null; GameObject cover = null;
    //GameObject light = null; //GameObject light34 = null;
    GameObject user_instruction = null;
    public Button trust_yes = null;
    public Button trust_no = null;
    //GameObject session1 = null;
    //GameObject session2 = null;
    GameObject subId = null;
    GameObject subText = null;
    //GameObject tryagainBu = null;
    public GameObject nextBu = null;
    //GameObject confidenttext = null;
    //GameObject confidenttextNo = null;
    //GameObject confidenttextYes = null;
    //GameObject slider_yes = null;
    //GameObject slider_yesText = null; public float slider_yesTextNo = 0;
    //GameObject ground = null;
    //GameObject slider_no = null;
    //GameObject slider_noText = null; public float slider_noTextNo = 0;
    //GameObject message = null;
    GameObject cone = null;
    GameObject startbutton = null;
    public GameObject YesBu = null;
    GameObject NoBu = null; GameObject FlatBu = null; GameObject NotsureBu = null;
    //GameObject IsFlat = null; GameObject IsNotFlat = null;
    GameObject visual_sur = null;
    //GameObject visual_sur_hole = null;
    //GameObject visual_sur_flat = null;
    //GameObject visual_sur_34 = null; private bool visual_sur_flag;//GameObject visual_sur_A2 = null; 
    //GameObject doneText_session1 = null; //0
    //GameObject doneText_session2 = null; //45
    //GameObject doneText_session3 = null; //90
    //GameObject doneText_session4 = null; // 2-D 0
    //GameObject doneText_session5 = null; // 2-D 45
    public GameObject doneText_session_all = null;
    //lida//trust scene
    //GameObject Button trust_yes, trust_no;
    GameObject trust_text, system_res;
    //lida//trust scene
    //public Text forceType;
    //[DisplayOnlyAttribute] private float stylusX = 0; //public Text stylusX; 
    //[DisplayOnlyAttribute] private float stylusY = 0; //public Text stylusY; 
    //[DisplayOnlyAttribute] private float stylusZ = 0; //public Text stylusZ; 
    private float stylusXX;
    private float stylusYY;
    private float stylusZZ;
    public Vector3 currentRotation;

    public int finishedpathFlag = 0;
    // Public, User-Adjustable Settings
    //public EFFECT_TYPE effectType = EFFECT_TYPE.VISCOUS; //!< Which type of effect occurs within this zone?

    public double DisPtoC; private Text DisPtoCText;
    public double Dis_threshold = 0.8f;
    [Range(0.0f, 1.0f)] public double Magnitude = 0.6f;//lida it was before 0.45f;
    [Range(0.0f, 1.0f)] private double Gain = 0.333f;
    [Range(1.0f, 1000.0f)] private double Frequency = 200.0f;
    [Range(0.0f, 1.0f)] public float hlStiffness = 0.7f;    //!< Higher values are less 'rubbery'.
    [Range(0.0f, 1.0f)] public float hlDamping = 0.1f;      //!< Higher values are less 'bouncy'.
    [Range(0.0f, 1.0f)] public float hlStaticFriction = 0.2f;   //!< Higher values have more 'sticky' surface friction.
    [Range(0.0f, 1.0f)] public float hlDynamicFriction = 0.3f;  //!< Higher values have more 'sliding' surface friction.
    [Range(0.0f, 1.0f)] public float hlPopThrough = 0.0f;   //!< If non-zero : How much force is required to "pop" through to the inside of the mesh.
                                                            //public float snapDistance = 10.0f;

    //private double Duration = 1.0f;

    // Keep track of the Haptic Devices
    public HapticPlugin[] devices;
    bool[] inTheZone;       //Is the stylus in the effect zone?
    Vector3[] devicePoint;  // Current location of stylus
    float[] delta;          // Distance from stylus to zone collider.
    int[] FXID;             // ID of the effect.  (Per device.)

    // These are the user adjustable vectors, converted to world-space. 
    private Vector3 focusPointWorld = Vector3.zero;
    //lida//direction change
    private Vector3 directionWorld = Vector3.down;//Lida deng's one is :Vector3.down;
    //lida//direction change
    public Vector3 Direction = Vector3.down; //Lida deng's one is :Vector3.down;
    //lida//direction change
    //lida-added
    public Vector3 Position = Vector3.zero;
    public Vector3 Direction1 = Vector3.up;
    //lida-added
    public Vector3 conePosition = Vector3.zero;
    public Vector3 stylPosition = Vector3.zero;

    public int touchNumber = 0;
    public int touchNumber_path = 0;
    public int tracingNumber = 0;
    public int tracingNumber_path = 0;
    public int frameNumber = 0;
    public int frameNumber_path = 0;

    public float touchAcc;
    public float[] touchAcc_path;
    private float touchAcc_m;
    public float touchAcc_row_m;
    public float touchAcc_col_m;
    private float touchAcc_row_mm;
    private float touchAcc_col_mm;
    //public Vector2 tracingAcc_m;
    private Vector3 previous;
    public Vector3 previous_path;
    public Vector2 tracingAcc;
    public Vector2[] tracingAcc_path;
    public float tracingAcc_row_m;
    public float tracingAcc_col_m;
    private float tracingAcc_row_mm;
    private float tracingAcc_col_mm;

    public Vector2 tracingSpeed;
    public Vector2[] tracingSpeed_path;
    private Vector2 speed_path_average;
    private Vector2[] tracingSpeed_path_tem;
    private Vector2 tracingSpeed_path_temtem;
    public float tracingSpeed_row_m;
    public float tracingSpeed_col_m;
    private float tracingSpeed_row_mm;
    private float tracingSpeed_col_mm;

    private float threshod_va;
    //private string yes_no_anw;
    private float conf_rate;
    private string tryagain_flag;
    //public float identfyAmpUp;
    //public float identfyAmpDown;
    //private bool oneFinshed = false;
    static bool gunSpeedReduced = false;
    private bool is_output; //ublic bool is_up = false; public bool is_down = false;
    string docPath_mid = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
    public string docPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyComputer));
    //string docPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyComputer), "results");// .DesktopDirectory));
    //string docPath_mid = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
    public bool save_previous_state1 = false;

    System.DateTime frame_before;
    System.DateTime frame_after;
    System.DateTime all_before;
    System.DateTime all_trial_before;
    public System.DateTime each_before;
    System.DateTime decision_before;
    System.DateTime decision_after_next;
    System.DateTime time_after_dec;
    System.TimeSpan decision_duration;
    System.DateTime eeg_decision_be;
    System.DateTime eeg_decision_af;
    System.DateTime trial_after;
    System.TimeSpan trial_duration;
    System.DateTime all_trial_after;
    System.TimeSpan all_trial_duration;
    System.DateTime all_after;
    System.TimeSpan all_duration;
    //[Header("ReadOnly ForceDir")]
    //[DisplayOnlyAttribute] public Vector3 stylusPositionRaw;    //!< (Readonly) Stylus position, in device coordinates.
    //[DisplayOnlyAttribute] public float touchingDepth = 0;

    //! Start() is called at the beginning of the simulation.
    //!
    //! It will identify the Haptic devices, initizlize variables internal to this script, 
    //! and request an Effect ID from Open Haptics. (One for each device.)
    //!

    //private bool buttonNoWasClicked = false;
    // private bool buttonFlatWasClicked = false;
    //private bool buttonYesWasClicked = false;
    public void NoSelection()//NoSelection of bump/hole/flat
    {
        YesBu.SetActive(false);
        FlatBu.SetActive(false);
        NoBu.SetActive(false);
        //IsFlat.SetActive(false);
        user_instruction.SetActive(false);
        changesconetostart();
        changescale();
        AnsewshowHide();
        time_after_dec = System.DateTime.Now;//lida//reserving the time here to calculate the decision time in the next scene (yes No)
        cur_time = System.DateTime.Now;
        dur_ref_time = cur_time.Subtract(all_trial_before);//lida-start time of all blocks//cur_time.Subtract(start_ref_time);//lida//start_ref_time: we are in the first path of 4 paths when the cone start to move//used for calculating the whole spending time doing an experimnets (4 paths)

        eeg_decision_af = System.DateTime.Now;
        //decision_after = System.DateTime.Now;
        //lida//it gives wrong answer//decision_duration = decision_after.Subtract(decision_before);//lida//decision_before: it seems it is the time that all the paths are finished//but this one is not printed:decision time
        //lida//marker//here//marker.Write(81);
        currentSample[0] = 94;// "1";// "hole-B";
        //print("Noselection");
        outlet.push_sample(currentSample);
        //lida//marker
        string lline1 = "";
        string text = "NoSelection-Marker94: " + "Current Time: [ " + System.DateTime.Now.ToString("HH:mm:ss.fff")+ " ]";
        // lida /????/ "eeg_decision_be" is captured at the end of each path.so is this correct that we subtract current time from that time? it means that this time is the decision time
        //text = text + "]   Decision Time:  [" + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + "]  Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
        //text = text + "]   Decision Time (after next):  [ " + cur_time.Subtract(decision_after_next).Minutes + ":" + cur_time.Subtract(decision_after_next).Seconds + ":" + cur_time.Subtract(decision_after_next).Milliseconds + "]   Decision Time (before next):  [" + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + "]  Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
        //string lline22= "Decision Time: [ " + cur_time.Subtract(decision_after_next).Minutes + ":" + cur_time.Subtract(decision_after_next).Seconds + ":" + cur_time.Subtract(decision_after_next).Milliseconds + " ] Decision Time(before next):  [ " + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + " ]" ;
        string lline22 = "Decision Time: [ " + cur_time.Subtract(decision_after_next).Seconds + "." + cur_time.Subtract(decision_after_next).Milliseconds + " ]";
        string line11 = "Decision Time(before next): [ " + eeg_decision_af.Subtract(eeg_decision_be).Seconds + "." + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + " ]";
        string lline33 = "Total Time: [ " + dur_ref_time.TotalSeconds.ToString("0.000") + " ]";
        Error_rate++;
        if (flag_concave)
            lline1 = "No Selection. It was a bump.";//lida//you can define the true-positive, TN, FP, FN here
        else if (flag_convex)
            lline1 = "No Selection. It was a hole";
       else if(flag_flat)
            lline1 = "No Selection. It was a falt";
        else if (flag_flat_b)
            lline1 = "No Selection. It was a illusionBump";
        else if (flag_flat_h)
            lline1 = "No Selection. It was a illusionHole";
        else if (flag_bump_f)
            lline1 = "No Selection. It was a illusionBFlat";
        else if (flag_hole_f)
            lline1 = "No Selection. It was a illusionHFlat";
        /*if (ReliableFlag)
        {
            ReliableResponse();
        }
        else if (UnReliableFlag)
        {
            UnReliableResponse();
        }*///if (unrelaible)
         /*else if(MixedReliableFlag)
         {
             MixedResponse();
         }*/
        //nextcurrentSample[0] = currentSample[0];
        trust_yes.GetComponent<Image>().color = Color.white;
        trust_no.GetComponent<Image>().color = Color.white;
        overdely = false;
        trust_yes.gameObject.SetActive(true);
        trust_no.gameObject.SetActive(true);
        trust_text.SetActive(true);
        system_res.SetActive(true);
        //lida//trust scene
        string lline3 = "=============================================================================";
        //int ii = i_trial1 + 1;
        //string lines1 = "Trial: " + i_trial1.ToString("00");
        string[] lines = { text, lline22, line11, lline33,lline1, feedback};//, lines1, lline3 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);

    }
    //lida//having three buttons
    public void showconfidence_No()//Hole
    {
        ///////////////////Lida
        //print("showconfidence_Hole()");
        YesBu.SetActive(false);
        FlatBu.SetActive(false);
        NoBu.SetActive(false);
        //IsFlat.SetActive(false);
        user_instruction.SetActive(false);
        changesconetostart();
        changescale();
        AnsewshowHide();
        //string lline2 = "";
        //cone.SetActive(true);
        //lida//my sessions
        /*session1.interactable = false; session2.interactable = false; cone.SetActive(false);
        user_instruction.SetActive(true); session1.gameObject.SetActive(true); session2.gameObject.SetActive(true);
        doneText_session_all.SetActive(true);
        *///lida//my sessions
        //////////////////////lida
        time_after_dec = System.DateTime.Now;//lida//reserving the time here to calculate the decision time in the next scene (yes No)
        cur_time = System.DateTime.Now;
        dur_ref_time = cur_time.Subtract(all_trial_before);//lida-start time of all blocks//cur_time.Subtract(start_ref_time);//lida//start_ref_time: we are in the first path of 4 paths when the cone start to move//used for calculating the whole spending time doing an experimnets (4 paths)

        eeg_decision_af = System.DateTime.Now;
        //decision_after = System.DateTime.Now;
        //lida//it gives wrong answer//decision_duration = decision_after.Subtract(decision_before);//lida//decision_before: it seems it is the time that all the paths are finished//but this one is not printed:decision time
        //lida//marker//here//marker.Write(81);
        currentSample[0] = 92;// "1";// "hole-B";
        //print("Hole");
        outlet.push_sample(currentSample);
        //lida//marker
        string lline1 = "";
        string text = "PressedHole-Marker92: " + "Current Time: [ " + System.DateTime.Now.ToString("HH:mm:ss.fff")+" ]";
        //lida/????/"eeg_decision_be" is captured at the end of each path. so is this correct that we subtract current time from that time?it means that this time is the decision time
        //text = text + "]   Decision Time:  [" + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + "]  Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
        //text = text + "]   Decision Time (after next):  [ " + cur_time.Subtract(decision_after_next).Minutes + ":" + cur_time.Subtract(decision_after_next).Seconds + ":" + cur_time.Subtract(decision_after_next).Milliseconds + "]   Decision Time (before next):  [" + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + "]  Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
        //string lline22= "Decision Time: [ " + cur_time.Subtract(decision_after_next).Minutes + ":" + cur_time.Subtract(decision_after_next).Seconds + ":" + cur_time.Subtract(decision_after_next).Milliseconds + " ] Decision Time(before next):  [ " + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + " ]" ;
        string lline22 = "Decision Time: [ " + cur_time.Subtract(decision_after_next).Seconds + "." + cur_time.Subtract(decision_after_next).Milliseconds + " ]";
        string line11= "Decision Time(before next): [ "  + eeg_decision_af.Subtract(eeg_decision_be).Seconds + "." + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + " ]"; 
        string lline33= "Total Time: [ " + dur_ref_time.TotalSeconds.ToString("0.000") + " ]";
        //confidenttext.SetActive(true); confidenttextNo.SetActive(true); confidenttextYes.SetActive(false);
        //slider_yes.SetActive(false); slider_yesText.SetActive(false); slider_no.SetActive(true); slider_noText.SetActive(true);
        //nextBu.SetActive(true); //tryagainBu.SetActive(true); 
        //NoBu.GetComponent<Image>().color = Color.cyan; buttonNotsureWasClicked = false; buttonNoWasClicked = true; buttonYesWasClicked = false;
        //lida//me sessions-delete
        //NoBu.GetComponent<Button>().interactable = false; NoBu.GetComponent<Image>().color = Color.yellow;
        //lida//me sessions-delete
        //buttonYesWasClicked = false; buttonNoWasClicked = true; buttonFlatWasClicked = false;  //Lida-Jing's//
        //lida//calculating the error rate of each participant
        /*if (flag_flat)
        {
            //print("wrong-showconfidence_No()");
            lline1 = "Wrong Answer. It is a Flat surface.";
            //line1 = "The TOTAL used trials are: " + Total_trial.ToString("00");
            //lida//you can define the true-positive, TN, FP, FN here

        }
        else
            lline1 = "Right Answer. It is a Non-Flat surface.";//lida//you can define the true-positive, TN, FP, FN here
                                                               //lida//end//calculating the error rate of each participant


        string[] lines = { text, lline1 };//lida//this part is printed at the end of the .txt file
        //string[] lines = { lines1, lines2, lines3, lines4, lines5, lines6, lines7, lines8, lines9, lines11 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);
        */
        if (flag_convex)//hole
            lline1 = "Right Answer. It was a hole.";//lida//you can define the true-positive, TN, FP, FN here
        else if (flag_flat_h)
            lline1 = "Right Answer. It was a illusionHole";
        else //if (flag_concave || flag_convex)
        {
            Error_rate++;
            //print("wrong-showconfidence_Yes()");
            if (flag_concave)
                lline1 = "Wrong Answer. It was a bump";
            else if(flag_flat)
                lline1 = "Wrong Answer. It was a falt";
            else if (flag_flat_b)
                lline1 = "Wrong Answer. It was a illusionBump";
            else if (flag_bump_f)
                lline1 = "Wrong Answer. It was a illusionBFlat";
            else if (flag_hole_f)
                lline1 = "Wrong Answer. It was a illusionHFlat";
            //lida//you can define the true-positive, TN, FP, FN here
        }
        //if (i_trial1 >= Total_trial)
        //{
        // doneText_session_all.SetActive(true);
        /*Error_rate = Error_rate / Total_trial;
        //lline2 = "Error_rate: " + Error_rate.ToString("0.00");
        //string lline3 = "=============================================================================";
        // string[] lines = {  text, lline1, lline2, lline3 };
         File.AppendAllLines(Path.Combine(docPath, out_result), lines);*/
        //Finalexport();
        //}
        //else
        //{
        //Lida//changing the system response
        /*if (ReliableFlag)
        {
            ReliableResponse();
        }
        else if (UnReliableFlag)
        {
            UnReliableResponse();
        }*///if (unrelaible)
         /*else if(MixedReliableFlag)
         {
             MixedResponse();
         }*/
         //Lida//changing the system response
         //lida//trust scene
        //nextcurrentSample[0] = currentSample[0];
        trust_yes.GetComponent<Image>().color = Color.white;
        trust_no.GetComponent<Image>().color = Color.white;
        overdely = false;
        trust_yes.gameObject.SetActive(true);
        trust_no.gameObject.SetActive(true);
        trust_text.SetActive(true);
        system_res.SetActive(true);
        //lida//trust scene
        string lline3 = "=============================================================================";
        //int ii = i_trial1 + 1;
        //string lines1 = "Trial: " + i_trial1.ToString("00");
        string[] lines = { text, lline22, line11, lline33, lline1, feedback };//, lines1, lline3 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);



        //}
        /*
        if (i_trial1 < Total_trial)
        {
            int ii = i_trial1 + 1;
            string llines1 = "Trial: " + ii.ToString("00");
            lines = { lline3, llines1, lline3 };
            File.AppendAllLines(Path.Combine(docPath, out_result), lines);
        }*/
    }
    public void showconfidence_Yes()//bump
    {
        ///////////////////Lida
        //print("showconfidence_Bump");
        YesBu.SetActive(false);
        FlatBu.SetActive(false);
        NoBu.SetActive(false);
        //IsFlat.SetActive(false);
        user_instruction.SetActive(false);
        changesconetostart();
        changescale();
        AnsewshowHide();
        //string lline2 = "";
        //cone.SetActive(true);
        //lida//my sessions
        /*session1.interactable = false; session2.interactable = false; cone.SetActive(false);
        user_instruction.SetActive(true); session1.gameObject.SetActive(true); session2.gameObject.SetActive(true);
        doneText_session_all.SetActive(true);*/
        //lida//my sessions
        //////////////////////lida
        //lida//marker//here//marker.Write(83);
        time_after_dec = System.DateTime.Now;//lida//reserving the time here to calculate the decision time in the next scene (yes No)
        currentSample[0] = 91;// "1";// "bump-B";
        //print("Bump");
        outlet.push_sample(currentSample);
        //lida//marker
        eeg_decision_af = System.DateTime.Now;
        //decision_after = System.DateTime.Now;
        //lida//it gives wrong answer//decision_duration = decision_after.Subtract(decision_before);//lida// this time is the decision time
        cur_time = System.DateTime.Now;
        dur_ref_time = cur_time.Subtract(all_trial_before);//lida-start time of all blocks//cur_time.Subtract(start_ref_time);//lida//start_ref_time: we are in the first path of 4 paths paths when the cone start to move//used for calculating the whole spending time doing an experimnets (4 paths)
                                                           //lida// I made it comment//descion_sur_time = cur_time.Subtract(first_sur_time);//lida//prints  first_sur_time:0001-01-01 12:00:00 AM

        //print("first_sur_time" + first_sur_time);
        //lida//I made it comment as I do not know what is for
        /*
        string text2 = "Selected sur: " + sur_sel;
        if (sur_init == sur_sel)
        {
            cor_num = cor_num + 1;
            text2 = text2 + "(Y)";
            out_corr_index[i_trial1] = "Y" + i_trial1.ToString() + "-";
        }
        else
        {
            text2 = text2 + "(N)";
            out_corr_index[i_trial1] = "N" + i_trial1.ToString() + "-";
        }
        use_anw = text2;
        cor_rate = 100 * cor_num / (i_trial1 + 1.0f);
        out_corr_rate[i_trial1] = cor_rate.ToString("0.00") + "%-";
        *///lida//I made it comment as I do not know what is for
          //Deng//Wrong//string text = ":83" + " @ " + cur_time.ToString("HH:mm:ss.fff") + "[Decision time: " + descion_sur_time.TotalSeconds.ToString("0.000") + "][" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
          //lida//in Deng's code it prints the upper line which is not so I wrote the following code for printing the decision time
        string text = "PressedBump-Marker91: " + "Current Time: [ " + System.DateTime.Now.ToString("HH:mm:ss.fff")+" ]";
        //text = text + "]   Decision Time (after next):  [" + cur_time.Subtract(decision_after_next).Minutes + ":" + cur_time.Subtract(decision_after_next).Seconds + ":" + cur_time.Subtract(decision_after_next).Milliseconds + "]   Decision Time (before next):  [" + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + "]  Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
        //string lline22 = "Decision Time: [ " + cur_time.Subtract(decision_after_next).Minutes + ":" + cur_time.Subtract(decision_after_next).Seconds + ":" + cur_time.Subtract(decision_after_next).Milliseconds + " ] Decision Time(before next):  [ " + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + " ]";
        string lline22 = "Decision Time: [ " + cur_time.Subtract(decision_after_next).Seconds + "." + cur_time.Subtract(decision_after_next).Milliseconds + " ]";
        string line11 = "Decision Time(before next): [ " + eeg_decision_af.Subtract(eeg_decision_be).Seconds + "." + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + " ]";
        string lline33 = "Total Time: [ " + dur_ref_time.TotalSeconds.ToString("0.000") + " ]";
        //text = text + "]   Decision Time:  [" + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + "]  Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
        //string text = ":83" + " @ " + cur_time.ToString("HH:mm:ss.fff") + "[Decision time: " + decision_duration.TotalSeconds.ToString("0.000") + "][" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
        //confidenttext.SetActive(true); confidenttextNo.SetActive(false); confidenttextYes.SetActive(true);
        //slider_no.SetActive(false); slider_noText.SetActive(false); slider_yes.SetActive(true); slider_yesText.SetActive(true);
        //nextBu.SetActive(true); //tryagainBu.SetActive(true); 
        //lida//me sessions-delete
        //YesBu.GetComponent<Button>().interactable = false; YesBu.GetComponent<Image>().color = Color.yellow;
        //lida//me sessions-delete
        //buttonYesWasClicked = true; buttonNoWasClicked = false; buttonFlatWasClicked = false;  //Lida-Jing's//
        //lida//calculating the error rate of each participant
        string lline1 = "";
        if (flag_concave)//bump
            lline1 = "Right Answer. It was a bump.";//lida//you can define the true-positive, TN, FP, FN here
        else if (flag_flat_b)
            lline1 = "Right Answer. It was a illusionBump";
        else //if (flag_concave || flag_convex)
        {
            Error_rate++;
            //print("wrong-showconfidence_Yes()");
            if (flag_convex)
                lline1 = "Wrong Answer. It was a hole";
            else if (flag_flat)
                lline1 = "Wrong Answer. It was a falt";
            else if (flag_flat_h)
                lline1 = "Wrong Answer. It was a illusionHole";
            else if (flag_bump_f)
                lline1 = "Wrong Answer. It was a illusionBFlat";
            else if (flag_hole_f)
                lline1 = "Wrong Answer. It was a illusionHFlat";
            //lida//you can define the true-positive, TN, FP, FN here
        }
        //if (i_trial1 >= Total_trial)
        // {
        // doneText_session_all.SetActive(true);
        /*Error_rate = Error_rate / Total_trial;
        lline2 = "Error_rate: " + Error_rate.ToString("0.00");
        string lline3 = "=============================================================================";
        string[] lines = {  text, lline1, lline2,lline3 };//string[] lines = { text2 + text, lline1, lline2 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);*/
        //Finalexport();
        // }
        //else
        //{
        
        //Lida//changing the system response
        /*if (ReliableFlag)
        {
            ReliableResponse();
        }
        else if (UnReliableFlag)
        {
            UnReliableResponse();
        }*///if (unrelaible)
         /*else if (MixedReliableFlag)
         {
             MixedResponse();
         }*/
         //Lida//changing the system response
         //lida//trust scene
        //nextcurrentSample[0] = currentSample[0];
        trust_yes.GetComponent<Image>().color = Color.white;
        trust_no.GetComponent<Image>().color = Color.white;
        overdely = false;
        trust_yes.gameObject.SetActive(true);
        trust_no.gameObject.SetActive(true);
        trust_text.SetActive(true);
        system_res.SetActive(true);
        //lida//trust scene
        string lline3 = "=============================================================================";
        //int ii = i_trial1 + 1;
        // string lines1 = "Trial: " + i_trial1.ToString("00");
        string[] lines = { text,lline22,line11,lline33, lline1, feedback};//, lines1, lline3 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);
    }
    public void showconfidence_Flat()//non-H
    {
        ///////////////////Lida
        //print("showconfidence_Flat()");
        YesBu.SetActive(false);
        FlatBu.SetActive(false);
        NoBu.SetActive(false);
        //IsFlat.SetActive(false);
        user_instruction.SetActive(false);
        //string lline2 = "";
        changesconetostart();
        changescale();
        AnsewshowHide();
        //cone.SetActive(true);
        //lida//my sessions
        /*session1.interactable = false; session2.interactable = false; cone.SetActive(false);
        user_instruction.SetActive(true); session1.gameObject.SetActive(true); session2.gameObject.SetActive(true);
        doneText_session_all.SetActive(true);
        *///lida//my sessions
        //////////////////////lida
        time_after_dec = System.DateTime.Now;//lida//reserving the time here to calculate the decision time in the next scene (yes No)
        cur_time = System.DateTime.Now;
        dur_ref_time = cur_time.Subtract(all_trial_before);//lida-start time of all blocks//cur_time.Subtract(start_ref_time);//lida//start_ref_time: we are in the first path of 4 paths paths when the cone start to move//used for calculating the whole spending time doing an experimnets (4 paths)

        eeg_decision_af = System.DateTime.Now;
        //decision_after = System.DateTime.Now;
        //lida//it gives wrong answer//decision_duration = decision_after.Subtract(decision_before);//lida//decision_before: it seems it is the time that all the paths are finished//but this one is not printed:decision time
        //lida//marker//here//marker.Write(84);
        currentSample[0] = 93;// "1";// "flat-B";
        //print("Flat");
        outlet.push_sample(currentSample);
        //lida//marker
        string lline1 = "";
        string text = "PressedFlat-Marker93: " + "Current Time: [ " + System.DateTime.Now.ToString("HH:mm:ss.fff")+" ]";
        //lida/????/"eeg_decision_be" is captured at the end of each path. so is this correct that we subtract current time from that time?it means that this time is the decision time
        //text = text + "]   Decision Time (after next):  [" + cur_time.Subtract(decision_after_next).Minutes + ":" + cur_time.Subtract(decision_after_next).Seconds + ":" + cur_time.Subtract(decision_after_next).Milliseconds + "]   Decision Time (before next):  [" + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + "]  Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
        //string lline22 = "Decision Time: [ " + cur_time.Subtract(decision_after_next).Minutes + ":" + cur_time.Subtract(decision_after_next).Seconds + ":" + cur_time.Subtract(decision_after_next).Milliseconds + " ] Decision Time(before next):  [ " + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + " ]";
        string lline22 = "Decision Time: [ " + cur_time.Subtract(decision_after_next).Seconds + "." + cur_time.Subtract(decision_after_next).Milliseconds + " ]";
        string line11 = "Decision Time(before next): [ " + eeg_decision_af.Subtract(eeg_decision_be).Seconds + "." + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + " ]";
        string lline33 = "Total Time: [ " + dur_ref_time.TotalSeconds.ToString("0.000") + " ]";
        //confidenttext.SetActive(true); confidenttextNo.SetActive(true); confidenttextYes.SetActive(false);
        //slider_yes.SetActive(false); slider_yesText.SetActive(false); slider_no.SetActive(true); slider_noText.SetActive(true);
        //nextBu.SetActive(true); //tryagainBu.SetActive(true); 
        //NoBu.GetComponent<Image>().color = Color.cyan; buttonNotsureWasClicked = false; buttonNoWasClicked = true; buttonYesWasClicked = false;
        //lida//me sessions-delete
        //NoBu.GetComponent<Button>().interactable = false; NoBu.GetComponent<Image>().color = Color.yellow;
        //lida//me sessions-delete
        //buttonYesWasClicked = false; buttonNoWasClicked = false; buttonFlatWasClicked = true;  //Lida-Jing's//
        //lida//calculating the error rate of each participant
        /*if (flag_flat)
        {
            //print("wrong-showconfidence_No()");
            lline1 = "Wrong Answer. It is a Flat surface.";
            //line1 = "The TOTAL used trials are: " + Total_trial.ToString("00");
            //lida//you can define the true-positive, TN, FP, FN here

        }
        else
            lline1 = "Right Answer. It is a Non-Flat surface.";//lida//you can define the true-positive, TN, FP, FN here
                                                               //lida//end//calculating the error rate of each participant


        string[] lines = { text, lline1 };//lida//this part is printed at the end of the .txt file
        //string[] lines = { lines1, lines2, lines3, lines4, lines5, lines6, lines7, lines8, lines9, lines11 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);
        */
        if (flag_flat)
            lline1 = "Right Answer. It was a flat.";//lida//you can define the true-positive, TN, FP, FN hereif (flag_flat_b)
        else if (flag_bump_f)
            lline1 = "Right Answer. It was a illusionBFlat";
        else if (flag_hole_f)
            lline1 = "Right Answer. It was a illusionHFlat";
        else //if (flag_concave || flag_convex)
        {
            Error_rate++;
            //print("wrong-showconfidence_Yes()");
            if (flag_concave)
                lline1 = "Wrong Answer. It was a bump";
            else if(flag_convex)
                lline1 = "Wrong Answer. It was a hole";
            else if (flag_flat_b)
                lline1 = "Wrong Answer. It was a illusionBump";
            else if (flag_flat_h)
                lline1 = "Wrong Answer. It was a illusionHole";
            //lida//you can define the true-positive, TN, FP, FN here
        }
        //if (i_trial1 >= Total_trial)
        //{
        //  doneText_session_all.SetActive(true);
        /*Error_rate = Error_rate / Total_trial;
        lline2 = "Error_rate: " + Error_rate.ToString("0.00");
        string lline3 = "=============================================================================";
        string[] lines = { text, lline1, lline2, lline3 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);*/
        //Finalexport();
        // }
        //else
        //{
        //Lida//changing the system response
        /*if (ReliableFlag)
        {
            ReliableResponse();
            
        }
        else if(UnReliableFlag)
        {
            UnReliableResponse();
        }*///if (unrelaible)
         /*else if (MixedReliableFlag)
         {
             MixedResponse();
         }*/
         //Lida//changing the system response
         //lida//trust scenetrust_yes.gameObject.SetActive(true);
        //nextcurrentSample[0] = currentSample[0];
        trust_yes.GetComponent<Image>().color = Color.white;
        trust_no.GetComponent<Image>().color = Color.white;
        overdely = false;
        trust_no.gameObject.SetActive(true);
        trust_yes.gameObject.SetActive(true);
        trust_text.SetActive(true);
        system_res.SetActive(true);
        //lida//trust scene
        string lline3 = "=============================================================================";
        //int ii = i_trial1 + 1;
        //string lines1 = "Trial: " + i_trial1.ToString("00");
        string[] lines = { text, lline22, line11, lline33, lline1, feedback };//, lines1, lline3 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);


        //}
    }
    void ReliableResponse()//Creating ReliableResponse
    {
        textsys = system_res.GetComponent<Text>();
        if (flag_concave)//bump
        {
            currentSample[0] = 80;
            textsys.text = "Bump??";
            feedback = "Reliable. CBump";
        }
        else if (flag_convex)//hole
        {
            currentSample[0] = 81;
            textsys.text = "Hole??";
            feedback = "Reliable. CHole";
        }
        else if (flag_flat)//flag_fla//flat
        {
            currentSample[0] = 82;
            textsys.text = "Flat??";
            feedback = "Reliable. CFlat";
        }
        else if (flag_flat_h)
        {
            currentSample[0] = 83;
            textsys.text = "Hole??";
            feedback = "Reliable. CIllusionHole";
        }
        else if (flag_flat_b)
        {
            currentSample[0] = 84;
            textsys.text = "Bump??";
            feedback = "Reliable. CIllusionBump";
        }
        else if (flag_bump_f)
        {
            currentSample[0] = 85;
            textsys.text = "Flat??";
            feedback = "Reliable. CIllusionBFlat";
        }
        else if (flag_hole_f)
        {
            currentSample[0] = 86;
            textsys.text = "Flat??";
            feedback = "Reliable. CIllusionHFlat";
        }

    }
  
    void UnReliableResponse()
    {
        textsys = system_res.GetComponent<Text>();
        if (MixedRe[i_trial1 - 1] == 1)//relaible 
        {
            if (flag_concave)//bump
            {
                currentSample[0] = 70;
                textsys.text = "Bump??";
                feedback = "UnReliable. CBump";//I am in unrelaible session of mixedsurfaces but the feedback that I gave is correct.
            }
            else if (flag_convex)//hole
            {
                currentSample[0] = 71;
                textsys.text = "Hole??";
                feedback = "UnReliable. CHole";
            }
            else if (flag_flat)//flag_fla//flat
            {
                currentSample[0] = 72;
                textsys.text = "Flat??";
                feedback = "UnReliable. CFlat";
            }
            else if (flag_flat_h)
            {
                currentSample[0] = 73;
                textsys.text = "Hole??";
                feedback = "UnReliable. CIllusionHole";
            }
            else if (flag_flat_b)
            {
                currentSample[0] = 74;
                textsys.text = "Bump??";
                feedback = "UnReliable. CIllusionBump";
            }
            else if (flag_bump_f)
            {
                currentSample[0] = 75;
                textsys.text = "Flat??";
                feedback = "UnReliable. CIllusionBFlat";
            }
            else if (flag_hole_f)
            {
                currentSample[0] = 76;
                textsys.text = "Flat??";
                feedback = "UnReliable. CIllusionHFlat";
            }
        }
        else//unrelaible
        {
            //Random rnd = new Random();
            //int gh = Random.Range(1, 3);//random chosing it will be a correct answer or wrong
            Random rnd = new Random();
            int gh = Random.Range(1, 3);//random chosing it will be a correct answer or wrong
            if (flag_concave)//bump
            {
                if (gh == 1)
                {
                    currentSample[0] = 20;
                    textsys.text = "Flat??";
                    feedback = "UnReliable. Flat";
                }
                else
                {
                    currentSample[0] = 21;
                    textsys.text = "Hole??";
                    feedback = "UnReliable. Hole";
                }
            }
            else if (flag_convex)//hole
            {
                if (gh == 1)
                {
                    currentSample[0] = 30;
                    textsys.text = "Flat??";
                    feedback = "UnReliable. Flat";
                }
                else
                {
                    currentSample[0] = 31;
                    textsys.text = "Bump??";
                    feedback = "UnReliable. Bump";
                }
            }
            else if (flag_flat)//flag_fla//flat
            {
                if (gh == 1)
                {
                    currentSample[0] = 40;
                    textsys.text = "Bump??";
                    feedback = "UnReliable. Bump";
                }
                else
                {
                    currentSample[0] = 41;
                    textsys.text = "Hole??";
                    feedback = "UnReliable. Hole";
                }
            }
            else if (flag_flat_h)
            {
                currentSample[0] = 50;
                textsys.text = "Flat??";
                feedback = "UnReliable. Flat";
            }
            else if (flag_flat_b)
            {
                currentSample[0] = 60;
                textsys.text = "Flat??";
                feedback = "UnReliable. Flat";
            }
            else if (flag_bump_f)
            {
                currentSample[0] = 61;
                textsys.text = "Bump??";
                feedback = "UnReliable. Bump";
            }
            else if (flag_hole_f)
            {
                currentSample[0] = 62;
                textsys.text = "Hole??";
                feedback = "UnReliable. Hole";
            }
        }
    }
    /*void MixedResponse()
    {
        textsys = system_res.GetComponent<Text>();
        if (MixedRe[i_trial1-1] == 1)//relaible 
        {
            if (flag_concave)//bump
            {
                textsys.text = "Bump??";
                feedback = "MReliable. CBump";//I am in unrelaible session of mixedsurfaces but the feedback that I gave is correct.
            }
            else if (flag_convex)//hole
            {
                textsys.text = "Hole??";
                feedback = "MReliable. CHole";
            }
            else if (flag_flat)//flag_fla//flat
            {
                textsys.text = "Flat??";
                feedback = "MReliable. CFlat";
            }
            else if (flag_flat_h)
            {
                textsys.text = "Hole??";
                feedback = "MReliable. CIllusionHole";
            }
            else if (flag_flat_b)
            {
                textsys.text = "Bump??";
                feedback = "MReliable. CIllusionBump";
            }
        }
        else//unrelaible
        {
            //Random rnd = new Random();
            //int gh = Random.Range(1, 3);//random chosing it will be a correct answer or wrong
            if (flag_concave)//bump
            {
                textsys.text = "Hole??";
                feedback = "MUnReliable. Hole";
            }
            else if (flag_convex)//hole
            {
                textsys.text = "Flat??";
                feedback = "MUnReliable. Bump";
            }
            else if (flag_flat)//flag_fla//flat
            {
                textsys.text = "Hole??";
                feedback = "MUnReliable. Hole";
            }
            else if (flag_flat_h)
            {
                textsys.text = "Flat??";
                feedback = "MUnReliable. Flat";
            }
            else if (flag_flat_b)
            {
                textsys.text = "Flat??";
                feedback = "MUnReliable. Flat";
            }
        }
    }*/
    //participant pressed the yes button for the system response
    public void Pressed_Yes()
    {
        //
        trust_yes.gameObject.SetActive(false);
        trust_no.gameObject.SetActive(false);
        trust_text.SetActive(false);
        system_res.SetActive(false);
        //if (i_trial1 < Total_trial)
        //   cone.SetActive(true);
        changesconetostart();
        changescale();
        AnsewshowHide();
        ///
        //time_after_dec = System.DateTime.Now;//lida//reserving the time in yes no scene to calculate the decision time in the next scene (yes No)
        cur_time = System.DateTime.Now;
        dur_ref_time = cur_time.Subtract(all_trial_before);//lida-start time of all blocks//cur_time.Subtract(start_ref_time);//lida//start_ref_time: we are in the first path of 4 paths paths when the cone start to move//used for calculating the whole spending time doing an experimnets (4 paths)
        //lida//marker
        currentSample[0] = 400;
        //print("Pressed_Yes"+ "]   Decision Time (after selection(H/B/F)):  [" + cur_time.Subtract(time_after_dec).Minutes + ":" + cur_time.Subtract(time_after_dec).Seconds + ":" + cur_time.Subtract(time_after_dec).Milliseconds + "]   Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]");
        outlet.push_sample(currentSample);
        //lida//marker
        string text = "PressedYes-Marker400: " + "Current Time: [ " + cur_time.ToString("HH:mm:ss.fff")+" ]";
        //lida/????/"eeg_decision_be" is captured at the end of each path. so is this correct that we subtract current time from that time?it means that this time is the decision time
        //text = text + "]   Decision Time (after selection(H/B/F)):  [" + cur_time.Subtract(time_after_dec).Minutes + ":" + cur_time.Subtract(time_after_dec).Seconds + ":" + cur_time.Subtract(time_after_dec).Milliseconds + "]   Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
        //string lline22 = "Decision Time: [ " + cur_time.Subtract(time_after_dec).Minutes + ":" + cur_time.Subtract(time_after_dec).Seconds + ":" + cur_time.Subtract(time_after_dec).Milliseconds + " ]";
        string lline22 = "Decision Time: [ "  + cur_time.Subtract(time_after_dec).Seconds + "." + cur_time.Subtract(time_after_dec).Milliseconds + " ]";
        string lline33= "Total Time: [ " + dur_ref_time.TotalSeconds.ToString("0.000") + " ]";
        string line33 = "AveTotal Time: [ " + (dur_ref_time.TotalSeconds / i_trial1).ToString("0.000") + " ]";
        //print(text);
        string lline3 = "=============================================================================";

        /*if (flag_flat)
            lline1 = "Right Answer. It is a flat.";//lida//you can define the true-positive, TN, FP, FN here
        else //if (flag_concave || flag_convex)
        {
            Error_rate++;
            //print("wrong-showconfidence_Yes()");
            if (flag_concave)
                lline1 = "Wrong Answer. It is a bump";
            else
                lline1 = "Wrong Answer. It is a hole";
            //lida//you can define the true-positive, TN, FP, FN here
        }*/
        //if((feedback== "MReliable. CIllusionBump") || (feedback== "MReliable. CIllusionHole") || (feedback == "MReliable. CFlat") || (feedback == "MReliable. CHole") || (feedback == "MReliable. CBump") || (feedback ==) || )
        
        if (UnReliableFlag)// || MixedReliableFlag)
        {
            if (MixedRe[i_trial1 - 1] == 0)
                Error_rate_s++;
        }//if (unrelaible)
        /*else if (MixedReliableFlag)
        {
            if (MixedRe[i_trial1 - 1] == 0)
                Error_rate_s++;
        }*/
        string lines1 = "Trial#: " + i_trial1.ToString("00");
        double ii = Error_rate / i_trial1;
        string lline44="First ErrorRate: "+ ii.ToString("0.00");
        ii = Error_rate_s / i_trial1;
        string lline55 = "Second ErrorRate: " + ii.ToString("0.00");
        //string[] lines = { text, lline1, lline3, lines1, lline3 };
        string[] lines = { text, lline22, lline33,line33, lline44, lline55, lines1, lline3 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);
        string[] myline = { lline3, lines1 , lline3 };
        File.AppendAllLines(Path.Combine(docPath, marker_result), myline);
        //visual_sur = GameObject.Find("MixedSurface"); 
        //lida//changing the surface type
        if (i_trial1 < Total_trial)
        {
            //timer before starting each session
            //cone.SetActive(true);
            //cone.GetComponent<Renderer>().material.color = Color.black;
            //StartCoroutine(Example());
            /*timer_text.SetActive(true);
            mytimer.SetActive(true);
            float cntdnw = 1.0f;
            if ((cntdnw - Time.deltaTime) > 0)
            {
                cntdnw -= Time.deltaTime;

                double b = System.Math.Round(cntdnw, 2);
                disvar.text = b.ToString();
            }
            else//if (cntdnw < 0)
            {
                timer_text.SetActive(false);
                mytimer.SetActive(false);
                cone.SetActive(true);
                //Debug.Log("Completed");
            }*/
            //timer before starting each session
            sessionID = Session_Number[i_trial1];
            //print("sinH--pre-yes--sessionID:    " + sessionID);
            if (sessionID == 1)//bump
            {
                //visual_sur = GameObject.Find("0.3e(-x__2div2__2)");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                visual_sur.transform.position = new Vector3(50.0f, 0.0f, 13.0f); //first task: (50.0f, 0.0f, 0.5f); three big surfaces//(0.0f, 0.0f, 0.5f); //bigbumphole//(-10.0f, 0.0f, 0.5f);smallbumphole //(0.0f, 0.0f, 0.5f);three small surf //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.04f, 0.0f, 0.0015f); //(0.0f, 0.15f, 0.0f); //(10.0f, 0.0f, 0.15f); //(0.0f, 0.0f, 0.0015f); //
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.3f); //(0.1f, 0.1f, 0.003f); //
                //visual_sur_bump.transform.gameObject.tag = "Touchable";
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //Vector3 newScale = new Vector3(temXZ, temXZ, temXZ); 
                //visual_sur_bump.transform.localScale = newScale;
                flag_concave = true;
                flag_convex = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, -100f);
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_bump.SetActive(true);
                visual_sur_bump.transform.gameObject.tag = "Touchable";
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
                visual_sur_flat.transform.gameObject.tag = "Untagged";
                visual_sur_flat.SetActive(false);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);*/
                //visual_sur_flat.transform.gameObject.tag = "Untagged";
                //visual_sur_flat.SetActive(false);
            }
            else if (sessionID == 2)//hole
            {
                visual_sur.transform.position = new Vector3(25.0f, 0.0f, 13.0f); //first task: (25.0f, 0.0f, 0.4f);//(25.0f, 0.0f, 0.4f); //(0.0f, 0.0f, 0.4f); //(-10.0f, 0.0f, 0.4f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.02f, 0.0f, -0.0015f); //(-25.0f, -0.15f, 0.0f); //(0.0f, 0.0f, -0.15f); //(0.1f, 0.0f, -0.0015f); //
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.3f); //(0.1f, 0.1f, 0.003f); //
                //visual_sur = GameObject.Find("-0.3e(-x__2div2__2)");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //visual_sur_hole.transform.localEulerAngles = new Vector3(90, 180, 0);
                //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, 0.5f);
                //visual_sur_hole.transform.gameObject.tag = "Touchable";
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //Vector3 newScale = new Vector3(temXZ, -temXZ, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = true;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur_bump.transform.position = new Vector3(0.0f, 0.0f, -100f);
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_hole.transform.gameObject.tag = "Touchable";
                visual_sur_hole.SetActive(true);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
                visual_sur_flat.transform.gameObject.tag = "Untagged";
                visual_sur_flat.SetActive(false);*/
                //visual_sur_flat.transform.gameObject.tag = "Untagged";
                //visual_sur_flat.SetActive(false);
            }
            else if (sessionID == 3)//flat
            {
                visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(-20.0f, 0.0f, 0.5f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.2f, 0.0f, 0.0f);//
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(0.1f, 0.1f, 0.0001f); //
                //visual_sur = GameObject.Find("F_Surface");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //visual_sur_flat.transform.gameObject.tag = "Touchable";
                //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = false;
                flag_concave = false;
                flag_flat = true;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_flat.transform.gameObject.tag = "Touchable";
                visual_sur_flat.SetActive(true);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);*/
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
            }
            else if (sessionID == 4)//flat-bump
            {
                visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(-20.0f, 0.0f, 0.5f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.2f, 0.0f, 0.0f);//
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(0.1f, 0.1f, 0.0001f); //
                //visual_sur = GameObject.Find("F_Surface");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //visual_sur_flat.transform.gameObject.tag = "Touchable";
                //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = false;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = true;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_flat.transform.gameObject.tag = "Touchable";
                visual_sur_flat.SetActive(true);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);*/
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
            }
            else if (sessionID == 5)//flat-hole
            {
                visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(-20.0f, 0.0f, 0.5f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.2f, 0.0f, 0.0f);//
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(0.1f, 0.1f, 0.0001f); //
                //visual_sur = GameObject.Find("F_Surface");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //visual_sur_flat.transform.gameObject.tag = "Touchable";
                //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = false;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = true;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_flat.transform.gameObject.tag = "Touchable";
                visual_sur_flat.SetActive(true);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);*/
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
            }
            else if (sessionID == 6)//bump-flat
            {
                visual_sur.transform.position = new Vector3(50.0f, 0.0f, 13.0f);//first task: (50.0f, 0.0f, 0.5f);
                myboxCollider.center = new Vector3(0.04f, 0.0f, 0.0015f);
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f);
                flag_concave = false;
                flag_convex = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = true;
                flag_hole_f = false;
            }
            else if (sessionID == 7)//hole-flat
            {
                visual_sur.transform.position = new Vector3(25.0f, 0.0f, 13.0f);//first task: (25.0f, 0.0f, 0.4f);
                myboxCollider.center = new Vector3(0.02f, 0.0f, -0.0015f);
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f);
                flag_convex = false;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = true;
            }
            //lida//changing the surface type

            cone.SetActive(true);
            CameraSwitcher.FindObjectOfType<CameraSwitcher>().SwitchCamera();
            plane.SetActive(true);
            waittimer = 0.0f;
            timer_text.SetActive(true);
            //cone.GetComponent<Renderer>().material.color = Color.black;
            //StartCoroutine(Example());
        }
        else
        {
            currentSample[0] = 200;
            outlet.push_sample(currentSample);
            doneText_session_all.SetActive(true);
            Finalexport(dur_ref_time);
        }
    }
    //participant pressed the no button for the system response
    public void Pressed_No()
    {
        //
        trust_yes.gameObject.SetActive(false);
        trust_no.gameObject.SetActive(false);
        trust_text.SetActive(false);
        system_res.SetActive(false);
        //if (i_trial1 < Total_trial)
        //  cone.SetActive(true);
        changesconetostart();
        changescale();
        AnsewshowHide();
        ///
        //time_after_dec = System.DateTime.Now;//lida//reserving the time in yes no scene to calculate the decision time in the next scene (yes No)
        cur_time = System.DateTime.Now;
        dur_ref_time = cur_time.Subtract(all_trial_before);//lida-start time of all blocks//cur_time.Subtract(start_ref_time);//lida//start_ref_time: we are in the first path of 4 paths paths when the cone start to move//used for calculating the whole spending time doing an experimnets (4 paths)
        //lida//marker
        currentSample[0] = 500;
        //print("Pressed_No");
        outlet.push_sample(currentSample);
        //lida//marker
        string text = "PressedNo-Marker500: " + "Current Time: [ " + cur_time.ToString("HH:mm:ss.fff")+" ]";
        //lida/????/"eeg_decision_be" is captured at the end of each path. so is this correct that we subtract current time from that time?it means that this time is the decision time
        //text = text + "]   Decision Time (after selection(H/B/F)):  [" + cur_time.Subtract(time_after_dec).Minutes + ":" + cur_time.Subtract(time_after_dec).Seconds + ":" + cur_time.Subtract(time_after_dec).Milliseconds + "]   Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
        //string lline22 = "Decision Time: [ " + cur_time.Subtract(time_after_dec).Minutes + ":" + cur_time.Subtract(time_after_dec).Seconds + ":" + cur_time.Subtract(time_after_dec).Milliseconds + " ]";
        string lline22 = "Decision Time: [ " + cur_time.Subtract(time_after_dec).Seconds + "." + cur_time.Subtract(time_after_dec).Milliseconds + " ]";
        string lline33= "Total Time: [ " + dur_ref_time.TotalSeconds.ToString("0.000") + " ]";
        string line33 = "AveTotal Time: [ " + (dur_ref_time.TotalSeconds / i_trial1).ToString("0.000") + " ]";
        string lline3 = "=============================================================================";

        /*if (flag_flat)
            lline1 = "Right Answer. It is a flat.";//lida//you can define the true-positive, TN, FP, FN here
        else //if (flag_concave || flag_convex)
        {
            Error_rate++;
            //print("wrong-showconfidence_Yes()");
            if (flag_concave)
                lline1 = "Wrong Answer. It is a bump";
            else
                lline1 = "Wrong Answer. It is a hole";
            //lida//you can define the true-positive, TN, FP, FN here
        }*/
        if (ReliableFlag)
            Error_rate_s++;
        else if (UnReliableFlag)// || MixedReliableFlag)
        {
            if (MixedRe[i_trial1 - 1] == 1)
                Error_rate_s++;
        }//if (unrelaible)
        /*else if (MixedReliableFlag)
        {
            if (MixedRe[i_trial1 - 1] == 1)
                Error_rate_s++;
        }*/
        string lines1 = "Trial#: " + i_trial1.ToString("00");
        double ii = Error_rate / i_trial1;
        string lline44 = "First ErrorRate: " + ii.ToString("0.00");
        ii = Error_rate_s / i_trial1;
        string lline55 = "Second ErrorRate: " + ii.ToString("0.00");
        //string[] lines = { text, lline1, lline3, lines1, lline3 };
        string[] lines = { text, lline22, lline33,line33, lline44,lline55, lines1, lline3 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);
        string[] myline = { lline3, lines1, lline3 };
        File.AppendAllLines(Path.Combine(docPath, marker_result), myline);

        //lida//changing the surface type
        if (i_trial1 < Total_trial)
        {
            //timer before starting each session
           // cone.SetActive(true);
            //cone.GetComponent<Renderer>().material.color = Color.black;
            //StartCoroutine(Example());
            /*timer_text.SetActive(true);
            mytimer.SetActive(true);
            float cntdnw = 1.0f;
            if ((cntdnw - Time.deltaTime) > 0)
            {
                cntdnw -= Time.deltaTime;

                double b = System.Math.Round(cntdnw, 2);
                disvar.text = b.ToString();
            }
            else//if (cntdnw < 0)
            {
                timer_text.SetActive(false);
                mytimer.SetActive(false);
                cone.SetActive(true);
                //Debug.Log("Completed");
            }*/
            //timer before starting each session
            sessionID = Session_Number[i_trial1];
            //print("sinH--pres-no--sessionID:    " + sessionID);
            if (sessionID == 1)//bump
            {
                //visual_sur = GameObject.Find("0.3e(-x__2div2__2)");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                visual_sur.transform.position = new Vector3(50.0f, 0.0f, 13.0f); //first task: (50.0f, 0.0f, 0.4f);//three big surfaces//(0.0f, 0.0f, 0.5f); //bigbumphole//(-10.0f, 0.0f, 0.5f);smallbumphole //(0.0f, 0.0f, 0.5f);three small surf //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.04f, 0.0f, 0.0015f); //(0.0f, 0.15f, 0.0f); //(10.0f, 0.0f, 0.15f); //(0.0f, 0.0f, 0.0015f); //
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.3f); //(0.1f, 0.1f, 0.003f); //
                //visual_sur_bump.transform.gameObject.tag = "Touchable";
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //Vector3 newScale = new Vector3(temXZ, temXZ, temXZ); 
                //visual_sur_bump.transform.localScale = newScale;
                flag_concave = true;
                flag_convex = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, -100f);
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_bump.SetActive(true);
                visual_sur_bump.transform.gameObject.tag = "Touchable";
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
                visual_sur_flat.transform.gameObject.tag = "Untagged";
                visual_sur_flat.SetActive(false);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);*/
                //visual_sur_flat.transform.gameObject.tag = "Untagged";
                //visual_sur_flat.SetActive(false);
            }
            else if (sessionID == 2)//hole
            {
                visual_sur.transform.position = new Vector3(25.0f, 0.0f, 13.0f); //first task: (25.0f, 0.0f, 0.4f);//(25.0f, 0.0f, 0.4f); //(0.0f, 0.0f, 0.4f); //(-10.0f, 0.0f, 0.4f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.02f, 0.0f, -0.0015f); //(-25.0f, -0.15f, 0.0f); //(0.0f, 0.0f, -0.15f); //(0.1f, 0.0f, -0.0015f); //
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.3f); //(0.1f, 0.1f, 0.003f); //
                //visual_sur = GameObject.Find("-0.3e(-x__2div2__2)");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //visual_sur_hole.transform.localEulerAngles = new Vector3(90, 180, 0);
                //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, 0.5f);
                //visual_sur_hole.transform.gameObject.tag = "Touchable";
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //Vector3 newScale = new Vector3(temXZ, -temXZ, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = true;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur_bump.transform.position = new Vector3(0.0f, 0.0f, -100f);
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_hole.transform.gameObject.tag = "Touchable";
                visual_sur_hole.SetActive(true);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
                visual_sur_flat.transform.gameObject.tag = "Untagged";
                visual_sur_flat.SetActive(false);*/
                //visual_sur_flat.transform.gameObject.tag = "Untagged";
                //visual_sur_flat.SetActive(false);
            }
            else if (sessionID == 3)//flat
            {
                visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(-20.0f, 0.0f, 0.5f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.2f, 0.0f, 0.0f);//
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(0.1f, 0.1f, 0.0001f); //
                //visual_sur = GameObject.Find("F_Surface");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //visual_sur_flat.transform.gameObject.tag = "Touchable";
                //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = false;
                flag_concave = false;
                flag_flat = true;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_flat.transform.gameObject.tag = "Touchable";
                visual_sur_flat.SetActive(true);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);*/
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
            }
            else if (sessionID == 4)//flat-bump
            {
                visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(-20.0f, 0.0f, 0.5f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.2f, 0.0f, 0.0f);//
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(0.1f, 0.1f, 0.0001f); //
                //visual_sur = GameObject.Find("F_Surface");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //visual_sur_flat.transform.gameObject.tag = "Touchable";
                //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = false;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = true;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_flat.transform.gameObject.tag = "Touchable";
                visual_sur_flat.SetActive(true);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);*/
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
            }
            else if (sessionID == 5)//flat-hole
            {
                visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(-20.0f, 0.0f, 0.5f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.2f, 0.0f, 0.0f);//
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(0.1f, 0.1f, 0.0001f); //
                //visual_sur = GameObject.Find("F_Surface");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //visual_sur_flat.transform.gameObject.tag = "Touchable";
                //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = false;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = true;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_flat.transform.gameObject.tag = "Touchable";
                visual_sur_flat.SetActive(true);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);*/
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
            }
            else if (sessionID == 6)//bump-flat
            {
                visual_sur.transform.position = new Vector3(50.0f, 0.0f, 13.0f);//first task: (50.0f, 0.0f, 0.5f);
                myboxCollider.center = new Vector3(0.04f, 0.0f, 0.0015f);
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f);
                flag_concave = false;
                flag_convex = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = true;
                flag_hole_f = false;
            }
            else if (sessionID == 7)//hole-flat
            {
                visual_sur.transform.position = new Vector3(25.0f, 0.0f, 13.0f);//first task: (25.0f, 0.0f, 0.4f);
                myboxCollider.center = new Vector3(0.02f, 0.0f, -0.0015f);
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f);
                flag_convex = false;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = true;
            }
            //lida//changing the surface type
            cone.SetActive(true);
            CameraSwitcher.FindObjectOfType<CameraSwitcher>().SwitchCamera();
            plane.SetActive(true);
            waittimer = 0.0f;
            timer_text.SetActive(true);
            //cone.GetComponent<Renderer>().material.color = Color.black;
            //StartCoroutine(Example());
        }
        else
        {
            currentSample[0] = 200;
            outlet.push_sample(currentSample);
            doneText_session_all.SetActive(true);
            Finalexport(dur_ref_time);
        }
    }
    void NoSelectionYesNo()//no selection of yes/no
    {
        trust_yes.gameObject.SetActive(false);
        trust_no.gameObject.SetActive(false);
        trust_text.SetActive(false);
        system_res.SetActive(false);
        //if (i_trial1 < Total_trial)
        //   cone.SetActive(true);
        changesconetostart();
        changescale();
        AnsewshowHide();
        ///
        //time_after_dec = System.DateTime.Now;//lida//reserving the time in yes no scene to calculate the decision time in the next scene (yes No)
        cur_time = System.DateTime.Now;
        dur_ref_time = cur_time.Subtract(all_trial_before);//lida-start time of all blocks//cur_time.Subtract(start_ref_time);//lida//start_ref_time: we are in the first path of 4 paths paths when the cone start to move//used for calculating the whole spending time doing an experimnets (4 paths)
        //lida//marker
        currentSample[0] = 600;
        //print("Pressed_Yes"+ "]   Decision Time (after selection(H/B/F)):  [" + cur_time.Subtract(time_after_dec).Minutes + ":" + cur_time.Subtract(time_after_dec).Seconds + ":" + cur_time.Subtract(time_after_dec).Milliseconds + "]   Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]");
        outlet.push_sample(currentSample);
        //lida//marker
        string text = "NoselectionYN-Marker600: " + "Current Time: [ " + cur_time.ToString("HH:mm:ss.fff")+" ]";
        //lida/????/"eeg_decision_be" is captured at the end of each path. so is this correct that we subtract current time from that time?it means that this time is the decision time
        //text = text + "]   Total Time: [ " + dur_ref_time.TotalSeconds.ToString("0.000") + " ]";
        string lline22 = "Total Time: [ " + dur_ref_time.TotalSeconds.ToString("0.000") + " ]";
        string line33 = "AveTotal Time: [ " + (dur_ref_time.TotalSeconds / i_trial1).ToString("0.000") + " ]";
        //print(text);
        string lline3 = "=============================================================================";

        /*if (flag_flat)
            lline1 = "Right Answer. It is a flat.";//lida//you can define the true-positive, TN, FP, FN here
        else //if (flag_concave || flag_convex)
        {
            Error_rate++;
            //print("wrong-showconfidence_Yes()");
            if (flag_concave)
                lline1 = "Wrong Answer. It is a bump";
            else
                lline1 = "Wrong Answer. It is a hole";
            //lida//you can define the true-positive, TN, FP, FN here
        }*/
        Error_rate_s++;
        string lines1 = "Trial#: " + i_trial1.ToString("00");
        double ii = Error_rate / i_trial1;
        string lline44 = "First ErrorRate: " + ii.ToString("0.00");
        ii = Error_rate_s / i_trial1;
        string lline55 = "Second ErrorRate: " + ii.ToString("0.00");
        //string[] lines = { text, lline1, lline3, lines1, lline3 };
        string[] lines = { text, lline22, line33,lline44,lline55,lines1, lline3 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);
        string[] myline = { lline3, lines1, lline3 };
        File.AppendAllLines(Path.Combine(docPath, marker_result), myline);
        //visual_sur = GameObject.Find("MixedSurface"); 
        //lida//changing the surface type
        if (i_trial1 < Total_trial)
        {
            //timer before starting each session
            //cone.SetActive(true);
            //cone.GetComponent<Renderer>().material.color = Color.black;
            //StartCoroutine(Example());
            /*timer_text.SetActive(true);
            mytimer.SetActive(true);
            float cntdnw = 1.0f;
            if ((cntdnw - Time.deltaTime) > 0)
            {
                cntdnw -= Time.deltaTime;

                double b = System.Math.Round(cntdnw, 2);
                disvar.text = b.ToString();
            }
            else//if (cntdnw < 0)
            {
                timer_text.SetActive(false);
                mytimer.SetActive(false);
                cone.SetActive(true);
                //Debug.Log("Completed");
            }*/
            //timer before starting each session
            sessionID = Session_Number[i_trial1];
            //print("sinH--no-selec--sessionID:    " + sessionID);
            if (sessionID == 1)//bump
            {
                //visual_sur = GameObject.Find("0.3e(-x__2div2__2)");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                visual_sur.transform.position = new Vector3(50.0f, 0.0f, 13.0f); //first task: (50.0f, 0.0f, 0.5f);//three big surfaces//(0.0f, 0.0f, 0.5f); //bigbumphole//(-10.0f, 0.0f, 0.5f);smallbumphole //(0.0f, 0.0f, 0.5f);three small surf //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.04f, 0.0f, 0.0015f); //(0.0f, 0.15f, 0.0f); //(10.0f, 0.0f, 0.15f); //(0.0f, 0.0f, 0.0015f); //
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.3f); //(0.1f, 0.1f, 0.003f); //
                //visual_sur_bump.transform.gameObject.tag = "Touchable";
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //Vector3 newScale = new Vector3(temXZ, temXZ, temXZ); 
                //visual_sur_bump.transform.localScale = newScale;
                flag_concave = true;
                flag_convex = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, -100f);
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_bump.SetActive(true);
                visual_sur_bump.transform.gameObject.tag = "Touchable";
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
                visual_sur_flat.transform.gameObject.tag = "Untagged";
                visual_sur_flat.SetActive(false);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);*/
                //visual_sur_flat.transform.gameObject.tag = "Untagged";
                //visual_sur_flat.SetActive(false);
            }
            else if (sessionID == 2)//hole
            {
                visual_sur.transform.position = new Vector3(25.0f, 0.0f, 13.0f); //first task: (25.0f, 0.0f, 0.4f);//(25.0f, 0.0f, 0.4f); //(0.0f, 0.0f, 0.4f); //(-10.0f, 0.0f, 0.4f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.02f, 0.0f, -0.0015f); //(-25.0f, -0.15f, 0.0f); //(0.0f, 0.0f, -0.15f); //(0.1f, 0.0f, -0.0015f); //
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.3f); //(0.1f, 0.1f, 0.003f); //
                //visual_sur = GameObject.Find("-0.3e(-x__2div2__2)");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //visual_sur_hole.transform.localEulerAngles = new Vector3(90, 180, 0);
                //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, 0.5f);
                //visual_sur_hole.transform.gameObject.tag = "Touchable";
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //Vector3 newScale = new Vector3(temXZ, -temXZ, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = true;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur_bump.transform.position = new Vector3(0.0f, 0.0f, -100f);
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_hole.transform.gameObject.tag = "Touchable";
                visual_sur_hole.SetActive(true);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
                visual_sur_flat.transform.gameObject.tag = "Untagged";
                visual_sur_flat.SetActive(false);*/
                //visual_sur_flat.transform.gameObject.tag = "Untagged";
                //visual_sur_flat.SetActive(false);
            }
            else if (sessionID == 3)//flat
            {
                visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(-20.0f, 0.0f, 0.5f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.2f, 0.0f, 0.0f);//
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(0.1f, 0.1f, 0.0001f); //
                //visual_sur = GameObject.Find("F_Surface");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //visual_sur_flat.transform.gameObject.tag = "Touchable";
                //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = false;
                flag_concave = false;
                flag_flat = true;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_flat.transform.gameObject.tag = "Touchable";
                visual_sur_flat.SetActive(true);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);*/
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
            }
            else if (sessionID == 4)//flat-bump
            {
                visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(-20.0f, 0.0f, 0.5f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.2f, 0.0f, 0.0f);//
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(0.1f, 0.1f, 0.0001f); //
                //visual_sur = GameObject.Find("F_Surface");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //visual_sur_flat.transform.gameObject.tag = "Touchable";
                //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = false;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = true;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_flat.transform.gameObject.tag = "Touchable";
                visual_sur_flat.SetActive(true);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);*/
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
            }
            else if (sessionID == 5)//flat-hole
            {
                visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(-20.0f, 0.0f, 0.5f); //
                //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.2f, 0.0f, 0.0f);//
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(0.1f, 0.1f, 0.0001f); //
                //visual_sur = GameObject.Find("F_Surface");
                //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
                //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
                //visual_sur_flat.transform.gameObject.tag = "Touchable";
                //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ);
                //visual_sur_bump.transform.localScale = newScale;
                flag_convex = false;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = true;
                flag_bump_f = false;
                flag_hole_f = false;
                //visual_sur.SetActive(true);
                //visual_sur.transform.gameObject.tag = "Touchable";
                /*visual_sur_flat.transform.gameObject.tag = "Touchable";
                visual_sur_flat.SetActive(true);
                visual_sur_hole.transform.gameObject.tag = "Untagged";
                visual_sur_hole.SetActive(false);
                visual_sur_bump.transform.gameObject.tag = "Untagged";
                visual_sur_bump.SetActive(false);*/
                //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
            }
            else if (sessionID == 6)//bump-flat
            {
                visual_sur.transform.position = new Vector3(50.0f, 0.0f, 13.0f);//first task: (50.0f, 0.0f, 0.5f);
                myboxCollider.center = new Vector3(0.04f, 0.0f, 0.0015f);
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f);
                flag_concave = false;
                flag_convex = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = true;
                flag_hole_f = false;
            }
            else if (sessionID == 7)//hole-flat
            {
                visual_sur.transform.position = new Vector3(25.0f, 0.0f, 13.0f);//first task: (25.0f, 0.0f, 0.4f);
                myboxCollider.center = new Vector3(0.02f, 0.0f, -0.0015f);
                myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f);
                flag_convex = false;
                flag_concave = false;
                flag_flat = false;
                flag_flat_b = false;
                flag_flat_h = false;
                flag_bump_f = false;
                flag_hole_f = true;
            }
            //lida//changing the surface type
            cone.SetActive(true);
            CameraSwitcher.FindObjectOfType<CameraSwitcher>().SwitchCamera();
            plane.SetActive(true);
            waittimer = 0.0f;
            timer_text.SetActive(true);
            //cone.GetComponent<Renderer>().material.color = Color.black;
            //StartCoroutine(Example());
        }
        else
        {
            currentSample[0] = 200;
            outlet.push_sample(currentSample);
            doneText_session_all.SetActive(true);
            Finalexport(dur_ref_time);
        }
    }
    //lida//having three buttons
    //lida//having two buttons instead of three
    /* public void showconfidence_Notsure()
     {

         eeg_decision_af = System.DateTime.Now;
         decision_after = System.DateTime.Now;
         decision_duration = decision_after.Subtract(decision_before);
         marker.Write(82);
         string text = "Pressed NSU button:82" + " @ " + System.DateTime.Now.ToString("MMMM dd HH:mm:ss.fff");
         string[] lines = { text + "(" + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + ")" };
         File.AppendAllLines(Path.Combine(docPath, out_result), lines);
         //confidenttext.SetActive(true); confidenttextNo.SetActive(true); confidenttextYes.SetActive(false);
         //slider_yes.SetActive(false); slider_yesText.SetActive(false); slider_no.SetActive(true); slider_noText.SetActive(true);
         //nextBu.SetActive(true); //tryagainBu.SetActive(true); 
         //NotsureBu.GetComponent<Image>().color = Color.cyan; buttonNotsureWasClicked = true; buttonNoWasClicked = false; buttonYesWasClicked = false;
     }
     public void showconfidence_No()//non-H
     {
         ///////////////////Lida
         print("showconfidence_No()");
         YesBu.SetActive(false);
         FlatBu.SetActive(false);
         NoBu.SetActive(false);
         IsFlat.SetActive(false);
         //lida//me sessions-add
         if (i_trial1 >= Total_trial)//lida//Total_trial=defines the # of trials for each block
         {
             doneText_session_all.SetActive(true);
         }
         //lida//me sessions-add
         //cone.SetActive(true);
         //lida//my sessions
         //session1.interactable = false; session2.interactable = false; cone.SetActive(false);
         //user_instruction.SetActive(true); session1.gameObject.SetActive(true); session2.gameObject.SetActive(true);
         //doneText_session_all.SetActive(true);
         ////lida//my sessions
         //////////////////////lida
         eeg_decision_af = System.DateTime.Now;
         decision_after = System.DateTime.Now;
         decision_duration = decision_after.Subtract(decision_before);//lida//decision_before: it seems it is the time that all the paths are finished//but this one is not printed
         marker.Write(81);
         string lline1 = "";
         string text = "Pressed NO button:81" + " @ " + System.DateTime.Now.ToString("MMMM dd HH:mm:ss.fff");
         //lida/????/"eeg_decision_be" is captured at the end of each path. so is this correct that we subtract current time from that time?it means that this time is the decision time
         text = text + "(" + eeg_decision_af.Subtract(eeg_decision_be).Minutes + ":" + eeg_decision_af.Subtract(eeg_decision_be).Seconds + ":" + eeg_decision_af.Subtract(eeg_decision_be).Milliseconds + ")";

         //confidenttext.SetActive(true); confidenttextNo.SetActive(true); confidenttextYes.SetActive(false);
         //slider_yes.SetActive(false); slider_yesText.SetActive(false); slider_no.SetActive(true); slider_noText.SetActive(true);
         //nextBu.SetActive(true); //tryagainBu.SetActive(true); 
         //NoBu.GetComponent<Image>().color = Color.cyan; buttonNotsureWasClicked = false; buttonNoWasClicked = true; buttonYesWasClicked = false;
         //lida//me sessions-delete
         //NoBu.GetComponent<Button>().interactable = false; NoBu.GetComponent<Image>().color = Color.yellow;
         //lida//me sessions-delete
         buttonYesWasClicked = false; buttonNoWasClicked = true;//Lida-Jing's// buttonNotsureWasClicked = false;  
         //lida//calculating the error rate of each participant
         if (flag_flat)
         {
             //print("wrong-showconfidence_No()");
             lline1 = "Wrong Answer. It is a Flat surface.";
             //line1 = "The TOTAL used trials are: " + Total_trial.ToString("00");
             //lida//you can define the true-positive, TN, FP, FN here

         }
         else
             lline1 = "Right Answer. It is a Non-Flat surface.";//lida//you can define the true-positive, TN, FP, FN here
                                                                //lida//end//calculating the error rate of each participant


         string[] lines = { text, lline1 };//lida//this part is printed at the end of the .txt file
         //string[] lines = { lines1, lines2, lines3, lines4, lines5, lines6, lines7, lines8, lines9, lines11 };
         File.AppendAllLines(Path.Combine(docPath, out_result), lines);
     }
     public void showconfidence_Yes()//flat-B
     {
         ///////////////////Lida
         print("showconfidence_Yes");
         YesBu.SetActive(false);
         FlatBu.SetActive(false);
         NoBu.SetActive(false);
         IsFlat.SetActive(false);
         //lida//me sessions-add
         if (i_trial1 >= Total_trial)//lida//Total_trial=defines the # of trials for each block
         {
             doneText_session_all.SetActive(true);
         }
         //lida//me sessions-add
         //cone.SetActive(true);
         //lida//my sessions
         //session1.interactable = false; session2.interactable = false; cone.SetActive(false);
         //user_instruction.SetActive(true); session1.gameObject.SetActive(true); session2.gameObject.SetActive(true);
         //doneText_session_all.SetActive(true);
         //lida//my sessions
         //////////////////////lida
         marker.Write(83);
         eeg_decision_af = System.DateTime.Now;
         decision_after = System.DateTime.Now;
         decision_duration = decision_after.Subtract(decision_before);//lida// this time is the decision time
         cur_time = System.DateTime.Now;
         dur_ref_time = cur_time.Subtract(start_ref_time);//lida//start_ref_time: we are in the first path of 4 paths//used for calculating the whole spending time doing an experimnets (4 paths)
         descion_sur_time = cur_time.Subtract(first_sur_time);//lida//prints  first_sur_time:0001-01-01 12:00:00 AM

         //print("first_sur_time" + first_sur_time);
         string text2 = "Selected sur: " + sur_sel;
         if (sur_init == sur_sel)
         {
             cor_num = cor_num + 1;
             text2 = text2 + "(Y)";
             out_corr_index[i_trial1] = "Y" + i_trial1.ToString() + "-";
         }
         else
         {
             text2 = text2 + "(N)";
             out_corr_index[i_trial1] = "N" + i_trial1.ToString() + "-";
         }
         use_anw = text2;
         cor_rate = 100 * cor_num / (i_trial1 + 1.0f);
         out_corr_rate[i_trial1] = cor_rate.ToString("0.00") + "%-";
         //Deng//Wrong//string text = ":83" + " @ " + cur_time.ToString("HH:mm:ss.fff") + "[Decision time: " + descion_sur_time.TotalSeconds.ToString("0.000") + "][" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
         //lida//in Deng's code it prints the upper line which is not so I wrote the following code for printing the decision time
         string text = ":83" + " @ " + cur_time.ToString("HH:mm:ss.fff") + "[Decision time: " + decision_duration.TotalSeconds.ToString("0.000") + "][" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
         //confidenttext.SetActive(true); confidenttextNo.SetActive(false); confidenttextYes.SetActive(true);
         //slider_no.SetActive(false); slider_noText.SetActive(false); slider_yes.SetActive(true); slider_yesText.SetActive(true);
         //nextBu.SetActive(true); //tryagainBu.SetActive(true); 
         //lida//me sessions-delete
         //YesBu.GetComponent<Button>().interactable = false; YesBu.GetComponent<Image>().color = Color.yellow;
         //lida//me sessions-delete
         buttonYesWasClicked = true; buttonNoWasClicked = false;//Lida-Jing's// buttonNotsureWasClicked = false;  
         //lida//calculating the error rate of each participant
         string lline1 = "";
         if (flag_concave || flag_convex)
         {
             //print("wrong-showconfidence_Yes()");
             lline1 = "Wrong Answer. It is a Non-Flat surface.";
             //lida//you can define the true-positive, TN, FP, FN here
         }
         else
             lline1 = "Right Answer. It is a Flat surface.";//lida//you can define the true-positive, TN, FP, FN here

         string[] lines = { text2 + text, lline1 };
         File.AppendAllLines(Path.Combine(docPath, out_result), lines);
         //lida//end//calculating the error rate of each participant
     }
     /*
      //lida//having two buttons instead of three
      
     /*public void next_button()
     {
         //decision_after = System.DateTime.Now; decision_duration = decision_after.Subtract(decision_before);  
         trial_after = System.DateTime.Now; trial_duration = trial_after.Subtract(each_before);
         cur_time = System.DateTime.Now; dur_ref_time = cur_time.Subtract(start_ref_time); trial_duration = dur_ref_time;
         string text0 = "-----------------------------------------------------------------";        
         print("Pressed the NEXT"); marker.Write(90);  
         string text = "Pressed NEXT button:9" + "      @ " + cur_time.ToString("HH:mm:ss.fff") + "[" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";
         string[] lines = { text0, text, text0 }; File.AppendAllLines(Path.Combine(docPath, out_result), lines); 
         threshod_va = transform.localScale.y; 
         if (buttonNoWasClicked)
         {
            // yes_no_anw = "Mismatch"; conf_rate = slider_noTextNo;
         }
         if (buttonNotsureWasClicked)
         {
             //yes_no_anw = "Unsure"; conf_rate = slider_noTextNo;
         }
         if (buttonYesWasClicked)
         {
             yes_no_anw = "Match"; conf_rate = slider_yesTextNo;
         }

         tryagain_flag = "N"; exportRe();
         //if (buttonNoWasClicked)
         //{
         changesconetostart(); changescale(); AnsewshowHide();
         //}
         if (i_trial1 >= Total_trial)
         {
             all_trial_after = System.DateTime.Now; all_trial_duration = all_trial_after.Subtract(all_trial_before);
             Finalexport();
         }
         overdely = false; slider_no.GetComponent<Image>().color = Color.white; slider_yes.GetComponent<Image>().color = Color.white;
         YesBu.GetComponent<Image>().color = Color.white; //NotsureBu.GetComponent<Image>().color = Color.white; NoBu.GetComponent<Image>().color = Color.white;
         if (sessionID < 3) { visual_sur.SetActive(true); }
         if (sessionID > 2) { visual_sur_34.SetActive(true); } //visual_sur_A1.SetActive(false); visual_sur_A2.SetActive(false);
         GameObject cover = GameObject.Find("Cover");
         if (preTest)
         {
             cover.GetComponent<MeshRenderer>().enabled = false; gunSpeedReduced = false;
         }
         else
         {
             cover.GetComponent<MeshRenderer>().enabled = true; gunSpeedReduced = false;
         }
         //x_d = 90; y_d = -90; z_d = 90; transform.rotation = Quaternion.Euler(x_d, y_d, z_d); 
         if (sessionID == 1)
         {
             visual_sur = GameObject.Find("0.3e(-x__2div2__2)");// sinx_256256505002");
             x_d = 90; y_d = 180; z_d = 0;
             visual_sur.transform.localEulerAngles = new Vector3(x_d, y_d, z_d);
             GameObject light = GameObject.Find("DirectionalLight");
             light.transform.localEulerAngles = new Vector3(-45, 80, 0);
         }
         if (sessionID == 2)
         {
             visual_sur = GameObject.Find("0.3e(-x__2div2__2)");// ("sinx_256256505002");
             x_d = 0; y_d = 90; z_d = 270;
             visual_sur.transform.localEulerAngles = new Vector3(x_d, y_d, z_d);
             GameObject light = GameObject.Find("DirectionalLight");
             light.transform.localEulerAngles = new Vector3(-45, 80, 0);
         }
         if (sessionID == 3)
         {
             visual_sur_34 = GameObject.Find("0.3e(-x__2div2__2)");// ("sinx05siny05_256256505002");
             x_d = 90; y_d = 180; z_d = 0;
             visual_sur_34.transform.localEulerAngles = new Vector3(x_d, y_d, z_d);
             GameObject light = GameObject.Find("DirectionalLight"); 
             light.transform.localEulerAngles = new Vector3(-45, 80, 0);
         }
         if (sessionID == 4)
         {
             visual_sur_34 = GameObject.Find("0.3e(-x__2div2__2)");// ("sinx05siny05_256256505002");
             x_d = 315; y_d = 270; z_d = 90;
             visual_sur_34.transform.localEulerAngles = new Vector3(x_d, y_d, z_d);
             GameObject light = GameObject.Find("DirectionalLight");
             light.transform.localEulerAngles = new Vector3(0, 80, 0);
         }
     }*/
    public void next_button()//lida//next button to other block-capturing the duration of that block-printing Tracing accura, Tracing speed, Touching acc-writing a marker 
    {
        //decision_after = System.DateTime.Now; decision_duration = decision_after.Subtract(decision_before);  
        cur_time = System.DateTime.Now;
        trial_duration = cur_time.Subtract(each_before);//lida//each_before: it seems after starting the first path of 4 paths
        //cur_time = System.DateTime.Now;
        dur_ref_time = cur_time.Subtract(all_trial_before);//lida-start time of all blocks//cur_time.Subtract(start_ref_time);//lida//time duration of whole blocks//start_ref_time: we are in the first path of 4 paths when the cone start to move//used for calculating the whole spending time doing an experimnets (4 paths)
        //trial_duration = dur_ref_time;
        decision_after_next = System.DateTime.Now;//lida//added//for capturing the decision time after pressing the next button
        string text0 = "-----------------------------------------------------------------";
        string text = "PressedNEXT-Marker90: " + "Current Time: [ " + cur_time.ToString("HH:mm:ss.fff") + " ]";//  Total Time:  [" + dur_ref_time.TotalSeconds.ToString("0.000") + "]";

        string lline22= "Total Time:  [ " + dur_ref_time.TotalSeconds.ToString("0.000") + " ]"; 
        string[] lines = { text0, text,lline22, text0 };
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);
        threshod_va = transform.localScale.y;

        //if(sessionID==2)
            //visual_sur.transform.position = new Vector3(25.0f, 0.0f, 0.5f);
        /*if (buttonNoWasClicked)
        {
            //Lida//in jing's code this is applyed and I made it uncomment
            yes_no_anw = "Non-flat";//"Hole";
            //conf_rate = slider_noTextNo;
        }
        if (buttonFlatWasClicked)
        {
            yes_no_anw = "Unsure"; //"Flat";
            //conf_rate = slider_noTextNo;
        }
        if (buttonYesWasClicked)
        {
            yes_no_anw = "Flat"; //"Bump";
            //conf_rate = slider_yesTextNo;
        }*/

        tryagain_flag = "N";
        exportRe();//lida//printing Tracing accura, Tracing speed, Touching acc
        //if (buttonNoWasClicked)
        //{
        changesconetostart();//lida//two instructions>>1)PathFollower.shared.distanceTravelled = 0; 2)PathFollower.shared.started = true;
        changescale();//lida//some instructions+scaling the dimention of the surface
        AnsewshowHide();//lida////selecting the kind of the surface// yes_flag = false;path_show.path_show_instace.is_first = true;--making the buttons nonactive

        //lida//marker//here//marker.Write(90);
        if (ReliableFlag)
        {
            ReliableResponse();
        }
        else if (UnReliableFlag)
        {
            UnReliableResponse();
        }
        //currentSample[0] = 90;// "1";// "next";
        //print("Pressed the NEXT");
        outlet.push_sample(currentSample);
        //lida//marker
        //}
        if (i_trial1 >= Total_trial)//lida//Total_trial=defines the # of trials for each block
        {
            //all_trial_after = System.DateTime.Now;
            //all_trial_duration = all_trial_after.Subtract(all_trial_before);//lida//calculating the time elapsed by each block
            //Finalexport();//lida//printing average Tracing accura, Tracing speed, Touching acc//it is calledwhen all iterations are finished
            cone.SetActive(false);
            //CameraSwitcher.FindObjectOfType<CameraSwitcher>().SwitchCamera();
        }
        //overdely = false; slider_no.GetComponent<Image>().color = Color.white; slider_yes.GetComponent<Image>().color = Color.white;
        //Lida//in jing's code this is applyed
        //YesBu.GetComponent<Image>().color = Color.white; //NotsureBu.GetComponent<Image>().color = Color.white; 
        //NoBu.GetComponent<Image>().color = Color.white;
        //lida//enter the system response
        //????
        //lida//enter the system response
        YesBu.GetComponent<Image>().color = Color.white;
        FlatBu.GetComponent<Image>().color = Color.white;
        NoBu.GetComponent<Image>().color = Color.white;
        overdely = false;
        nextBu.SetActive(false);
        YesBu.SetActive(true);
        FlatBu.SetActive(true);
        NoBu.SetActive(true);
        user_instruction.SetActive(true);
        //Lida
        //if (sessionID < 3) { visual_sur.SetActive(true); }
        // if (sessionID > 2) { visual_sur_34.SetActive(true); } //visual_sur_A1.SetActive(false); visual_sur_A2.SetActive(false);
        GameObject cover = GameObject.Find("Cover");
        if (preTest)//lida//it is false
        {
            cover.GetComponent<MeshRenderer>().enabled = false; gunSpeedReduced = false;
        }
        else
        {
            cover.GetComponent<MeshRenderer>().enabled = true; gunSpeedReduced = false;
        }

        //x_d = 90; y_d = -90; z_d = 90; transform.rotation = Quaternion.Euler(x_d, y_d, z_d); 
        /*if (sessionID == 1)
        {
            visual_sur = GameObject.Find("sinx_256256505002");
            x_d = 90; y_d = 180; z_d = 0;
            visual_sur.transform.localEulerAngles = new Vector3(x_d, y_d, z_d);
            GameObject light = GameObject.Find("DirectionalLight");
            light.transform.localEulerAngles = new Vector3(-45, 80, 0);
        }
        if (sessionID == 2)
        {
            visual_sur = GameObject.Find("sinx_256256505002");
            x_d = 0; y_d = 90; z_d = 270;
            visual_sur.transform.localEulerAngles = new Vector3(x_d, y_d, z_d);
            GameObject light = GameObject.Find("DirectionalLight");
            light.transform.localEulerAngles = new Vector3(-45, 80, 0);
        }
        if (sessionID == 3)
        {
            visual_sur_34 = GameObject.Find("sinx05siny05_256256505002");
            x_d = 90; y_d = 180; z_d = 0;
            visual_sur_34.transform.localEulerAngles = new Vector3(x_d, y_d, z_d);
            GameObject light = GameObject.Find("DirectionalLight"); 
            light.transform.localEulerAngles = new Vector3(-45, 80, 0);
        }
        if (sessionID == 4)
        {
            visual_sur_34 = GameObject.Find("sinx05siny05_256256505002");
            x_d = 315; y_d = 270; z_d = 90;
            visual_sur_34.transform.localEulerAngles = new Vector3(x_d, y_d, z_d);
            GameObject light = GameObject.Find("DirectionalLight");
            light.transform.localEulerAngles = new Vector3(0, 80, 0);
        }*/

    }
    public void tryagain_button()
    {
        /*
         * decision_after = System.DateTime.Now; decision_duration = decision_after.Subtract(decision_before);
        threshod_va = transform.localScale.y;
        if (buttonNoWasClicked)
        {
            yes_no_anw = "Non-flat"; conf_rate = slider_noTextNo;
        }
        if (buttonNotsureWasClicked)
        {
           // yes_no_anw = "Unsure"; conf_rate = slider_noTextNo;
        }
        if (buttonYesWasClicked)
        {
            yes_no_anw = "Flat"; conf_rate = slider_yesTextNo;
        }
        trial_after = System.DateTime.Now; trial_duration = trial_after.Subtract(each_before);
        tryagain_flag = "Y"; exportRe(); i_trial1 = i_trial1 - 1;
        AnsewshowHide(); changesconetostart(); //changescale();
        //if (i_trial1 >= Total_trial)
        //{
        //    all_trial_after = System.DateTime.Now; all_trial_duration = all_trial_after.Subtract(all_trial_before);
        //    Finalexport();
        //}
        overdely = false; slider_no.GetComponent<Image>().color = Color.white; slider_yes.GetComponent<Image>().color = Color.white;
        YesBu.GetComponent<Image>().color = Color.white; NotsureBu.GetComponent<Image>().color = Color.white; NoBu.GetComponent<Image>().color = Color.white; //
        */
    }

    public void changescale()//lida//some instructions+scaling the dimention of the surface//called in  next_button()
    {
        //print("changescale");
        frameNumber = 0;
        frameNumber_path = 0;
        touchNumber = 0;
        touchNumber_path = 0;
        tracingNumber = 0;
        tracingNumber_path = 0;
        touchAcc = 0;
        tracingAcc = new Vector2(0, 0);
        tracingSpeed = new Vector2(0, 0);
        responsetimer = 0;
        showtimer = 0;
        //Lida//one_height
        /*
        if (isTest)//lida//now it is checked/true
        {
            if (current_session == 1)//Lida//shows the block number--(flag_concave | flag_convex) && 
            {
                //Vector3 newScale = new Vector3(temXZ, threshod_value_curr, temXZ);
                //newScale = newScale - new Vector3(0.0f, 0.008f, 0.0f);
                Vector3 newScale;
                if (flag_concave | flag_convex)//lida//for non_falt surface
                {
                    newScale = new Vector3(temXZ, threshod_value[0], temXZ); //i_trial1], temXZ);
                    transform.localScale = newScale;
                    threshod_value_curr = transform.localScale.y;
                }
            }
            if ( current_session == 2)//Lida//shows the block number//rotate the surface--(flag_concave | flag_convex) &&
            {
                //Vector3 newScale = new Vector3(temXZ, threshod_value_curr, temXZ);
                //newScale = newScale + new Vector3(0.0f, 0.008f, 0.0f);
                Vector3 newScale;
                if (flag_concave | flag_convex)//lida//for non_falt surface
                {
                    newScale = new Vector3(temXZ, threshod_value[0], temXZ); //Mathf.Abs(Total_trial-i_trial1-1)], temXZ);
                    transform.localScale = newScale;
                    threshod_value_curr = transform.localScale.y;
                }
            }
        }
        else
        {
            if ((flag_concave | flag_convex)&& current_session == 1)//lida//(flag_concave | flag_convex) &&
            {
                //Vector3 newScale = new Vector3(temXZ, threshod_value_curr, temXZ);
                //newScale = newScale - new Vector3(0.0f, 0.008f, 0.0f);
                Vector3 newScale = new Vector3(temXZ, threshod_value[i_trial1], temXZ);
                transform.localScale = newScale;
                threshod_value_curr = transform.localScale.y; 
            }
            if ((flag_concave | flag_convex) && current_session == 2)//lida//--(flag_concave | flag_convex) && 
            {
                //Vector3 newScale = new Vector3(temXZ, threshod_value_curr, temXZ);
                //newScale = newScale + new Vector3(0.0f, 0.008f, 0.0f);
                Vector3 newScale = new Vector3(temXZ, threshod_value[Mathf.Abs(Total_trial - i_trial1 - 1)], temXZ);
                transform.localScale = newScale;
                threshod_value_curr = transform.localScale.y;
            }*/
        /*this part is comment in Deng's code
         if (Total_trial == 2)
        {
            if (i_trial1 == 1)
            {
                if (current_session == 1)
                {
                    Vector3 newScale = new Vector3(temXZ, threshod_value[9], temXZ);
                    transform.localScale = newScale;
                }
                if (current_session == 2)
                {
                    Vector3 newScale = new Vector3(temXZ, threshod_value[0], temXZ);
                    transform.localScale = newScale;
                }
            }
            threshod_value_curr = transform.localScale.y;
        }
        if (Total_trial == 3)
        {
            if (i_trial1 == 1)
            {
                Vector3 newScale = new Vector3(temXZ, threshod_value[6], temXZ);
                transform.localScale = newScale;
            }
            if (i_trial1 == 2)
            {
                if (current_session == 1)
                {
                    Vector3 newScale = new Vector3(temXZ, threshod_value[8], temXZ);
                    transform.localScale = newScale;
                }
                if (current_session == 2)
                {
                    Vector3 newScale = new Vector3(temXZ, threshod_value[1], temXZ);
                    transform.localScale = newScale;
                }                    
            }                
            threshod_value_curr = transform.localScale.y;
        }this part is comment in Deng's code*/
        //}
        //Lida//one_height
    }
    public void changesconetostart()//lida//it is called in next_button()
    {
        //print("changesconetostart");
        PathFollower.shared.distanceTravelled = 0;
        PathFollower.shared.started = true; //save_previous_state = true;// false;
        //transform.localScale -= new Vector3(0, 0.01f, 0f);  
    }
    void exportRe()//lida//it is called in next_button()//printing Tracing accura, Tracing speed, Touching acc
    {
        //print("exportRe");
        i_trial1 = i_trial1 + 1; touchAcc_m = touchAcc_m + touchAcc; //tracingAcc_m = tracingAcc_m + tracingAcc;
        //tracingAcc_row_m = 0;
        tracingAcc_row_m = (float)(tracingAcc_path[0].x + tracingAcc_path[1].x) / 2;
        //tracingAcc_col_m = 0;
        tracingAcc_col_m = (float)(tracingAcc_path[2].y + tracingAcc_path[3].y) / 2;

        //tracingSpeed_row_m = 0;
        tracingSpeed_row_m = (float)(tracingSpeed_path[0].x + tracingSpeed_path[1].x) / 2;
        //tracingSpeed_col_m = 0;
        tracingSpeed_col_m = (float)(tracingSpeed_path[2].y + tracingSpeed_path[3].y) / 2;

        //touchAcc_row_m = 0;
        touchAcc_row_m = (float)(touchAcc_path[0] + touchAcc_path[1]) / 2;
        //touchAcc_col_m = 0;
        touchAcc_col_m = (float)(touchAcc_path[2] + touchAcc_path[3]) / 2;

        if (tryagain_flag == "N")
        {
            //print("if (tryagain_flag == )");
            tracingAcc_row_mm = tracingAcc_row_mm + tracingAcc_row_m;
            tracingAcc_col_mm = tracingAcc_col_mm + tracingAcc_col_m;
            tracingSpeed_row_mm = tracingSpeed_row_mm + tracingSpeed_row_m;
            tracingSpeed_col_mm = tracingSpeed_col_mm + tracingSpeed_col_m;
            touchAcc_row_mm = touchAcc_row_mm + touchAcc_row_m;
            touchAcc_col_mm = touchAcc_col_mm + touchAcc_col_m;
        }

        // Create a string array with the additional lines of text
        string lines3 = "Tracing accura L1 is: " + tracingAcc_path[0].ToString("0.000");
        string lines4 = "               L2 is: " + tracingAcc_path[1].ToString("0.000");
        string lines5 = "               L3 is: " + tracingAcc_path[2].ToString("0.000");
        //string lines6 = "               L4 is: " + tracingAcc_path[3].ToString("0.000");
        string lines9 = "Tracing acc ave_row is: " + tracingAcc_row_m.ToString("0.000");
        //string lines10 = "Tracing acc ave_col is: " + tracingAcc_col_m.ToString("0.000");
        string lines11 = "...........................................";
        string lines12 = "Tracing speed L1 is: " + tracingSpeed_path[0].ToString("0.000");
        string lines13 = "              L2 is: " + tracingSpeed_path[1].ToString("0.000");
        string lines14 = "              L3 is: " + tracingSpeed_path[2].ToString("0.000");
        //string lines15 = "              L4 is: " + tracingSpeed_path[3].ToString("0.000");
        string lines18 = "Tracing speed ave_row is: " + tracingSpeed_row_m.ToString("0.000");
        //string lines19 = "Tracing speed ave_col is: " + tracingSpeed_col_m.ToString("0.000");
        string lines20 = "...........................................";
        string lines21 = "Touching acc L1 is: " + touchAcc_path[0].ToString("0.000");
        string lines22 = "             L2 is: " + touchAcc_path[1].ToString("0.000");
        string lines23 = "             L3 is: " + touchAcc_path[2].ToString("0.000");
        //string lines24 = "             L4 is: " + touchAcc_path[3].ToString("0.000");
        string lines27 = "Touching acc ave_row is: " + touchAcc_row_m.ToString("0.000");
        //string lines28 = "Touching acc ave_col is: " + touchAcc_col_m.ToString("0.000");
        string lines29 = "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++";
        //string lines30 = "The threshod  value: " + threshod_va.ToString("0.000");
        if (use_anw == "") { use_anw = "no selection!"; }
        string lines31 = "Users'  answer   is: " + use_anw;// yes_no_anw;// + " and the rate is: " + conf_rate.ToString("00") + "%";
        //string lines32 = "Used  decision time: " + decision_duration.Seconds + "." + decision_duration.Milliseconds + "s" + " Overdely: " + overdely;
        string lines33 = "Elapsed trial  time: " + trial_duration.TotalSeconds.ToString("0.000") + "s";//trial_duration.Seconds + "." + trial_duration.Milliseconds + "s"; 
        string lines35 = "[" + string.Join("", out_threshod_value) + "]";
        string lines36 = "[" + string.Join("", out_squ_index) + "]";
        string lines37 = "[" + string.Join("", out_corr_index) + "]";
        string lines38 = "[CorrectRate: " + string.Join("", (out_corr_rate)) + "]";
        string lines34 = "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++";
        //string[] lines = { lines3, lines4, lines5, lines6, lines9, lines10, lines11, lines12, lines13, lines14, lines15, lines18, lines19, lines20, lines21, lines22, lines23, lines24, lines27, lines28, lines29, lines33, lines34 };
        string[] lines = { lines3, lines4, lines5, lines9, lines11, lines12, lines13, lines14, lines18, lines20, lines21, lines22, lines23, lines27, lines29, lines33, lines34 };
        // Append new lines of text to the file
        File.AppendAllLines(Path.Combine(docPath, out_result), lines);
        conf_rate = 0;
        //is_output = false; is_up = false; is_down = false;
    }
    public void Finalexport(System.TimeSpan last_time)//lida//printing average Tracing accura, Tracing speed, Touching acc//it is called when all iterations are finished
    {
        //cone.SetActive(false);
        //YesBu.SetActive(false); NoBu.SetActive(false);
        //lida//my sesssions
        /*switch (session_flg)
        {
            case "FF":
                session_flg_1 = false; session_flg_2 = false; welcome_settings(); break;
            case "TF":
                session_flg_1 = true; welcome_settings(); break;//doneText_session1.SetActive(true); 
            case "FT":
                session_flg_2 = true; welcome_settings(); break;//doneText_session2.SetActive(true); 
            case "TT":
                session_flg_1 = true; session_flg_2 = true;
                
                cone.SetActive(false);
                //lida-modified
                //session1.interactable = false; session2.interactable = false; 
                //user_instruction.SetActive(true); session1.gameObject.SetActive(true); session2.gameObject.SetActive(true);
                //doneText_session_all.SetActive(true);
                //lida-modified
                //YesBu.SetActive(true);
                // NoBu.SetActive(true);
                IsFlat.SetActive(true);

                //lida
                break;
        }*/
        //string lines1 = lines_tem;
        string lines1 = "##############################" + " [" + all_before.ToString("HH:mm:ss.fff tt") + "]" + "##############################";//lida//start time of each block
        string lines2 = "The TOTAL used trials are: " + Total_trial.ToString("00");
        string lines3 = "The ave tracing accuracy row is: " + (tracingAcc_row_mm / Total_trial).ToString("0.000");
        //string lines4 = "The ave tracing accuracy col is: " + (tracingAcc_col_mm / Total_trial).ToString("0.000");
        string lines5 = "The ave tracing speed row is: " + (tracingSpeed_row_mm / Total_trial).ToString("0.000");
        //string lines6 = "The ave tracing speed col is: " + (tracingSpeed_col_mm / Total_trial).ToString("0.000");
        string lines7 = "The ave touching accuracy row is: " + (touchAcc_row_mm / Total_trial).ToString("0.000");
        //string lines8 = "The ave touching accuracy col is: " + (touchAcc_col_mm / Total_trial).ToString("0.000");
        //all_trial_after = System.DateTime.Now;
        //all_trial_duration = all_trial_after.Subtract(all_trial_before);
        //string lines9 = "Block   elapsed    time   is: " + all_trial_duration.Minutes + ":" + all_trial_duration.Seconds + "." + all_trial_duration.Milliseconds + " miniutes";//lida//calculating the time elapsed by each block
        //string lines5 = "The noticeable threshold  is: " + transform.localScale.y.ToString("0.000");
        string lines10 = "Correct number: " + (cor_num).ToString("00");
        string lines11 = "##############################" + " [" + System.DateTime.Now.ToString("HH:mm:ss.fff tt") + "]" + "##############################";
        all_after = System.DateTime.Now;
        all_duration = all_after.Subtract(all_before);
        //string lines12 = "Total Elapsed Time is: " + dur_ref_time.Minutes + ":" + dur_ref_time.Seconds + ":" + dur_ref_time.Milliseconds + " miniutes";//lida//from the time ok button is pressed
        //string lines12 = "Total Elapsed Time is: " + last_time.Minutes + ":" + last_time.Seconds + ":" + last_time.Milliseconds + " miniutes";//lida//from the time ok button is pressed
        //print(lines12);
        string lines13 = "[" + string.Join("", out_threshod_value) + "]";
        string lines14 = "[" + string.Join("", out_squ_index) + "]";
        string lines15 = "[" + string.Join("", out_corr_index) + "]";
        string lines16 = "[CorrectRate: " + string.Join("", (out_corr_rate)) + "]";
        string lline2 = "";
        Error_rate = Error_rate / Total_trial;
        lline2 = "First ErrorRate: " + Error_rate.ToString("0.00");
        Error_rate_s = Error_rate_s / i_trial1;
        string lline55 = "Second ErrorRate: " + Error_rate_s.ToString("0.00");
        //if (string.Compare(session_flg, "TT") != 0)
        //{
        //   string[] lines = { lines1, lines2, lines3, lines4, lines5, lines6, lines7, lines8, lines9, lines11 };
        //   File.AppendAllLines(Path.Combine(docPath, out_result), lines);
        // }
        // else
        //{
        //string[] liness = { lines1, lines2, lines3, lines4, lines5, lines6, lines7, lines8, lines11, lines12, lline2 };
        string[] liness = { lines1, lines2, lines3, lines5, lines7, lline2, lline55, lines11 };
        File.AppendAllLines(Path.Combine(docPath, out_result), liness);
        // }
    }

    public void welcome_settings()//lida//called in startIsPressed() and Finalexport() (if it is the begining of the exp or we have blocks that have not tried yet)
    {
        user_instruction.SetActive(false);
        cone.SetActive(false);
        //CameraSwitcher.FindObjectOfType<CameraSwitcher>().SwitchCamera();
        YesBu.SetActive(false); //NotsureBu.SetActive(false);  //
        FlatBu.SetActive(false);
        //Lida// it is applyed in jing's code
        NoBu.SetActive(false);
        //Lida
        //IsFlat.SetActive(false); IsNotFlat.SetActive(false);
        //confidenttext.SetActive(false); confidenttextNo.SetActive(false); confidenttextYes.SetActive(false);
        //slider_yes.GetComponent<Slider>().value = 0; slider_no.GetComponent<Slider>().value = 0;
        //slider_yes.SetActive(false); slider_yesText.SetActive(false);
        //slider_no.SetActive(false); slider_noText.SetActive(false);
        nextBu.SetActive(false); //tryagainBu.SetActive(false); 
        //lida//my sessions
        //session1.gameObject.SetActive(true); session2.gameObject.SetActive(true);
        //lida//my sessions
        //doneText_session1.SetActive(false);
        //doneText_session2.SetActive(false);
        //lida//my sessions
        /*switch (session_flg)
        {
            case "FF":
                doneText_session_all.SetActive(false); session1.interactable = true; session2.interactable = true; break;
            case "TF":
                doneText_session_all.SetActive(false); session1.interactable = false; session2.interactable = true; break;
            case "FT":
                doneText_session_all.SetActive(false); session1.interactable = true; session2.interactable = false; break;
            case "TT":
                session1.interactable = false; session2.interactable = false; break;
                //doneText_session_all.SetActive(true);
        }*/
        //lida//my sessions
        //PathFollower.shared.distanceTravelled = 0;
    }

    //lida//called in next_button(), block1, block2//selecting the kind of the surface//yes_flag = false;path_show.path_show_instace.is_first = true;--making the buttons nonactive
    public void AnsewshowHide()
    {
        //print("AnsewshowHide");
        //yes_flag = false;
        //IsFlat.SetActive(false); IsNotFlat.SetActive(false);
        user_instruction.SetActive(false);
        //session1.gameObject.SetActive(false); session2.gameObject.SetActive(false);
        //YesBu.SetActive(false); //NotsureBu.SetActive(false); //
        //FlatBu.SetActive(false);
        //YesBu.GetComponent<Image>().color = Color.white; //NotsureBu.GetComponent<Image>().color = Color.white; 
        //Lida//apply in jing's code
        //NoBu.SetActive(false);
        //NoBu.GetComponent<Image>().color = Color.white;
        //Lida
        //confidenttext.SetActive(false); confidenttextNo.SetActive(false); confidenttextYes.SetActive(false);
        //slider_yes.GetComponent<Slider>().value = 0;
        //slider_yes.SetActive(false); slider_yesText.SetActive(false);
        //slider_no.GetComponent<Slider>().value = 0;
        //slider_no.SetActive(false); slider_noText.SetActive(false);
        nextBu.SetActive(false); //tryagainBu.SetActive(false); 
        //lida//my sessions
        //cone.SetActive(true); //cone.transform.position = new Vector3(-2.24f, 1.6f, 0f); 
        //lida//my sessions
        path_show.path_show_instace.is_first = true;//lida//at the end of each block we make it true for the next block and after starting each block we make it false 
        responsetimer = 0; showtimer = 0;
        for (int i = 0; i < alpha.Count; i++)//lida//alpha = new List<int> {0,1,2,3};//a kinf of Permutation
        {
            int temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }

        /*if (sessionID == 1)
        {
            visual_sur = GameObject.Find("0.3e(-x__2div2__2)");
            visual_sur.transform.localEulerAngles = new Vector3(90, 180, 0);
            cone.transform.position = new Vector3(-2.8f, 0f, 0f);
            flag_concave = true;
        }
        else if (sessionID == 2)
        {
            visual_sur = GameObject.Find("0.3e(-x__2div2__2)");
            visual_sur.transform.localEulerAngles = new Vector3(0, 90, 270);
            cone.transform.position = new Vector3(0f, 2.8f, 0f);
            flag_concave = true;
        }
        else if (sessionID == 3)
        {
            visual_sur = GameObject.Find("F_Surface");
            flag_flat = true;
        }
        else if (sessionID == 4)
        {
            visual_sur = GameObject.Find("-0.3e(-x__2div2__2)");
            visual_sur.transform.localEulerAngles = new Vector3(90, 180, 0);
            cone.transform.position = new Vector3(-2.8f, 0f, 0f);
            flag_convex = true;
        }
        else if (sessionID == 5)
        {
            visual_sur = GameObject.Find("-0.3e(-x__2div2__2)");
            visual_sur.transform.localEulerAngles = new Vector3(0, 90, 270);
            cone.transform.position = new Vector3(0f, 2.8f, 0f);
            flag_convex = true;
        }
        visual_sur.transform.gameObject.tag = "Touchable";
        //lida//my sessions-delete
        //comment
        //if (current_session == 1)
        //{
         //   if (flag_concave | flag_convex)//lida//I want to rotate the surface when it is not flat
         //       visual_sur.transform.localEulerAngles = new Vector3(90, 180, 0);
         //   cone.transform.position = new Vector3(-2.8f, 0f, 0f);
        //}
        //if (current_session == 2)
        //{
         //   if (flag_concave | flag_convex)//lida//I want to rotate the surface when it is not flat
         //       visual_sur.transform.localEulerAngles = new Vector3(0, 90, 270);
         //   cone.transform.position = new Vector3(0f, 2.8f, 0f);
        //}
        ///lida//my sessions-delete
        if (sessionID == -12)
        {
            visual_sur = GameObject.Find("0.3e(-x__2div2__2)");// ("sinx_256256505002");
            visual_sur.transform.gameObject.tag = "Touchable";
            if (preTest)
            {
                x_d = squ_index_angle[squ_index_training[i_trial1], 0];
            }
            else
            {
                x_d = squ_index_angle[squ_index[i_trial1], 0];
            }
            y_d = squ_index_angle[0, 1]; z_d = squ_index_angle[0, 2];
            switch (x_d)
            {
                case 90:
                    sur_init = "Case |";
                    break;
                case 0:
                    sur_init = "Case -";
                    break;
                case 20:
                    sur_init = "Case \\";
                    break;
                case 340:
                    sur_init = "Case /";
                    break;
            }
            visual_sur.transform.localEulerAngles = new Vector3(x_d, y_d, z_d);
            GameObject light = GameObject.Find("DirectionalLight");
            light.transform.localEulerAngles = new Vector3(-45, 80, 0);
            first_show_sur1 = true; sur_sel = ""; first_show_sur2 = true; first_show_sur3 = true; first_show_sur4 = true;
            first_show_end1 = true; first_show_end2 = true; first_show_end3 = true; first_show_end4 = true;
            out_squ_index[i_trial1] = "N" + i_trial1.ToString() + "-";
            cor_rate = 100 * cor_num / (i_trial1 + 1.0f);
            out_corr_rate[i_trial1] = cor_rate.ToString("0.00") + "%-";
            if (preTest)
            {
                out_threshod_value[i_trial1] = threshod_value_curr.ToString("0.000") + "-"; out_squ_index[i_trial1] = squ_index_training[i_trial1].ToString() + "-";
            }
            else
            {
                out_threshod_value[i_trial1] = threshod_value_curr.ToString("0.000") + "-"; out_squ_index[i_trial1] = squ_index[i_trial1].ToString() + "-";
            }
            if (string.IsNullOrEmpty(out_corr_index[i_trial1])) { out_corr_index[i_trial1] = "O" + i_trial1.ToString() + "-"; }
        }*/
        /*if (i_trial1 < Total_trial)
        {
            int ii = i_trial1 + 1;
            string lines1 = "Trial: " + ii.ToString("00"); string lines2 = "=============================================================================";
            string[] lines = { lines2, lines1, lines2 };
            File.AppendAllLines(Path.Combine(docPath, out_result), lines);
        }*/
        //PathFollower.shared.distanceTravelled = 0;
        //PathFollower.shared.started = false; 
        //PathFollower.shared.path_finished = false; 
        //PathFollower.shared.all_end = false;
    }
    /* public void Session_1_start()
     {
         if (session1.interactable)
         {
             current_session = 1;
             touchAcc_m = 0;
             cor_num = 0; //tracingAcc_m = Vector3.zero;
             tracingSpeed = Vector2.zero;
             i_trial1 = 0;

         char[] ch = session_flg.ToCharArray(); ch[0] = 'T';
             string tem = new string(ch);
             session_flg = tem;
             session_flg_1 = false;
             string lines1 = "********************************** Block 1 **********************************";
             string[] lines = { lines1 };
             File.AppendAllLines(Path.Combine(docPath, out_result), lines);
             AnsewshowHide();
             //lida//my sessions
             cone.SetActive(true); //cone.transform.position = new Vector3(-2.24f, 1.6f, 0f); 
             //lida//my sessions
         }
     }
     public void Session_2_start()
     {
         if (session2.interactable)
         {
             current_session = 2;
             touchAcc_m = 0;
             cor_num = 0; //tracingAcc_m = Vector3.zero;
             tracingSpeed = Vector2.zero;
             i_trial1 = 0;

             char[] ch = session_flg.ToCharArray(); ch[1] = 'T';
             string tem = new string(ch); session_flg = tem;
             session_flg_2 = false;
             string lines1 = "********************************** Block 2 **********************************";
             string[] lines = { lines1 };
             File.AppendAllLines(Path.Combine(docPath, out_result), lines);
             AnsewshowHide();
             //lida//my sessions
             cone.SetActive(true); //cone.transform.position = new Vector3(-2.24f, 1.6f, 0f); 
             //lida//my sessions
         }
     }*/
    //public void yesIsPressed()
    //{
    //     exportRe();
    //}
    public void startIsPressed()//lida//ok button
    {
        currentSample[0] = 1005;// "1";// "hole-B";
        //print("Hole");
        outlet.push_sample(currentSample);
        welcome.SetActive(false); subId.SetActive(false); subText.SetActive(false); startbutton.SetActive(false);
        // Create a string with a line of text.
        string Id = subText.GetComponent<InputField>().text;
        //Id = "001";
        subID = Id;
        Id = "00";
        if (Id != "")
        {
            string text;
            if (ReliableFlag)
            {
                out_result = "./Results/ReliableSession_Sub" + Id + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm-ss.fff") + out_result;
                //marker_result = "./Results/MarkerReliableSession" + sessionID + "_Sub" + Id + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm-ss") + marker_result;
                marker_result = "./Results/MarkerReliableSession_Sub" + Id + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm-ss.fff") + marker_result;
                text = "Reliable-Marker: 1003-Subject ID: " + Id + " [" + System.DateTime.Now.ToString("HH:mm:ss.fff tt") + " " + System.DateTime.Today.ToString("yyy/MM/dd") + "]" + System.Environment.NewLine;
            }
            else if (UnReliableFlag)
            {
                out_result = "./Results/UnReliableSession_Sub" + Id + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm-ss.fff") + out_result;
                //marker_result = "./Results/MarkerUnReliableSession" + sessionID + "_Sub" + Id + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm-ss") + marker_result;
                marker_result = "./Results/MarkerUnReliableSession_Sub"+ Id + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm-ss.fff") + marker_result;
                text = "UnReliableSession-Marker: 1004-Subject ID: " + Id + " [" + System.DateTime.Now.ToString("HH:mm:ss.fff tt") + " " + System.DateTime.Today.ToString("yyy/MM/dd") + "]" + System.Environment.NewLine;
            }
            else// if (MixedReliableFlag)
            {
                out_result = "./Results/MixedSession_Sub" + Id + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm-ss.fff") + out_result;
                //marker_result = "./Results/MarkerMixedSession" + sessionID + "_Sub" + Id + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm-ss") + marker_result;
                marker_result = "./Results/MarkerMixedSession_Sub" + Id + "_" + System.DateTime.Now.ToString("MM-dd-HH-mm-ss.fff") + marker_result;
                text = "MixedSession-Marker: 1000-Subject ID: " + Id + " [" + System.DateTime.Now.ToString("HH:mm:ss.fff tt") + " " + System.DateTime.Today.ToString("yyy/MM/dd") + "]" + System.Environment.NewLine;
            }
            // Set a variable to the Documents path.
            File.WriteAllText(Path.Combine(docPath, out_result), text);
            File.WriteAllText(Path.Combine(docPath, marker_result), text);
            welcome_settings();
            all_before = System.DateTime.Now;
            //lida//my sessions- add this part
            AnsewshowHide();
            //timer before starting each session
            //timer_text.SetActive(true);
            //StartCoroutine(Example());

            cone.SetActive(true);
            CameraSwitcher.FindObjectOfType<CameraSwitcher>().SwitchCamera();
            plane.SetActive(true);
            waittimer = 0.0f;
            timer_text.SetActive(true);
            cone.GetComponent<Renderer>().material.color = Color.black;
            //StartCoroutine(Example());
            /*mytimer.SetActive(true);
            float cntdnw = 1.0f;
            if ((cntdnw - Time.deltaTime) > 0)
            {
                cntdnw -= Time.deltaTime;

                double b = System.Math.Round(cntdnw, 2);
                disvar.text = b.ToString();
            }
            else//if (cntdnw < 0)
            {
                timer_text.SetActive(false);
                mytimer.SetActive(false);
                cone.SetActive(true);
                //Debug.Log("Completed");
            }*/

            //timer before starting each session
            //lida//my sessions- add this part
        }
        else
        {
            welcome.SetActive(true); subId.SetActive(true); subText.SetActive(true); startbutton.SetActive(true);
            visual_sur.transform.position = new Vector3(50.0f, 0.0f, 17.0f);
        }
        //transform.localScale = new Vector3(0.5f, threshod_value[i_trial], 0.5f);
    }

    //final configuearion for SessionID************************************
    void GiveSessionIDMatchingReliable()
    {
        Session_Number[0] = 1;//bump
        Session_Number[1] = 3;//flat
        Session_Number[2] = 1;
        Session_Number[3] = 2;//hole
        Session_Number[4] = 2;
        Session_Number[5] = 3;
        Session_Number[6] = 1;
        Session_Number[7] = 1;
        Session_Number[8] = 2;
        Session_Number[9] = 3;
        Session_Number[10] = 2;
        Session_Number[11] = 3;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 1;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 1;
        MixedRe[5] = 1;
        MixedRe[6] = 1;
        MixedRe[7] = 1;
        MixedRe[8] = 1;
        MixedRe[9] = 1;
        MixedRe[10] = 1;
        MixedRe[11] = 1;
    }
    void GiveSessionIDMatchingUnReliable1()
    {
        Session_Number[0] = 3;
        Session_Number[1] = 1;
        Session_Number[2] = 1;
        Session_Number[3] = 2;
        Session_Number[4] = 3;
        Session_Number[5] = 3;
        Session_Number[6] = 2;
        Session_Number[7] = 1;
        Session_Number[8] = 2;
        Session_Number[9] = 1;
        Session_Number[10] = 3;
        Session_Number[11] = 2;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 0;
        MixedRe[2] = 1;
        MixedRe[3] = 0;
        MixedRe[4] = 0;
        MixedRe[5] = 1;
        MixedRe[6] = 1;
        MixedRe[7] = 0;
        MixedRe[8] = 1;
        MixedRe[9] = 1;
        MixedRe[10] = 0;
        MixedRe[11] = 0;
    }

    void GiveSessionIDMatchingUnReliable2()
    {
        Session_Number[0] = 1;
        Session_Number[1] = 3;
        Session_Number[2] = 3;
        Session_Number[3] = 2;
        Session_Number[4] = 1;
        Session_Number[5] = 2;
        Session_Number[6] = 3;
        Session_Number[7] = 1;
        Session_Number[8] = 1;
        Session_Number[9] = 2;
        Session_Number[10] = 2;
        Session_Number[11] = 3;
        ////////
        MixedRe[0] = 0;
        MixedRe[1] = 1;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 0;
        MixedRe[5] = 0;
        MixedRe[6] = 0;
        MixedRe[7] = 1;
        MixedRe[8] = 1;
        MixedRe[9] = 1;
        MixedRe[10] = 0;
        MixedRe[11] = 0;
    }
    void GiveSessionIDMisMatchingReliable_c()
    {
        Session_Number[0] = 5;
        Session_Number[1] = 6;
        Session_Number[2] = 7;
        Session_Number[3] = 7;
        Session_Number[4] = 4;
        Session_Number[5] = 4;
        Session_Number[6] = 5;
        Session_Number[7] = 5;
        Session_Number[8] = 6;
        Session_Number[9] = 4;
        Session_Number[10] = 4;
        Session_Number[11] = 5;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 1;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 1;
        MixedRe[5] = 1;
        MixedRe[6] = 1;
        MixedRe[7] = 1;
        MixedRe[8] = 1;
        MixedRe[9] = 1;
        MixedRe[10] = 1;
        MixedRe[11] = 1;
    }
    void GiveSessionIDMisMatchingUnReliable1_c()
    {
        Session_Number[0] = 6;
        Session_Number[1] = 5;
        Session_Number[2] = 5;
        Session_Number[3] = 7;
        Session_Number[4] = 6;
        Session_Number[5] = 4;
        Session_Number[6] = 4;
        Session_Number[7] = 7;
        Session_Number[8] = 5;
        Session_Number[9] = 4;
        Session_Number[10] = 4;
        Session_Number[11] = 5;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 1;
        MixedRe[2] = 0;
        MixedRe[3] = 0;
        MixedRe[4] = 0;
        MixedRe[5] = 1;
        MixedRe[6] = 0;
        MixedRe[7] = 1;
        MixedRe[8] = 0;
        MixedRe[9] = 0;
        MixedRe[10] = 1;
        MixedRe[11] = 1;
    }
    void GiveSessionIDMisMatchingUnReliable2_c()
    {
        Session_Number[0] = 4;
        Session_Number[1] = 5;
        Session_Number[2] = 4;
        Session_Number[3] = 5;
        Session_Number[4] = 6;
        Session_Number[5] = 6;
        Session_Number[6] = 4;
        Session_Number[7] = 4;
        Session_Number[8] = 7;
        Session_Number[9] = 7;
        Session_Number[10] = 5;
        Session_Number[11] = 5;

        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 0;
        MixedRe[2] = 0;
        MixedRe[3] = 0;
        MixedRe[4] = 1;
        MixedRe[5] = 0;
        MixedRe[6] = 0;
        MixedRe[7] = 1;
        MixedRe[8] = 0;
        MixedRe[9] = 1;
        MixedRe[10] = 1;
        MixedRe[11] = 1;

    }
    void GiveSessionLearningReliable()
    {
        Session_Number[0] = 1;
        Session_Number[1] = 2;
        Session_Number[2] = 2;
        Session_Number[3] = 4;
        Session_Number[4] = 6;
        Session_Number[5] = 3;
        Session_Number[6] = 4;
        Session_Number[7] = 7;
        Session_Number[8] = 5;
        Session_Number[9] = 7;
        Session_Number[10] = 3;
        Session_Number[11] = 5;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 1;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 1;
        MixedRe[5] = 1;
        MixedRe[6] = 1;
        MixedRe[7] = 1;
        MixedRe[8] = 1;
        MixedRe[9] = 1;
        MixedRe[10] = 1;
        MixedRe[11] = 1;
    }
    void GiveSessionLearningUnReliable()
    {
        Session_Number[0] = 1;
        Session_Number[1] = 2;
        Session_Number[2] = 2;
        Session_Number[3] = 4;
        Session_Number[4] = 6;
        Session_Number[5] = 3;
        Session_Number[6] = 4;
        Session_Number[7] = 7;
        Session_Number[8] = 5;
        Session_Number[9] = 7;
        Session_Number[10] = 3;
        Session_Number[11] = 5;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 0;
        MixedRe[2] = 0;
        MixedRe[3] = 0;
        MixedRe[4] = 1;
        MixedRe[5] = 0;
        MixedRe[6] = 0;
        MixedRe[7] = 1;
        MixedRe[8] = 0;
        MixedRe[9] = 1;
        MixedRe[10] = 1;
        MixedRe[11] = 1;
    }
    /*void GiveSessioIDUnReliable101()
    {
        Session_Number[0] = 1;
        Session_Number[1] = 3;
        Session_Number[2] = 5;
        Session_Number[3] = 3;
        Session_Number[4] = 4;
        Session_Number[5] = 2;
        Session_Number[6] = 5;
        Session_Number[7] = 1;
        Session_Number[8] = 2;
        Session_Number[9] = 4;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 0;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 0;
        MixedRe[5] = 1;
        MixedRe[6] = 0;
        MixedRe[7] = 0;
        MixedRe[8] = 0;
        MixedRe[9] = 1;
        
        
    }
    void GiveSessioIDUnReliable102()
    {
        Session_Number[0] = 2;
        Session_Number[1] = 4;
        Session_Number[2] = 3;
        Session_Number[3] = 1;
        Session_Number[4] = 5;
        Session_Number[5] = 2;
        Session_Number[6] = 1;
        Session_Number[7] = 5;
        Session_Number[8] = 3;
        Session_Number[9] = 4;
        ////////
        MixedRe[0] = 0;
        MixedRe[1] = 0;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 0;
        MixedRe[5] = 1;
        MixedRe[6] = 0;
        MixedRe[7] = 1;
        MixedRe[8] = 0;
        MixedRe[9] = 1;
        

    }
    void GiveSessioIDUnReliable103()
    {
        Session_Number[0] = 2;
        Session_Number[1] = 2;
        Session_Number[2] = 4;
        Session_Number[3] = 1;
        Session_Number[4] = 4;
        Session_Number[5] = 3;
        Session_Number[6] = 5;
        Session_Number[7] = 3;
        Session_Number[8] = 1;
        Session_Number[9] = 5;
        ////////
        MixedRe[0] = 0;
        MixedRe[1] = 1;
        MixedRe[2] = 0;
        MixedRe[3] = 0;
        MixedRe[4] = 1;
        MixedRe[5] = 0;
        MixedRe[6] = 0;
        MixedRe[7] = 1;
        MixedRe[8] = 1;
        MixedRe[9] = 1;
        

    }
    void GiveSessioIDUnReliable104()
    {
        Session_Number[0] = 1;
        Session_Number[1] = 4;
        Session_Number[2] = 2;
        Session_Number[3] = 3;
        Session_Number[4] = 5;
        Session_Number[5] = 2;
        Session_Number[6] = 4;
        Session_Number[7] = 3;
        Session_Number[8] = 1;
        Session_Number[9] = 5;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 0;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 0;
        MixedRe[5] = 0;
        MixedRe[6] = 1;
        MixedRe[7] = 0;
        MixedRe[8] = 0;
        MixedRe[9] = 1;
        
        

    }
    void GiveSessioIDUnReliable105()
    {
        Session_Number[0] = 4;
        Session_Number[1] = 2;
        Session_Number[2] = 4;
        Session_Number[3] = 1;
        Session_Number[4] = 5;
        Session_Number[5] = 2;
        Session_Number[6] = 1;
        Session_Number[7] = 5;
        Session_Number[8] = 3;
        Session_Number[9] = 3;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 1;
        MixedRe[2] = 0;
        MixedRe[3] = 1;
        MixedRe[4] = 0;
        MixedRe[5] = 0;
        MixedRe[6] = 0;
        MixedRe[7] = 1;
        MixedRe[8] = 0;
        MixedRe[9] = 1;
        

    }
    void GiveSessioIDReliable102()
    {
        Session_Number[0] = 4;
        Session_Number[1] = 2;
        Session_Number[2] = 4;
        Session_Number[3] = 1;
        Session_Number[4] = 5;
        Session_Number[5] = 2;
        Session_Number[6] = 1;
        Session_Number[7] = 5;
        Session_Number[8] = 3;
        Session_Number[9] = 3;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 1;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 1;
        MixedRe[5] = 1;
        MixedRe[6] = 1;
        MixedRe[7] = 1;
        MixedRe[8] = 1;
        MixedRe[9] = 1;
        
         

    }
    void GiveSessioIDReliable101()
    {
        Session_Number[0] = 1;
        Session_Number[1] = 3;
        Session_Number[2] = 4;
        Session_Number[3] = 3;
        Session_Number[4] = 5;
        Session_Number[5] = 5;
        Session_Number[6] = 4;
        Session_Number[7] = 2;
        Session_Number[8] = 2;
        Session_Number[9] = 1;
        ////////
        MixedRe[0] = 1;
        MixedRe[1] = 1;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 1;
        MixedRe[5] = 1;
        MixedRe[6] = 1;
        MixedRe[7] = 1;
        MixedRe[8] = 1;
        MixedRe[9] = 1;
        
         
        
    }
    void GiveSessioIDReliable12() //fisrt 12 trials of reliable session
    {
        int[] Re;
        Re = new int[5];
        int[] order;
        //int size = 12;
        order = new int[Total_trial];
        Re[0] = 2;//bump-1
        Re[1] = 3;//hole-2
        Re[2] = 2;//flat-3
        Re[3] = 3;//IllusionBump-4
        Re[4] = 2;//IllusionHole-5
        //fisrt 12 trials of reliable session one order
        //Nonillusion surface is 1 and illusion surface is 0 {NNI,NIN,INN,INI}={110,101,011,010}
        order[0] = 1;
        order[1] = 1;
        order[2] = 0;
        order[3] = 1;
        order[4] = 0;
        order[5] = 1;
        order[6] = 0;
        order[7] = 1;
        order[8] = 1;
        order[9] = 0;
        order[10] = 1;
        order[11] = 0;
        //fisrt 12 trials of reliable session second order for counter balance
        
        Random rnd = new Random();
        for(int ii=0;ii< Total_trial; ii++)
        {
            if (order[ii] == 1)//bump/hole/flat
            {
                int gh = Random.Range(1, 4);//session ID should be 1 or 2 or 3
                if (Re[gh-1]>0)
                {
                    Session_Number[ii] = gh;
                    Re[gh - 1]--;
                }
                else if(gh==1)
                {
                    if(Re[1] > 0)
                    {
                        Session_Number[ii] = 2;
                        Re[1]--;
                    }
                    else
                    {
                        Session_Number[ii] = 3;
                        Re[2]--;
                    }
                }
                else if (gh == 2)
                {
                    if (Re[0] > 0)
                    {
                        Session_Number[ii] = 1;
                        Re[0]--;
                    }
                    else
                    {
                        Session_Number[ii] = 3;
                        Re[2]--;
                    }
                }
                else if (gh == 3)
                {
                    if (Re[1] > 0)
                    {
                        Session_Number[ii] = 2;
                        Re[1]--;
                    }
                    else
                    {
                        Session_Number[ii] = 1;
                        Re[0]--;
                    }
                }

            }//bump/hole/flat
            else//illusion part
            {
                int gh = Random.Range(4, 6);//session ID should be 4 or 5
                if (Re[gh - 1] > 0)
                {
                    Session_Number[ii] = gh;
                    Re[gh - 1]--;
                }
                else if (gh == 4)
                {
                    Session_Number[ii] = 5;
                    Re[4]--;
                }
                else
                {
                    Session_Number[ii] = 4;
                    Re[3]--;
                }
            }
            MixedRe[ii] = 1;
        }
        
        
    }
    void GiveSessioIDReliable13()//second 13 trials of reliable session
    {
        int[] Re;
        Re = new int[5];
        int[] order;
        //int size = 12;
        order = new int[Total_trial];
        //fisrt 12 trials of reliable session one order
        Re[0] = 3;//bump-1
        Re[1] = 2;//hole-2
        Re[2] = 3;//flat-3
        Re[3] = 2;//IllusionBump-4
        Re[4] = 3;//IllusionHole-5
        //Nonillusion surface is 1 and illusion surface is 0 {NNI,NIN,INN,INI}={110,101,011,010}
        //Nonillusion surface is 1 and illusion surface is 0 {NNI,NIN,INN,INI}={110,101,011,010}
        order[0] = 0;
        order[1] = 1;
        order[2] = 0;
        order[3] = 1;
        order[4] = 1;
        order[5] = 0;
        order[6] = 1;
        order[7] = 0;
        order[8] = 1;
        order[9] = 1;
        order[10] = 0;
        order[11] = 1;
        order[12] = 1;
        //fisrt 13 trials of reliable session second order for counter balance
        
        Random rnd = new Random();
        for (int ii = 0; ii < Total_trial; ii++)
        {
            if (order[ii] == 1)//bump/hole/flat
            {
                int gh = Random.Range(1, 4);//session ID should be 1 or 2 or 3
                if (Re[gh - 1] > 0)
                {
                    Session_Number[ii] = gh;
                    Re[gh - 1]--;
                }
                else if (gh == 1)
                {
                    if (Re[1] > 0)
                    {
                        Session_Number[ii] = 2;
                        Re[1]--;
                    }
                    else
                    {
                        Session_Number[ii] = 3;
                        Re[2]--;
                    }
                }
                else if (gh == 2)
                {
                    if (Re[0] > 0)
                    {
                        Session_Number[ii] = 1;
                        Re[0]--;
                    }
                    else
                    {
                        Session_Number[ii] = 3;
                        Re[2]--;
                    }
                }
                else if (gh == 3)
                {
                    if (Re[1] > 0)
                    {
                        Session_Number[ii] = 2;
                        Re[1]--;
                    }
                    else
                    {
                        Session_Number[ii] = 1;
                        Re[0]--;
                    }
                }

            }//bump/hole/flat
            else//illusion part
            {
                int gh = Random.Range(4, 6);//session ID should be 4 or 5
                if (Re[gh - 1] > 0)
                {
                    Session_Number[ii] = gh;
                    Re[gh - 1]--;
                }
                else if (gh == 4)
                {
                    Session_Number[ii] = 5;
                    Re[4]--;
                }
                else
                {
                    Session_Number[ii] = 4;
                    Re[3]--;
                }
            }
            MixedRe[ii] = 1;
        }
    }*/
    void Start()
    {

        //print("bump[0]:   " + bump[0]);
        //print("bump[1]:   " + bump[1]);
        //Lida//putting markers in 1 mm, 2mm and 3 mm heights
        Flag_Markers_s = new bool[5];
        for (int i = 0; i < 5; i++)
            Flag_Markers_s[i] = true;
        streamInfo = new StreamInfo("Inlet", "Markers", 1, 0, LSL.channel_format_t.cf_int32, "Outlet");//(StreamName, StreamType, 3, Time.fixedDeltaTime * 1000, LSL.channel_format_t.cf_float32);
        outlet = new StreamOutlet(streamInfo);
        currentSample = new int[1];
        //Lida//putting markers in 1 mm, 2mm and 3 mm heights
        /*
        //Lida*******************************************
        //creating a plane in the position of the ReferencePlane for measuring distance between the the stylus and the reference plane 
        LRef_Plane = GameObject.Find("ReferencePlane");
        var filter1 = LRef_Plane.GetComponent<MeshFilter>();
        if (filter1 && filter1.mesh.normals.Length > 0)
            Lnormal1 = filter1.transform.TransformDirection(filter1.mesh.normals[0]);
        Lplane1 = new Plane(Lnormal1, LRef_Plane.transform.position);
        //Lida*********************************
        */
        
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
            FXID[ii] = HapticPlugin.effects_assignEffect(devices[ii].configName);//Lida//configName=Default Device
        }

        //marker = FindObjectOfType<LSLMarkerStream>();
        waitTime = (showtimer_dur + 12.0f);//??lida-used just here
        waitTimeA = (showtimer_dur + 13.0f);//??lida-used just here

        tracingAcc_path = new Vector2[6];
        tracingSpeed_path = new Vector2[6];
        tracingSpeed_path_tem = new Vector2[1000];
        touchAcc_path = new float[6];
        Vector2 speed_path_average = Vector2.zero;

        responsetimer = 0;
        showtimer = 0;
        //GameObject inputFieldGo = GameObject.Find("InputField");
        //InputField txt_Input = inputFieldGo.GetComponent<InputField>();
        welcome = GameObject.Find("Welcome");
        subId = GameObject.Find("SubID");
        subText = GameObject.Find("InputField");
        startbutton = GameObject.Find("OkButton");
        //subText = GameObject.Find("InputField");
        user_instruction = GameObject.Find("Massage_user");
        cone = GameObject.Find("cone");
        //session1 = GameObject.Find("Session1Bu").GetComponent<Button>();
        //session2 = GameObject.Find("Session2Bu").GetComponent<Button>(); //GameObject.Find("Session2Bu");
        YesBu = GameObject.Find("YesButton"); //NotsureBu = GameObject.Find("NotsureButton"); 
        FlatBu = GameObject.Find("FlatButton");
        //Lida//applyed in jing's code
        NoBu = GameObject.Find("NoButton");
        //lida
        //IsFlat = GameObject.Find("CanYouFeelaFlat");
        //IsNotFlat = GameObject.Find("CanYouFeelaNonFlat");
        //confidenttext = GameObject.Find("ConfidentText");
        //confidenttextNo = GameObject.Find("ConfidentText_marker_no");
        //confidenttextYes = GameObject.Find("ConfidentText_marker_yes");
        //slider_yes = GameObject.Find("Slider_yes");
        //slider_yesText = GameObject.Find("Slider_yesText");
        //slider_no = GameObject.Find("Slider_no");
        //slider_noText = GameObject.Find("Slider_noText"); //tryagainBu = GameObject.Find("tryButton");
        nextBu = GameObject.Find("nextButton");
        //doneText_session1 = GameObject.Find("DoneText_session1");
        //doneText_session2 = GameObject.Find("DoneText_session2");
        doneText_session_all = GameObject.Find("DoneText_session_all");
        //lida//trust scene
        trust_yes = GameObject.Find("trust_yes").GetComponent<Button>();
        trust_no = GameObject.Find("trust_no").GetComponent<Button>();
        trust_text = GameObject.Find("trust_text");
        system_res = GameObject.Find("system_res");
        timer_text = GameObject.Find("timer_text");
        //mytimer = GameObject.Find("mytimer");
        //lida//trust scene
        plane = GameObject.Find("Background"); 
        GameObject plane1 = GameObject.Find("Plane"); 
        plane1.GetComponent<MeshRenderer>().enabled = false;
        GameObject cover = GameObject.Find("Cover");
        if (preTest)
        {
            cover.GetComponent<MeshRenderer>().enabled = false; //Lida//background is white
        }
        else
        {
            cover.GetComponent<MeshRenderer>().enabled = true; //Lida//background is black
        }
        //////////////
        // instantiating the Session_Number
        Session_Number = new int[40];//[Total_trial];
        MixedRe = new int[40];//[Total_trial];
        /*trial_type[0] = Bump_trial;
        trial_type[1] = hole_trial;
        trial_type[2] = flat_trial;
        trial_type[3] = Nobump_trial;
        trial_type[4] = Nohole_trial;*/
        //int R1;
        //int j1;
        //j1 = Bump_trial + hole_trial + flat_trial + Nobump_trial + Nohole_trial;
        if (ReliableFlag)// || UnReliableFlag)
        {
            //GiveSessionLearningReliable();
            //GiveSessionIDMisMatchingReliable_c();
            GiveSessionIDMatchingReliable();
            //GiveSessioIDReliable101();//learning
            //beforeGiveSessioIDReliable101();
        }
        else if(UnReliableFlag)
        {
            //GiveSessionLearningUnReliable();
            //GiveSessionIDMisMatchingUnReliable2_c();
            GiveSessionIDMatchingUnReliable2();
            //GiveSessioIDUnReliable101();//learning
            //beforeGiveSessioIDUnReliable101();
        }
        else//mixed
        {
            /*R1 = 6;//total 5 sessionId
            int k1= Total_trial - j1;//128-32=96
            for (int i = 0; i <= k1; i =(i+ j1))
            GiveSessioID(i1,k1,R1);//0 to
            */
            //GiveSessioIDMix1();

        }
        //for unrelaible session

        bump = new int[2];
        bump[0] = 5;//# relaible feedback for bump
        bump[1] = 5;//# unrelaible feedback
        hole = new int[2];
        hole[0] = 5;//# relaible feedback
        hole[1] = 5;//# unrelaible feedback
        flat = new int[2];
        flat[0] = 5;//# relaible feedback
        flat[1] = 5;//# unrelaible feedback
        //for unrelaible session
        // instantiating the Session_Number
        //print("start--i_trial1    " + i_trial1);
        //Session_Number[0] = 1;
        //Session_Number[1] = 2;
        //Session_Number[2] = 3;
        //Session_Number[3] = 3;
        //Session_Number[4] = 1;
        //Session_Number[5] = 2;
        //Session_Number[6] = 3;
        //Session_Number[7] = 2;
        //Session_Number[8] = 1;
        sessionID = Session_Number[i_trial1];
        //visual_sur_bump = GameObject.Find("0.3e(-x__2div2__2)");//("Bump");//
        //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
        //visual_sur_bump.transform.position = new Vector3(0.0f, 0.0f, 0.5f);
        //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");//("Hole");// 
        //visual_sur_flat = GameObject.Find("F_Surface");
        //print("sinH--start--sessionID:    " + sessionID);
        //Session_Number[0] = 3;
        //Session_Number[1] = 2;
       // Session_Number[2] = 1;
        //sessionID = Session_Number[0];
        //var scene = SceneManager.GetActiveScene();
        //var sceneRoots = scene.GetRootGameObjects();
        /*myObjects = new GameObject[3];
        myObjects[0]= GameObject.Find("Bump");
        myObjects[1] = GameObject.Find("Hole");
        myObjects[2] = GameObject.Find("Falt");*/

        visual_sur = GameObject.Find("MixedSurface");
        myboxCollider = visual_sur.GetComponent<BoxCollider>();
        visual_sur.transform.position = new Vector3(50.0f, 0.0f, 20.0f);
        if (sessionID == 1)//bump
        {
           // visual_sur_hole.transform.gameObject.tag = "Untagged";
            //visual_sur_hole.SetActive(false);
            //visual_sur_flat.transform.gameObject.tag = "Untagged";
            //visual_sur_flat.SetActive(false);
            //foreach (var root in sceneRoots)
            /*visual_sur_bump = myObjects[0].transform.Find("0.3e(-x__2div2__2)").gameObject;
            print("start:  " + visual_sur_bump.name);*/
            //Vector3 newScale = new Vector3(temXZ, -temXZ, temXZ); //i_trial1], temXZ);
            //visual_sur_bump.transform.localScale = newScale;
            //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
            visual_sur.transform.position = new Vector3(50.0f, 0.0f, 13.0f);//first task: (50.0f, 0.0f, 0.5f);// three big surfaces//(0.0f, 0.0f, 0.5f); smallbumphole//(-10.0f, 0.0f, 0.5f); //
            //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            myboxCollider.center= new Vector3(0.04f, 0.0f, 0.0015f); //(0.0f, 0.15f, 0.0f); //(10.0f, 0.0f, 0.15f); //(0.04f, 0.0f, 0.0015f);
            myboxCollider.size= new Vector3(0.02f, 0.02f, 0.003f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.3f);//
            flag_concave = true;
            flag_convex = false;
            flag_flat = false;
            flag_flat_b = false;
            flag_flat_h = false;
            flag_bump_f = false;
            flag_hole_f = false;
            //visual_sur_bump.transform.gameObject.tag = "Touchable";
            //visual_sur_bump.SetActive(true);
            //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, 100f);
            //visual_sur_hole = GameObject.Find("-0.3e(-x__2div2__2)");
        }
        else if (sessionID == 2)//hole
        {
            //visual_sur = GameObject.Find("-0.3e(-x__2div2__2)");
            //visual_sur_hole.transform.gameObject.tag = "Touchable";
            //visual_sur_hole.SetActive(true);
            //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, 0.5f);
            //Vector3 newScale = new Vector3(temXZ, -temXZ, temXZ); //i_trial1], temXZ);
            //visual_sur_bump.transform.localScale = newScale;
            //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 90, 90);
            //visual_sur_bump.transform.rotation= Quaternion.Euler(90, 90, 90);
            //double yy = visual_sur_bump.transform.localScale.y + 0.0001;
            //newScale = new Vector3(temXZ, 1, temXZ); //i_trial1], temXZ);
            //visual_sur_bump.transform.localScale = newScale;
            //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
            flag_convex = true;
            flag_concave = false;
            flag_flat = false;
            flag_flat_b = false;
            flag_flat_h = false;
            flag_bump_f = false;
            flag_hole_f = false;
            //visual_sur_bump = GameObject.Find("0.3e(-x__2div2__2)");
            visual_sur.transform.position = new Vector3(25.0f, 0.0f, 13.0f); //first task: (25.0f, 0.0f, 0.4f);//(25.0f, 0.0f, 0.4f); //(0.0f, 0.0f, 0.4f); //
            //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            myboxCollider.center = new Vector3(0.02f, 0.0f, -0.0015f); //(-25.0f, -0.15f, 0.0f); //(0.0f, 0.0f, -0.15f); //
            myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.3f); //
            //visual_sur_bump.transform.gameObject.tag = "Untagged";
            //visual_sur_bump.SetActive(false);
            //visual_sur_flat = GameObject.Find("F_Surface");
            //visual_sur_flat.transform.gameObject.tag = "Untagged";
            //visual_sur_flat.SetActive(false);
            //visual_sur_flat.transform.gameObject.tag = "Untagged";
            //visual_sur_flat.SetActive(false);
        }
        else if (sessionID == 3)//flat
        {
            //visual_sur = GameObject.Find("F_Surface");
            //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
            //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ); //i_trial1], temXZ);
            //visual_sur_bump.transform.localScale = newScale;
            //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
            flag_convex = false;
            flag_concave = false;
            flag_flat = true;
            flag_flat_b = false;
            flag_flat_h = false;
            flag_bump_f = false;
            flag_hole_f = false;
            //visual_sur_flat.transform.gameObject.tag = "Touchable";
            //visual_sur_flat.SetActive(true);
            //visual_sur_bump.transform.gameObject.tag = "Untagged";
            //visual_sur_bump.SetActive(false);
            //visual_sur_hole.transform.gameObject.tag = "Untagged";
            //visual_sur_hole.SetActive(false);
            //visual_sur_flat.transform.gameObject.tag = "Touchable";
            //visual_sur_bump.transform.gameObject.tag = "Untagged";
            visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(0.0f, 0.0f, 0.4f); //
            //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.0f, 0.15f, 0.0f); //(0.0f, 0.0f, -0.15f);//
            myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.001f); //
            //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, 100f);
            //visual_sur_hole.transform.gameObject.tag = "Untagged";
            //visual_sur_bump.transform.gameObject.tag = "Untagged";
            //visual_sur_bump.SetActive(false);
            //visual_sur_hole.SetActive(false);
        }
        else if (sessionID == 4)//flat
        {
            //visual_sur = GameObject.Find("F_Surface");
            //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
            //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ); //i_trial1], temXZ);
            //visual_sur_bump.transform.localScale = newScale;
            //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
            flag_convex = false;
            flag_concave = false;
            flag_flat = false;
            flag_flat_b = true;
            flag_flat_h = false;
            flag_bump_f = false;
            flag_hole_f = false;
            //visual_sur_flat.transform.gameObject.tag = "Touchable";
            //visual_sur_flat.SetActive(true);
            //visual_sur_bump.transform.gameObject.tag = "Untagged";
            //visual_sur_bump.SetActive(false);
            //visual_sur_hole.transform.gameObject.tag = "Untagged";
            //visual_sur_hole.SetActive(false);
            //visual_sur_flat.transform.gameObject.tag = "Touchable";
            //visual_sur_bump.transform.gameObject.tag = "Untagged";
            visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(0.0f, 0.0f, 0.4f); //
            //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.0f, 0.15f, 0.0f); //(0.0f, 0.0f, -0.15f);//
            myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.001f); //
            //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, 100f);
            //visual_sur_hole.transform.gameObject.tag = "Untagged";
            //visual_sur_bump.transform.gameObject.tag = "Untagged";
            //visual_sur_bump.SetActive(false);
            //visual_sur_hole.SetActive(false);
        }
        else if (sessionID == 5)//flat
        {
            //visual_sur = GameObject.Find("F_Surface");
            //visual_sur_bump.transform.localEulerAngles = new Vector3(90, 180, 0);
            //Vector3 newScale = new Vector3(temXZ, 0.00001f, temXZ); //i_trial1], temXZ);
            //visual_sur_bump.transform.localScale = newScale;
            //cone.transform.position = new Vector3(-2.8f, 0f, 0f);
            flag_convex = false;
            flag_concave = false;
            flag_flat = false;
            flag_flat_b = false;
            flag_flat_h = true;
            flag_bump_f = false;
            flag_hole_f = false;
            //visual_sur_flat.transform.gameObject.tag = "Touchable";
            //visual_sur_flat.SetActive(true);
            //visual_sur_bump.transform.gameObject.tag = "Untagged";
            //visual_sur_bump.SetActive(false);
            //visual_sur_hole.transform.gameObject.tag = "Untagged";
            //visual_sur_hole.SetActive(false);
            //visual_sur_flat.transform.gameObject.tag = "Touchable";
            //visual_sur_bump.transform.gameObject.tag = "Untagged";
            visual_sur.transform.position = new Vector3(0.0f, 0.0f, 13.0f); //first task: (0.0f, 0.0f, 0.4f);//(0.0f, 0.0f, 0.4f); //
            //visual_sur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            myboxCollider.center = new Vector3(0.0f, 0.0f, 0.0f); //(0.0f, 0.15f, 0.0f); //(0.0f, 0.0f, -0.15f);//
            myboxCollider.size = new Vector3(0.02f, 0.02f, 0.0001f); //(25.0f, 0.3f, 25.0f); //(10.0f, 10.0f, 0.001f); //
            //visual_sur_hole.transform.position = new Vector3(0.0f, 0.0f, 100f);
            //visual_sur_hole.transform.gameObject.tag = "Untagged";
            //visual_sur_bump.transform.gameObject.tag = "Untagged";
            //visual_sur_bump.SetActive(false);
            //visual_sur_hole.SetActive(false);
        }
        else if (sessionID == 6)//bump-flat
        {
            flag_concave = false;
            flag_convex = false;
            flag_flat = false;
            flag_flat_b = false;
            flag_flat_h = false;
            flag_bump_f = true;
            flag_hole_f = false;
            visual_sur.transform.position = new Vector3(50.0f, 0.0f, 13.0f);//first task: (50.0f, 0.0f, 0.5f);
            myboxCollider.center = new Vector3(0.04f, 0.0f, 0.0015f);
            myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f);
        }
        else if (sessionID == 7)//hole-flat
        {
            flag_convex = false;
            flag_concave = false;
            flag_flat = false;
            flag_flat_b = false;
            flag_flat_h = false;
            flag_bump_f = false;
            flag_hole_f = true;
            visual_sur.transform.position = new Vector3(25.0f, 0.0f, 13.0f);//first task: (25.0f, 0.0f, 0.4f);
            myboxCollider.center = new Vector3(0.02f, 0.0f, -0.0015f);
            myboxCollider.size = new Vector3(0.02f, 0.02f, 0.003f);
        }
        /*else if (sessionID == 5)
        {
            visual_sur = GameObject.Find("-0.3e(-x__2div2__2)");
            visual_sur.transform.localEulerAngles = new Vector3(0, 90, 270);
            cone.transform.position = new Vector3(0f, 2.8f, 0f);
            flag_convex = true;
        }
        else if (sessionID == 2)
        {
            visual_sur = GameObject.Find("0.3e(-x__2div2__2)");
            visual_sur.transform.localEulerAngles = new Vector3(0, 90, 270);
            cone.transform.position = new Vector3(0f, 2.8f, 0f);
            flag_convex = true;
        }*/

        //lida//deleted
        /*if (sessionID == 1)
        {
            //print("0.3e(-x__2div2__2)*********************");
            visual_sur = GameObject.Find("0.3e(-x__2div2__2)"); //concave
        }
        if (sessionID == 2)
        {
            //print("-0.3e(-x__2div2__2)*********************");
            visual_sur = GameObject.Find("-0.3e(-x__2div2__2)"); //convex
        }
        else if (sessionID == 3)
        {
            //print("Flat_Surface*********************");
            visual_sur = GameObject.Find("Flat_Surface");
        }*/
        //lida//deleted
        //if (sessionID < 3) { visual_sur = GameObject.Find("sinx_256256505002"); }
        //if (sessionID > 2) { visual_sur_34 = GameObject.Find("sinx05siny05_256256505002"); }
        //Vector3 eulerAngles = visual_sur.transform.rotation.eulerAngles; Debug.Log("transform.rotation angles x: " + eulerAngles.x + " y: " + eulerAngles.y + " z: " + eulerAngles.z);
        user_instruction.SetActive(false);
        //session1.gameObject.SetActive(false);
        //session2.gameObject.SetActive(false);
        timer_text.SetActive(false);
        plane.SetActive(false);
        //mytimer.SetActive(false);
        cone.SetActive(false);
        //CameraSwitcher.FindObjectOfType<CameraSwitcher>().SwitchCamera();
        YesBu.SetActive(false);
        FlatBu.SetActive(false);
        NoBu.SetActive(false); //Lida-jing's//NotsureBu.SetActive(false); 
        //IsFlat.SetActive(false);
        //IsNotFlat.SetActive(false);
        //confidenttext.SetActive(false);
        //confidenttextNo.SetActive(false);
        //confidenttextYes.SetActive(false);
        //slider_yes.SetActive(false);
        //slider_yesText.SetActive(false);
        //slider_no.SetActive(false);
        ///slider_noText.SetActive(false);
        nextBu.SetActive(false); //tryagainBu.SetActive(false); 
        //session1.gameObject.SetActive(false);
        //session2.gameObject.SetActive(false);
        //doneText_session1.SetActive(false);
        //doneText_session2.SetActive(false);
        doneText_session_all.SetActive(false);
        //lida//trust scene
        trust_yes.gameObject.SetActive(false);
        trust_no.gameObject.SetActive(false);
        trust_text.SetActive(false);
        system_res.SetActive(false);
        //lida//trust scene
        //doneText_session2.SetActive(false);
        //visual_sur_A1.SetActive(false); visual_sur_A2.SetActive(false);
    }
    void FixedUpdate()
    {
        //lidamarker
        //sending markers in the start of each of the 3r paths (hapticPlugin)
        for (int i = 0; i < 2; i++)
            if (devices[0].LSLMarker1[i])
            {
                devices[0].LSLMarker1[i] = false;
                currentSample[0] = i + 1; //lg1;
                outlet.push_sample(currentSample);
            }
        //sending markers in the specified height of cone position (pathFollower)
        //if (sessionID != 3)//(sessionID == 1) || (sessionID == 2))//when the surface is a bump or a hole not a flat
            for (int i = 0; i < 5; i++)//each path hase 5 markers
                if (PathFollower.shared)
                    if (PathFollower.shared.LSLMarkerCone1[i])
                    {
                        PathFollower.shared.LSLMarkerCone1[i] = false;
                        currentSample[0] = i + 301;
                        outlet.push_sample(currentSample);
                    }
        //lidamarker
        //Lida//timer before a trial
        //if (cone)
        //{
            //print("yellowwwwwwwwwwwwww");
            if (plane.gameObject.activeSelf)//(cone.gameObject.activeSelf && (cone.GetComponent<Renderer>().material.color == Color.black))
            {
                //print("blackkkk");
                waittimer += Time.deltaTime; //print("aaaaaaaaaaa"); print(PathFollower.shared.pathCreator.path.length); print(PathFollower.shared.distanceTravelled);
                                             // Check if we have reached beyond 2 seconds.Subtracting two is more accurate over time than resetting to zero.
                if (waittimer > 1.0f)
                {
                    waittimer = 0.0f;
                    timer_text.SetActive(false);
                    plane.SetActive(false);
                    //cone.GetComponent<Renderer>().material.color = Color.yellow;
                    //print("yellowwwwwwwwwwwwww");
                    // Remove the recorded 2 seconds.
                    //waittimer = waittimer - ResponsewaitTime;
                    Time.timeScale = scrollBar;
                }
            }
        //}//Lida//timer before a trial
    }
    void Update()
    {
        
        //Lida//timer for the bump/hole/flat buttons
        if (YesBu)
        {
            if (YesBu.gameObject.activeSelf)
            {
                responsetimer += Time.deltaTime; //print("aaaaaaaaaaa"); print(PathFollower.shared.pathCreator.path.length); print(PathFollower.shared.distanceTravelled);
                                                 // Check if we have reached beyond 2 seconds.Subtracting two is more accurate over time than resetting to zero.
                if (responsetimer > ResponsewaitTime)
                {
                    YesBu.GetComponent<Image>().color = Color.red;
                    FlatBu.GetComponent<Image>().color = Color.red;
                    NoBu.GetComponent<Image>().color = Color.red;
                    overdely = true;
                    // Remove the recorded 2 seconds.
                    //responsetimer = responsetimer - ResponsewaitTime;
                    Time.timeScale = scrollBar;
                }
                if (overdely && (responsetimer > (ResponsewaitTime + 2.0f)))
                {
                    NoSelection();
                }
            }//Lida//timer for the bump/hole/flat buttons
        }
        //Lida//timer for the Yes/No buttons
        if (trust_yes)
        {
            if (trust_yes.gameObject.activeSelf)
            {
                responsetimer += Time.deltaTime; //print("aaaaaaaaaaa"); print(PathFollower.shared.pathCreator.path.length); print(PathFollower.shared.distanceTravelled);
                                                 // Check if we have reached beyond 2 seconds.Subtracting two is more accurate over time than resetting to zero.
                if (responsetimer > ResponsewaitTime)
                {
                    trust_yes.GetComponent<Image>().color = Color.red;
                    trust_no.GetComponent<Image>().color = Color.red;
                    overdely = true;
                    // Remove the recorded 2 seconds.
                    //responsetimer = responsetimer - ResponsewaitTime;
                    Time.timeScale = scrollBar;
                }
                if (overdely && (responsetimer > (ResponsewaitTime + 2.0f)))
                {
                    NoSelectionYesNo();
                }
            }//Lida//timer for the Yes/No buttons
        }
        //lidamarker
        //sending markers in the start of each of the 4 paths (hapticPlugin)
        /*for (int i = 0; i < 4; i++)
            if (devices[0].LSLMarker1[i])
            {
                devices[0].LSLMarker1[i] = false;
                currentSample[0] = i + 1; //lg1;
                outlet.push_sample(currentSample);
            }
        //sending markers in the specified height of cone position (pathFollower)
        if ((sessionID == 1) || (sessionID == 2))//when the surface is a bump or a hole not a flat
            for (int i = 0; i < 5; i++)//each path hase 5 markers
                if (PathFollower.shared)
                if (PathFollower.shared.LSLMarkerCone1[i])
                {
                    PathFollower.shared.LSLMarkerCone1[i] = false;
                    currentSample[0] = i + 301;
                    outlet.push_sample(currentSample);
                }*/
        //lidamarker

        frame_before = System.DateTime.Now;//lida//get the current time
        /*if (oneFinshed)
        {             
             oneFinshed = false; 
            //   answEnabled = true;//AnsewshowHide();                  
        }*/

        // Find the pointer to the collider that defines the "zone". 
        Collider collider = gameObject.GetComponent<Collider>();
        /*Collider collider;
        //print("sin--update--sessionID:    " + sessionID);
        if (sessionID == 1)//bump
        {
            collider = visual_sur_bump.GetComponent<Collider>();
            //print("Bumpppppppppppppppppppppppppppppp");
        }
        else if (sessionID == 2)//hole
        {
            collider = visual_sur_hole.GetComponent<Collider>();
        }
        else //if (sessionID == 3)//flat
        {
            collider = visual_sur_flat.GetComponent<Collider>();
        }*/
        if (collider == null)
        {
            Debug.LogError("This Haptic Effect Zone requires a collider");
            return;
        }
        //int M = 0;
        // if (hlTouchModel == HLTOUCH_MODEL.HL_CONSTRAINT)
        //M = 1;
        //HapticPlugin.shape_constraintSettings(gameObject.GetInstanceID(), 1, snapDistance);
        // Update the World-Space vectors
        focusPointWorld = transform.TransformPoint(stylPosition);
        //lida//direction change
        //directionWorld =  transform.TransformDirection(Direction);
        //lida//direction change

        // Update the effect seperately for each haptic device.
        for (int ii = 0; ii < devices.Length; ii++)
        {
            HapticPlugin device = devices[ii];
            //print("devices.Length  " + devices.Length);
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
            Vector3 StylusPos = device.stylusPositionWorld; //World Coordinates//HapticEffect
            //print("StylusPos"+StylusPos);//lida//in flat it prints numbers
            Vector3 CP = collider.ClosestPoint(StylusPos);  //World Coordinates//HapticEffect
                                                            //if ((PathFollower.shared.started) & (PathFollower.shared.distanceTravelled == 0))
                                                            //{
                                                            //    previous_path = stylPosition;
                                                            //}
            /*bool flag_button = false;
            if (trust_yes.gameObject.activeSelf || YesBu.gameObject.activeSelf || nextBu.gameObject.activeSelf || doneText_session_all.activeSelf)
                flag_button = true;*/
            if(cone.gameObject.activeSelf)//if(!flag_button)
            {
                if (save_previous_state1 && !PathFollower.shared.started)//Lida//finishing each of the four paths//started becomes true by pressing next button and every 5 frames in judge_distance if it was false
                {
                    //Lida//putting markers in 1 mm, 2mm and 3 mm heights
                    for (int i = 0; i < 5; i++)
                        Flag_Markers_s[i] = true;
                    //Lida//putting markers in 1 mm, 2mm and 3 mm heights
                    //print("Lida SHE-stylPosition " + stylPosition);//print the stylus position but I do not know where maybe at the end of the each path
                    //print("StylusPos"+StylusPos);//lida//in flat it prints numbers
                    //print("Path endingg: " + (path_show.path_show_instace.next_index - 1));//lida// writting the path number
                                                                                           //lida//marker//here//sinHEffect.FindObjectOfType<sinHEffect>().marker.Write((path_show.path_show_instace.next_index - 1 + 10));//lida//path number+10
                                                                                           //int lg1;
                                                                                           //lg1 = path_show.path_show_instace.next_index - 1 + 10;
                    currentSample[0] = path_show.path_show_instace.next_index - 1 + 10; //lg1;
                    //print(currentSample[0]);
                    outlet.push_sample(currentSample);
                    //lida//marker
                    eeg_decision_be = System.DateTime.Now;
                    cur_time = System.DateTime.Now;
                    dur_cur_time = cur_time.Subtract(each_before); //lida-start time of each block//cur_time.Subtract(start_each_path_time);//lida//time duration of each block
                    dur_ref_time1 = cur_time.Subtract(all_trial_before);//lida-start time of all blocks// cur_time.Subtract(start_ref_time); //lida//time duration of whole blocks////lida//start_ref_time: we are in the first path of 4 paths when the cone start to move//used for calculating the whole spending time doing an experimnets (4 paths)
                    string text = "                           ~ " + cur_time.ToString("HH:mm:ss.fff") + " [ " + dur_cur_time.TotalSeconds.ToString("0.000") + " ][ " + dur_ref_time1.TotalSeconds.ToString("0.000") + " ]";// + System.Environment.NewLine
                    string[] lines = { eeg_text, click_text, text };
                    File.AppendAllLines(Path.Combine(docPath, out_result), lines);
                    File.AppendAllLines(Path.Combine(docPath, marker_result), lines);

                    finishedpathFlag = 1;
                    save_previous_state1 = PathFollower.shared.started;//lida//make"save_previous_state1" false
                                                                       //string lines11111 = PathFollower.shared.started + " == " + PathFollower.shared.distanceTravelled.ToString() + " == " + Mathf.Round(PathFollower.shared.distanceTravelled).ToString();
                                                                       //string lines11112 = "=========";
                                                                       //string[] liness1 = { lines11111, lines11112 };
                                                                       //File.AppendAllLines(Path.Combine(docPath_mid, out_result_mid), liness1);
                    for (int i = 1; i < touchNumber_path; i++)
                    {
                        speed_path_average += tracingSpeed_path_tem[i];
                    }
                    speed_path_average = speed_path_average / touchNumber_path;
                    tracingSpeed_path[path_show.path_show_instace.next_index - 2] = speed_path_average;
                    if ((path_show.path_show_instace.next_index - 2) < 2) //if ((path_show.path_show_instace.next_index - 2) < 2)//lida//horizaltal path accuracy
                    {
                        //YesBu.SetActive(true);
                        //print("enddddd");
                        tracingAcc_path[path_show.path_show_instace.next_index - 2].x = (float)(tracingNumber_path * 1.0 / frameNumber_path);
                    }
                    else//lida//vertical path accuracy
                    {
                        //print("enddddd");
                        tracingAcc_path[path_show.path_show_instace.next_index - 2].y = (float)(tracingNumber_path * 1.0 / frameNumber_path);
                    }
                    touchAcc_path[path_show.path_show_instace.next_index - 2] = (float)(touchNumber_path * 1.0 / frameNumber_path);
                    tracingNumber_path = 0;
                    frameNumber_path = 0; //Debug.Log(tracingAcc_path[path_show.path_show_instace.next_index - 2].x);
                    touchNumber_path = 0;
                    finishedpathFlag = 0;
                }
                if(PathFollower.shared)
                if ((PathFollower.shared.started) && (PathFollower.shared.distanceTravelled < PathFollower.shared.pathCreator.path.length))//Lida//after starting the first path and before finishing it
                {
                    //print("enddddd");
                    //print("StylusPos"+StylusPos);//lida//in flat it prints numbers
                    //print("Lida SHE-stylPosition " + stylPosition);//lida//print the stylus position when the cone is green or red (not yellow) and it is moving
                    if (touchNumber_path > 10 && (HapticHand.FindObjectOfType<HapticHand>().buttonHoldDown) && clickflg)//lida////buttonHoldDown is correct when This is a "click" if we're pressed now, but weren't last frame.
                    {//lida//touchNumber_path becomes 0 in changescale() which is called in Next
                     //print("endddddddddddddddddd");// it does not print anything
                     //print("Lida SHE-stylPosition " + stylPosition);//lida//print the stylus position when the cone is green or red (not yellow) and it is moving
                     //lida//marker//here//marker.Write(7);
                     //currentSample[0] = "1";// "7";
                     //outlet.push_sample(currentSample);
                     //lida//marker
                        clikcur_time = System.DateTime.Now;
                        dur_clickcur_time = clikcur_time.Subtract(each_before); //lida-start time of each block// clikcur_time.Subtract(start_each_path_time);
                        click_text = "                           click @ " + clikcur_time.ToString("HH:mm:ss.fff") + "[ " + dur_clickcur_time.TotalSeconds.ToString("0.000") + " ]";// + System.Environment.NewLine
                                                                                                                                                                                  //string[] lines = { text }; File.AppendAllLines(Path.Combine(docPath, out_result), lines);
                        clickflg = false;
                    }
                    if ((path_show.path_show_instace.next_index == 1) & (i_trial1 == 0))//Lida//saving the start time of each block in first trial//after starting the first path and before finishing it
                    {
                        //print("enddddd1");
                        all_trial_before = System.DateTime.Now;

                    }
                    if (path_show.path_show_instace.next_index == 1)//Lida///saving the start time of each block//after starting the first path and before finishing it
                    {
                        //print("enddddd2");
                        each_before = System.DateTime.Now;
                    }
                    frameNumber = frameNumber + 1;
                    frameNumber_path = frameNumber_path + 1;
                    if (is_touching)//Lida//after starting the first path and before finishing it
                    {
                        //print("enddddd3");
                        touchNumber = touchNumber + 1;
                        touchNumber_path = touchNumber_path + 1;
                        //print("touchNumber_path:   " + touchNumber_path);
                        //print("stylPosition.x"+stylPosition);//lida
                        //print("StylusPos"+StylusPos);//lida//in flat it prints numbers
                        Vector2 tracingSpeed = (stylPosition - previous) / Time.deltaTime;
                        //if(tracingSpeed.x>0.0)
                        //print("tracingSpeed.x" );
                        previous = stylPosition;
                        Vector2 ctracingSpeed = UpdateParameters_Velocity(tracingSpeed.x, tracingSpeed.y);
                        tracingSpeed = ctracingSpeed;
                        string lines1110 = "The speed at touchframe " + touchNumber_path.ToString() + " is " + tracingSpeed.ToString();
                        string[] lines = { lines1110 };
                        // Append new lines of text to the file
                        File.AppendAllLines(Path.Combine(docPath_mid, out_result_mid), lines);
                        tracingSpeed_path_tem[touchNumber_path - 1] = new Vector2(Mathf.Abs(tracingSpeed.x), Mathf.Abs(tracingSpeed.y));
                        //print("tracingSpeed.x" + tracingSpeed.x);
                        //print("tracingSpeed.x" + tracingSpeed.x);
                    }
                    save_previous_state1 = PathFollower.shared.started;//lida//make"save_previous_state1" true
                }
                else
                {
                    //print("enddddd");//it does not print anything
                    if (PathFollower.shared.distanceTravelled > PathFollower.shared.pathCreator.path.length)
                    {
                        //print("enddddd");//Lida//after starting the first path and before finishing it

                        
                        showtimer += Time.deltaTime;

                        touchAcc = (float)touchNumber / frameNumber;
                        tracingAcc.x = tracingNumber / frameNumber;
                        finishedpathFlag = 1;
                        //visual_sur.transform.gameObject.tag = "Untagged";
                        //Lida//in jing's code this is applyed
                        //cone.SetActive(false);
                        //PathFollower.shared.all_end = false; 
                        //lida
                        save_previous_state1 = PathFollower.shared.started;

                        /*switch (session_flg)
                        {
                            case "FF":
                                YesBu.SetActive(false); FlatBu.SetActive(false); NoBu.SetActive(false);//Lida-jing's//NotsureBu.SetActive(false); 
                                //IsFlat.SetActive(false); IsNotFlat.SetActive(false);
                                user_instruction.SetActive(false); break;
                            default:
                                if (yes_flag == false)
                                {
                                    //Lida//in jing's code this is applyed-adding the button
                                    /////here
                                    //YesBu.SetActive(true);

                                   // if (current_session == 1)
                                    //{ IsFlat.SetActive(true); }
                                    //if (current_session == 2)
                                    //{ IsNotFlat.SetActive(true); }
                                    //here

                                    //Lida
                                    decision_before = System.DateTime.Now;
                                    yes_flag = true;
                                }
                                //else
                                //{
                                   // YesBu.SetActive(false);
                                   // NoBu.SetActive(false);
                                    //confidenttext.SetActive(false); confidenttextNo.SetActive(false); confidenttextYes.SetActive(false);
                                    //slider_yes.SetActive(false); slider_yesText.SetActive(false);
                                   // slider_no.SetActive(false); slider_noText.SetActive(false); nextBu.SetActive(false);
                                    //IsFlat.SetActive(false); IsNotFlat.SetActive(false); break;
                               // }
                                break;
                        }*/
                        /*if ((session_flg_1) | (session_flg_2))
                        {
                            YesBu.SetActive(false);
                            NoBu.SetActive(false);
                            Canyoufeelaflat.SetActive(false);
                        }
                        else
                        {
                            YesBu.SetActive(true);
                            NoBu.SetActive(true);
                            Canyoufeelaflat.SetActive(true);
                        }*/
                        if (PathFollower.shared.all_end)//Lida//finishing one block (all 4 paths)
                        {
                            //print("enddddd");
                            /*if (current_session == 1)
                            {
                                //   nextBu.transform.localPosition = new Vector3(cone.transform.localPosition.x * 50, 0.0f, 0.0f);//Canvas.transform.position.x + 
                            }
                            if (current_session == 2)
                            {
                                //    nextBu.transform.localPosition = new Vector3(0.0f, cone.transform.localPosition.y * 50, 0.0f);//
                            }*/
                            //before//lida//false
                            cone.SetActive(false); //Thread.Sleep(500); 
                                CameraSwitcher.FindObjectOfType<CameraSwitcher>().SwitchCamera();
                                nextBu.SetActive(true); PathFollower.shared.all_end = false;
                            visual_sur.transform.position = new Vector3(visual_sur.transform.position.x, 0.0f, 20.0f);
                            }
                        //nextBu.SetActive(true); 
                        //slider_up.SetActive(true); slider_upText.SetActive(true);
                        //ground.SetActive(true);
                        //slider_do.SetActive(true); slider_doText.SetActive(true);
                    }
                }
                if (PathFollower.shared)
                if (PathFollower.shared.distanceTravelled >= PathFollower.shared.pathCreator.path.length)//Lida//after starting the first path and before finishing it
                {
                    //print("enddddd");
                    PathFollower.shared.started = false; //save_previous_state = false;//oneFinshed = true; 
                }
            }//if (!trust_yes.gameObject.activeSelf || !YesBu.gameObject.activeSelf || !nextBu.gameObject.activeSelf || !doneText_session_all.gameObject.activeSelf)
            devicePoint[ii] = CP;//HapticEffect
            delta[ii] = (CP - StylusPos).magnitude;//HapticEffect// CP = collider.ClosestPoint(StylusPos); 
                                                   //print("StylusPos" + StylusPos);
                                                   //if (delta[ii] <= Mathf.Epsilon)
                                                   // print("yesssss");
                                                   //else
                                                   // print("nooo");
                                                   //print("delta[ii]"+ delta[ii]);
                                                   //If the stylus is within the Zone, The ClosestPoint and the Stylus point will be identical.
            if (delta[ii] <= Mathf.Epsilon)
            {
                inTheZone[ii] = true;

                double Mag = Magnitude;
                if (device.isInSafetyMode())
                    Mag = 0;
                stylusXX = StylusPos.x;//stylusPositionRaw.x;//proxyPositionRaw
                //print("StylusPos.x" + StylusPos.x);
                stylusYY = StylusPos.y;
                stylusZZ = StylusPos.z;
                Vector3 cPosition = UpdateParameters_Position(Mag, stylPosition, stylusXX, stylusYY, stylusZZ);
                //print("stylPosition" + stylPosition);
                stylPosition = cPosition;
                //print
                //print("distance-SHE   " + plane1.GetDistanceToPoint(stylPosition));//Lida//printing the distance between the curved surface and the a plane in the background
                //if(!(PathFollower.FindObjectOfType<PathFollower>().path_finished)) print("Lida SHE-stylPosition " + stylPosition);//print the stylus position when the cone is green
                //flag_button = false;
                //if (trust_yes.gameObject.activeSelf || YesBu.gameObject.activeSelf || nextBu.gameObject.activeSelf || doneText_session_all.activeSelf)
                    //flag_button = true;
                if (cone.gameObject.activeSelf) //(!flag_button)
                { 
                    if(PathFollower.shared)
                    if ((PathFollower.shared.started) && (PathFollower.shared.distanceTravelled < PathFollower.shared.pathCreator.path.length))//lida//cone is moving
                {
                    //print("it is moving");
                    if (Mathf.Sqrt(Mathf.Pow(stylPosition.x - conePosition.x, 2) + Mathf.Pow(stylPosition.y - conePosition.y, 2)) < 1.5)//print the stylus position when the cone is green or red (not yellow) and it is moving
                    {
                        tracingNumber = tracingNumber + 1;
                        tracingNumber_path = tracingNumber_path + 1;
                        /////////******************************************lida
                        //print("Lida SHE-stylPosition " + stylPosition);//print the stylus position when the cone is green or red (not yellow) and it is moving
                        //lida//getting the distance between stylus and the plane// it is correct Lida
                        /*float dis_ref = Lplane1.GetDistanceToPoint(stylPosition);
                        //for having the first reference for measuring the other distance
                        
                        if(Lflag_first_var)
                        {
                            Lfirst_var = dis_ref;
                            Lflag_first_var = false;
                        }
                        //putting makers in the first part
                        float temp1 = Mathf.Abs(Lfirst_var - dis_ref);
                        if (Lflag_first && temp1 >0.0005 && temp1<0.0015)//putting a marker at 1 mm with athreshold 0.5 mm
                        {
                            Lflag_first = false;
                            marker.Write(1001);
                            print("Lflag_first");
                        }
                        if(Lflag_second && temp1 > 0.00151 && temp1 < 0.0025)//putting a marker at 2 mm with athreshold 0.5 mm
                        {
                            Lflag_second = false;
                            marker.Write(1002);
                            print("Lflag_second");
                        }
                        if (Lflag_third && temp1 > 0.00251 && temp1 < 0.0035)//putting a marker at 3 mm with athreshold 0.5 mm
                        {
                            Lflag_third = false;
                            marker.Write(1003);
                            print("Lflag_third");
                        }
                        //ptutting a marker at the top
                        if (dis_ref>0.2f && flag_peak && flag_concave && (dis_ref - LPre_dis_ref) < 0.0f)
                        {
                            
                            flag_peak = false;
                            marker.Write(1004);
                            Ldis_ref_top = dis_ref;
                            print("flag_peak_concave");// + dis_ref);
                            //print("and" + LPre_dis_ref);
                        }
                        else if (dis_ref > 0.2f && flag_peak && flag_convex && (dis_ref - LPre_dis_ref) > 0.0f)
                        {
                            flag_peak = false;
                            marker.Write(1004);
                            Ldis_ref_top = dis_ref;
                            print("flag_peak_convex");// + dis_ref);
                            //print("and" + LPre_dis_ref);
                        }
                        //putting a marker after the peak in distance 2mm
                        if(!flag_peak)//&& flag )you can put a flag here too that after put the marker after the peak do not come here
                        {
                            
                            temp1 = Mathf.Abs(Ldis_ref_top - dis_ref);
                            if (temp1 > 0.002 && temp1 < 0.0025)
                            {
                                marker.Write(1005);
                                print("flag_peak_after");
                            }
                        }
                        //if (stylPosition.x==0.0f)
                       // {
                          //  print("marker-x==0");
                          //  marker.Write(1004);
                       // }
                        LPre_dis_ref = dis_ref;//lida//for getting the distance with the previous point to see where is the peak point if the diffrence is less than 0 and it is concave the peak point is there and the diffrence is larger than 0 and it is convex the peak point is there
                        //print("distance-SHE   " + dis_ref);//Lida//printing the distance between the curved surface and the a plane in the background when the cone is green or red (not yellow) and it is moving
                        ////////////*************************************lida
                        */

                        //lida//putting markers according to x position of stylus
                        //((sessionID == 1) || (sessionID == 2) || (sessionID == 4) || (sessionID == 5))
                        if ((sessionID != 3) && (path_show.path_show_instace.next_index - 1)<2)//when the surface is a bump or a hole not a flat
                        {
                                //print("path index" + (path_show.path_show_instace.next_index - 1));
                            if (Flag_Markers_s[0] && (stylPosition.x + 2.096f) > 0.0f && (stylPosition.x + 2.096f) < 0.2f)//else if (Math.Abs(transform.position.x + 2.0962f) < 0.2f)//lida//put a marker when the surface has 2mm height
                            {
                                //lida//marker//here////marker.Write(1002);
                                currentSample[0] = 101;// "1mm-left2";
                                //print("s1L-101");
                                outlet.push_sample(currentSample);
                                //lida//marker
                                Flag_Markers_s[0] = false;//Lida//putting markers in 1 mm, 2mm and 3 mm heights
                                                          // lida//logging the stylus position
                                string lline3 = "Stylus in 1left =================================================================";
                                string lline2 = "best position: -2.096";
                                string time1 = "( " + System.DateTime.Now.ToString("HH:mm:ss.fff") + " )";
                                string lline4 = "=================================================================================";
                                string lines1 = "( " + stylPosition.x.ToString("0.000") + ", " + stylPosition.y.ToString("0.000") + ", " + stylPosition.y.ToString("0.000") + " )";
                                //lida//logging stylus position
                                //lida//logging cone position
                                //HapticPlugin ldevice = sinHEffect.FindObjectOfType<sinHEffect>().devices[0];
                                //Vector3 StylusPos = ldevice.stylusPositionWorld;
                                string lines2 = "( " + cone.transform.position.x.ToString("0.000") + ", " + cone.transform.position.y.ToString("0.000") + ", " + cone.transform.position.y.ToString("0.000") + " )";
                                //string lines2 = "Cone position:                                      " + cone.transform.position;// (" + StylusPos.x + ", " + StylusPos.y + "," + StylusPos.z + ")";
                                //lida//logging cone position
                                string[] lines = { lline3, lline2, lines2, lines1, time1, lline4 };
                                File.AppendAllLines(Path.Combine(docPath, marker_result), lines);

                            }
                            else if (Flag_Markers_s[1] && (stylPosition.x + 1.273f) > 0.0f && (stylPosition.x + 1.273f) < 0.2f)//else if (Math.Abs(transform.position.x + 1.273f) < 0.2f)//lida//put a marker when the surface has 1mm height//(transform.position.x == -1.274f)
                            {
                                //lida//marker//here////marker.Write(1001);
                                currentSample[0] = 102;// "2mm-left2";
                                //print("s2L-102");
                                outlet.push_sample(currentSample);
                                //lida//marker
                                Flag_Markers_s[1] = false;//Lida//putting markers in 1 mm, 2mm and 3 mm heights
                                                          // lida//logging the stylus position
                                string lline3 = "Stylus in 2left =================================================================";
                                string lline2 = "best position: -1.273";
                                string time1 = "( " + System.DateTime.Now.ToString("HH:mm:ss.fff") + " )";
                                string lline4 = "=================================================================================";
                                string lines1 = "( " + stylPosition.x.ToString("0.000") + ", " + stylPosition.y.ToString("0.000") + ", " + stylPosition.y.ToString("0.000") + " )";
                                //lida//logging stylus position
                                //lida//logging cone position
                                //HapticPlugin ldevice = sinHEffect.FindObjectOfType<sinHEffect>().devices[0];
                                //Vector3 StylusPos = ldevice.stylusPositionWorld;
                                string lines2 = "( " + cone.transform.position.x.ToString("0.000") + ", " + cone.transform.position.y.ToString("0.000") + ", " + cone.transform.position.y.ToString("0.000") + " )";
                                //lida//logging cone position
                                string[] lines = { lline3, lline2, lines2, lines1, time1, lline4 };
                                File.AppendAllLines(Path.Combine(docPath, marker_result), lines);

                            }
                            else if (Flag_Markers_s[2] && ((stylPosition.x == 0.0f) || ((stylPosition.x > 0.0f) && (stylPosition.x < 0.2f))))//if (Math.Abs(transform.position.x - 0.0f) < 0.2f)//lida//put a marker when we are in the top of the hole or bump
                            {//lida//after passsing that point not before it
                             //lida//marker//here////marker.Write(1003);
                                currentSample[0] = 103;// "topbottom";
                                //print("sTB-103");
                                outlet.push_sample(currentSample);
                                //lida//marker
                                Flag_Markers_s[2] = false;//Lida//putting markers in 1 mm, 2mm and 3 mm heights
                                                          // lida//logging the stylus position
                                string lline3 = "Stylus in middle =================================================================";
                                string lline2 = "best position: 0";
                                string time1 = "( " + System.DateTime.Now.ToString("HH:mm:ss.fff") + " )";
                                string lline4 = "==================================================================================";
                                string lines1 = "( " + stylPosition.x.ToString("0.000") + ", " + stylPosition.y.ToString("0.000") + ", " + stylPosition.y.ToString("0.000") + " )";
                                //lida//logging stylus position
                                //lida//logging cone position
                                //HapticPlugin ldevice = sinHEffect.FindObjectOfType<sinHEffect>().devices[0];
                                //Vector3 StylusPos = ldevice.stylusPositionWorld;
                                string lines2 = "( " + cone.transform.position.x.ToString("0.000") + ", " + cone.transform.position.y.ToString("0.000") + ", " + cone.transform.position.y.ToString("0.000") + " )";
                                //lida//logging cone position
                                string[] lines = { lline3, lline2, lines2, lines1, time1, lline4 };
                                File.AppendAllLines(Path.Combine(docPath, marker_result), lines);
                            }
                            else if (Flag_Markers_s[3] && (stylPosition.x - 1.273f) > 0.0f && (stylPosition.x - 1.273f) < 0.2f)//else if (Math.Abs(transform.position.x - 1.273f) < 0.2f)//lida//put a marker when the surface has 1mm height
                            {
                                //lida//marker//here////marker.Write(1001);
                                currentSample[0] = 104;// "2mm-right2";
                                //print("s2R-104");
                                outlet.push_sample(currentSample);
                                //lida//marker
                                Flag_Markers_s[3] = false;//Lida//putting markers in 1 mm, 2mm and 3 mm heights
                                                          // lida//logging the stylus position
                                string lline3 = "Stylus in 2right =================================================================";
                                string lline2 = "best position: 1.273";
                                string time1 = "( " + System.DateTime.Now.ToString("HH:mm:ss.fff") + " )";
                                string lline4 = "==================================================================================";
                                string lines1 = "( " + stylPosition.x.ToString("0.000") + ", " + stylPosition.y.ToString("0.000") + ", " + stylPosition.y.ToString("0.000") + " )";
                                //lida//logging stylus position
                                //lida//logging cone position
                                //HapticPlugin ldevice = sinHEffect.FindObjectOfType<sinHEffect>().devices[0];
                                //Vector3 StylusPos = ldevice.stylusPositionWorld;
                                string lines2 = "( " + cone.transform.position.x.ToString("0.000") + ", " + cone.transform.position.y.ToString("0.000") + ", " + cone.transform.position.y.ToString("0.000") + " )";
                                //lida//logging cone position
                                string[] lines = { lline3, lline2, lines2, lines1, time1, lline4 };
                                File.AppendAllLines(Path.Combine(docPath, marker_result), lines);

                            }
                            else if (Flag_Markers_s[4] && (stylPosition.x - 2.096f) > 0.0f && (stylPosition.x - 2.096f) < 0.2f)//else if (Math.Abs(transform.position.x - 2.0962f) < 0.2f)//lida//put a marker when the surface has 2mm height
                            {
                                //lida//marker//here////marker.Write(1002);
                                currentSample[0] = 105;// "1mm-right2";
                                //print("s1R-105");
                                outlet.push_sample(currentSample);
                                //lida//marker
                                Flag_Markers_s[4] = false;//Lida//putting markers in 1 mm, 2mm and 3 mm heights
                                                          // lida//logging the stylus position
                                string lline3 = "Stylus in 1right =================================================================";
                                string lline2 = "best position: 2.096";
                                string time1 = "( " + System.DateTime.Now.ToString("HH:mm:ss.fff") + " )";
                                string lline4 = "==================================================================================";
                                string lines1 = "( " + stylPosition.x.ToString("0.000") + ", " + stylPosition.y.ToString("0.000") + ", " + stylPosition.y.ToString("0.000") + " )";
                                //lida//logging stylus position
                                //lida//logging cone position
                                //HapticPlugin ldevice = sinHEffect.FindObjectOfType<sinHEffect>().devices[0];
                                //Vector3 StylusPos = ldevice.stylusPositionWorld;
                                string lines2 = "( " + cone.transform.position.x.ToString("0.000") + ", " + cone.transform.position.y.ToString("0.000") + ", " + cone.transform.position.y.ToString("0.000") + " )";
                                //lida//logging cone position
                                string[] lines = { lline3, lline2, lines2, lines1, time1, lline4 };
                                File.AppendAllLines(Path.Combine(docPath, marker_result), lines);

                            }
                            //lida//putting markers according to x position of stylus
                        }

                    }
                }
                }//if (!trust_yes.gameObject.activeSelf || !YesBu.gameObject.activeSelf || !nextBu.gameObject.activeSelf || !doneText_session_all.gameObject.activeSelf)

                //string forceTypeName = System.Enum.GetName(typeof(EFFECT_TYPE), (int)effectType);
                //float m = map(stylusZZ, -60f, 0f, 0.2f, 0.0f); //float m = map(value, low1, high1, low2, high2);
                //lida//flat and non-flat surface
                /*if (sessionID == 3)
                {
                    Vector3 dd = new Vector3 (0.0f, 0.0f, 0.0f);
                    directionWorld = transform.TransformDirection(dd);
                    print("flat   " + directionWorld);
                }
                else
                {*/
                //Updatepos(Mag, stylusXX, stylusYY, stylusZZ);
                Vector3 cDirection = UpdateParameters_Direction(Mag, Direction, stylusXX, stylusYY, stylusZZ);
                Direction = cDirection;
                //lida//direction change
                directionWorld = transform.TransformDirection(Direction);
                //print("Direction.x    " + Direction.x);
                //print("Direction.y    " + Direction.y);
                //print("Direction.z    " + Direction.z);
                //lida//direction change
                //print("non-flat   " + directionWorld);
                //}
                //Lida

                // Convert from the World coordinates to coordinates relative to the haptic device.
                //if (sessionID == 3)
                // focusPointWorld = transform.TransformPoint(Position);
                Vector3 focalPointDevLocal = device.transform.InverseTransformPoint(focusPointWorld);//HapticEffect
                Vector3 rotationDevLocal = device.transform.InverseTransformDirection(directionWorld);//HapticEffect
                double[] pos = { focalPointDevLocal.x, focalPointDevLocal.y, focalPointDevLocal.z };//HapticEffect
                double[] dir = { rotationDevLocal.x, rotationDevLocal.y, rotationDevLocal.z };//HapticEffect
                //print("rotationDevLocal    " + rotationDevLocal);
                //print("rotationDevLocal.y    " + rotationDevLocal.y);
                //print("rotationDevLocal.z    " + rotationDevLocal.z);
                // Send the current effect settings to OpenHaptics.
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

            }
            else
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
                }
                else
                {
                    HapticPlugin.effects_stopEffect(device.configName, ID);
                }
            }

        }
        double fps = 1 / Time.deltaTime;
        //frame_after = System.DateTime.Now;
        //all_trial_duration = frame_after.Subtract(frame_before);
        string lines22 = "frame time: " + fps;// all_trial_duration.ToString();
        string[] liness22 = { lines22 };
        File.AppendAllLines(Path.Combine(docPath_mid, out_result_mid), liness22);
    }
    private Vector2 UpdateParameters_Velocity(float tracingSpeedX, float tracingSpeedY)
    {
        tracingSpeed.x = tracingSpeedX;
        tracingSpeed.y = tracingSpeedY;
        return tracingSpeed;
    }
    private Vector2 UpdateParameters_Velocity_path(float tracingSpeed_pathX, float tracingSpeed_pathY)
    {
        tracingSpeed_path_temtem.x = tracingSpeed_pathX;
        tracingSpeed_path_temtem.y = tracingSpeed_pathY;
        return tracingSpeed_path_temtem;
    }
    private Vector3 UpdateParameters_Position(double Mag, Vector3 stylPosition, float stylusXX, float stylusYY, float stylusZZ)
    {
        stylPosition.x = stylusXX;
        stylPosition.y = stylusYY;
        stylPosition.z = stylusZZ;
        return stylPosition;
    }
    private Vector3 UpdateParameters_Direction(double Mag, Vector3 Direction, float stylusXX, float stylusYY, float stylusZZ)
    {
        //Mag = 0.1f;
        Magnitude = Mag;
        //Lida//calculating the force direction acccoring to the surface function
        //Deng's Code for the Gaussian surface which is not correct
        /*Direction.x = 0.1f * Mathf.Cos(stylusXX);//0;//
        Direction.y = 0.1f * Mathf.Cos(stylusYY);//0;//
        Direction.z = 0;*/
        if (sessionID == 3 || sessionID == 6 || sessionID == 7 || !cone.gameObject.activeSelf || plane.gameObject.activeSelf)// || trust_yes.gameObject.activeSelf || YesBu.gameObject.activeSelf || nextBu.gameObject.activeSelf || doneText_session_all.activeSelf)
        {
            Direction.x = 0.0f;
            Direction.y = 0.0f;
            Direction.z = 0.0f;
        }
        else if (sessionID == 1 || sessionID==4)//bump and not real bump
        {
            Direction.x = -0.3f * (stylusXX / 2.0f) * Mathf.Exp(-(Mathf.Pow(stylusXX, 2) / 4.0f)); //0.0f;//
                                                                                                   //print("stylusXX   " + stylusXX);
                                                                                                   //print("Direction.x    " + Direction.x);
                                                                                                   //print("stylusYY   " + stylusYY);
            Direction.y = 0.0f; //-0.3f * (stylusYY / 2.0f) * Mathf.Exp(-(Mathf.Pow(stylusYY, 2) / 4.0f));//
                                //print("Direction.y    " + Direction.y);
                                //Direction.x = 0.0f;
                                //Direction.y = 0.0f;
            Direction.z = 0.0f;//-0.3f * (stylusXX / 2.0f) * Mathf.Exp(-(Mathf.Pow(stylusXX, 2) / 4.0f));//
                               //Lida//calculating the force direction acccoring to the surface function
                               //print("Direction    " + Direction);
        }
        else if (sessionID == 2 || sessionID ==5)//hole and not real hole
        {
            Direction.x = 0.3f * (stylusXX / 2.0f) * Mathf.Exp(-(Mathf.Pow(stylusXX, 2) / 4.0f)); //0.0f;//
                                                                                                  //print("stylusXX   " + stylusXX);
                                                                                                  //print("Direction.x    " + Direction.x);
                                                                                                  //print("stylusYY   " + stylusYY);
            Direction.y = 0.0f; //-0.3f * (stylusYY / 2.0f) * Mathf.Exp(-(Mathf.Pow(stylusYY, 2) / 4.0f));//
                                //print("Direction.y    " + Direction.y);
                                //Direction.x = 0.0f;
                                //Direction.y = 0.0f;
            Direction.z = 0.0f;//-0.3f * (stylusXX / 2.0f) * Mathf.Exp(-(Mathf.Pow(stylusXX, 2) / 4.0f));//
                               //Lida//calculating the force direction acccoring to the surface function
                               //print("Direction    " + Direction);
        }
        else //if (sessionID == 3)//flat surface
        {
            print("I am hereeeeeeeeeee");
            Direction.x = 0.0f;
            Direction.y = 0.0f;
            Direction.z = 0.0f;
        }
        //print("Updatex    " + Direction.x);
        //print("Updatey    " + Direction.y);
        //print("Updatez    " + Direction.z);
        return Direction;
    }
    void Updatedis_PtoC(double dis_PtoC)
    {
        //DisPtoCText.text = "StylusX: " + dis_PtoC.ToString("0.00");
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
            HapticPlugin device = devices[ii];
            if (device == null)
                continue;
            int ID = FXID[ii];
            HapticPlugin.effects_stopEffect(device.configName, ID);
        }
    }
    void OnDisable()
    {
        //For every haptic device, send a Stop event to OpenHaptics
        for (int ii = 0; ii < devices.Length; ii++)
        {
            HapticPlugin device = devices[ii];
            if (device == null)
                continue;
            int ID = FXID[ii];
            HapticPlugin.effects_stopEffect(device.configName, ID);
            inTheZone[ii] = false;
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

        Vector3 focusPointWorld = transform.TransformPoint(stylPosition);


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
            if (delta[ii] <= Mathf.Epsilon)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(devicePoint[ii], 1.0f);
                //if(effectType == EFFECT_TYPE.SPRING)
                //	Gizmos.DrawLine(focusPointWorld, devicePoint [ii]);
            }
        }

    }
    //giving timer or delay for one action****************
    /*IEnumerator Example()
    {
        //print("first:   " + Time.time);
        //Time.timeScale = 1.0f;
        timer_text.SetActive(true);
        //cone.SetActive(false);
        cone.GetComponent<Renderer>().material.color = Color.black;
        yield return new WaitForSeconds(2);
        //cone.SetActive(true);
        cone.GetComponent<Renderer>().material.color = Color.yellow;
        timer_text.SetActive(false);
        //print("second:   " + Time.time);
    }*/
    //different functions for given session ID*********************************************
    void GiveSessioID()
    {
        //trial_type = new int[5];//before 3
        //int[] Session_Number = new int[30];
        /*trial_type[0] = 10;
        trial_type[1] = 10;
        trial_type[2] = 10;
        trial_type[3] = 0;
        trial_type[4] = 0;*/
        /*trial_type[0] = Bump_trial;
        trial_type[1] = hole_trial;
        trial_type[2] = flat_trial;
        trial_type[3] = Nobump_trial;
        trial_type[4] = Nohole_trial;*/
        var names = new List<int>() { 1, 2, 3, 1, 2, 3 };
        //int TT = Bump_trial + hole_trial + flat_trial+ Nobump_trial+ Nohole_trial;// 30//FirstN + T1;
        int p1;                                            //Session_Number[0] = 1;//Lida//I want to put the first surface always bump so in below kk started from 1
        System.Random rnd = new System.Random();
        int kk, kl;
        int tedad = 0;
        //for (kk = FirstN; kk < TT; kk++)
        for (kk = 0; kk < 5; kk++)//5*6=30
        {
            for (kl = 0; kl < 6; kl++)//length of abow list
            {
                p1 = rnd.Next(names.Count);
                Session_Number[tedad] = (int)names.ElementAt(p1);
                names.Remove(Session_Number[tedad]);
                tedad++;
            }
            names = new List<int>() { 1, 2, 3, 1, 2, 3 };
            if (kk > 0)
            {
                if (Session_Number[tedad - 6 - 1] == Session_Number[tedad - 6])
                {
                    //Console.WriteLine("it happened");
                    int temp1;
                    temp1 = Session_Number[tedad - 6 - 1];
                    Session_Number[tedad - 6 - 1] = Session_Number[tedad - 6 - 3];
                    Session_Number[tedad - 6 - 3] = temp1;
                }
            }
        }
        //for (int d = 0; d < 30; d++)
        // print("Session_Number[d]: " + Session_Number[d]);
        ///Console.WriteLine(" {0}--{1}", d, Session_Number[d]);
    }
    void GiveSessioIDUn()
    {
        Session_Number[0] = 1;
        Session_Number[1] = 1;
        Session_Number[2] = 3;
        Session_Number[3] = 2;
        Session_Number[4] = 2;
        Session_Number[5] = 3;
        Session_Number[6] = 1;
        Session_Number[7] = 2;
        Session_Number[8] = 3;
        Session_Number[9] = 1;
        Session_Number[10] = 3;
        Session_Number[11] = 2;
        Session_Number[12] = 2;
        Session_Number[13] = 3;
        Session_Number[14] = 1;
        Session_Number[15] = 3;
        Session_Number[16] = 2;
        Session_Number[17] = 2;
        Session_Number[18] = 1;
        Session_Number[19] = 1;
        Session_Number[20] = 3;
        Session_Number[21] = 3;
        Session_Number[22] = 2;
        Session_Number[23] = 1;
        Session_Number[24] = 3;
        Session_Number[25] = 2;
        Session_Number[26] = 2;
        Session_Number[27] = 1;
        Session_Number[28] = 3;
        Session_Number[29] = 1;
        /////////////
        MixedRe[0] = 0;
        MixedRe[1] = 1;
        MixedRe[2] = 1;
        MixedRe[3] = 0;
        MixedRe[4] = 0;
        MixedRe[5] = 1;
        MixedRe[6] = 1;
        MixedRe[7] = 1;
        MixedRe[8] = 0;
        MixedRe[9] = 0;
        MixedRe[10] = 0;
        MixedRe[11] = 1;
        MixedRe[12] = 0;
        MixedRe[13] = 1;
        MixedRe[14] = 1;
        MixedRe[15] = 1;
        MixedRe[16] = 1;
        MixedRe[17] = 1;
        MixedRe[18] = 0;
        MixedRe[19] = 1;
        MixedRe[20] = 0;
        MixedRe[21] = 0;
        MixedRe[22] = 1;
        MixedRe[23] = 0;
        MixedRe[24] = 1;
        MixedRe[25] = 0;
        MixedRe[26] = 0;
        MixedRe[27] = 1;
        MixedRe[28] = 0;
        MixedRe[29] = 0;

    }
    void GiveSessioIDMix1()
    {
        Session_Number[0] = 1;
        Session_Number[1] = 5;
        Session_Number[2] = 1;
        Session_Number[3] = 4;
        Session_Number[4] = 2;
        Session_Number[5] = 3;
        Session_Number[6] = 4;
        Session_Number[7] = 3;
        Session_Number[8] = 5;
        Session_Number[9] = 2;
        Session_Number[10] = 5;
        Session_Number[11] = 4;
        Session_Number[12] = 3;
        Session_Number[13] = 5;
        Session_Number[14] = 3;
        Session_Number[15] = 4;
        Session_Number[16] = 1;
        Session_Number[17] = 2;
        Session_Number[18] = 1;
        Session_Number[19] = 2;
        Session_Number[20] = 3;
        Session_Number[21] = 5;
        Session_Number[22] = 3;
        Session_Number[23] = 4;
        Session_Number[24] = 4;
        Session_Number[25] = 5;
        Session_Number[26] = 2;
        Session_Number[27] = 1;
        Session_Number[28] = 2;
        Session_Number[29] = 1;
        Session_Number[30] = 5;
        Session_Number[31] = 2;
        Session_Number[32] = 5;
        Session_Number[33] = 1;
        Session_Number[34] = 3;
        Session_Number[35] = 3;
        Session_Number[36] = 1;
        Session_Number[37] = 4;
        Session_Number[38] = 4;
        Session_Number[39] = 2;
        /////////////
        MixedRe[0] = 1;
        MixedRe[1] = 1;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 1;
        MixedRe[5] = 1;
        MixedRe[6] = 1;
        MixedRe[7] = 1;
        MixedRe[8] = 1;
        MixedRe[9] = 1;
        MixedRe[10] = 1;
        MixedRe[11] = 1;
        MixedRe[12] = 1;
        MixedRe[13] = 1;
        MixedRe[14] = 1;
        MixedRe[15] = 1;
        MixedRe[16] = 1;
        MixedRe[17] = 1;
        MixedRe[18] = 1;
        MixedRe[19] = 1;
        MixedRe[20] = 0;
        MixedRe[21] = 0;
        MixedRe[22] = 0;
        MixedRe[23] = 0;
        MixedRe[24] = 1;
        MixedRe[25] = 0;
        MixedRe[26] = 0;
        MixedRe[27] = 1;
        MixedRe[28] = 1;
        MixedRe[29] = 0;
        MixedRe[30] = 1;
        MixedRe[31] = 0;
        MixedRe[32] = 0;
        MixedRe[33] = 1;
        MixedRe[34] = 1;
        MixedRe[35] = 1;
        MixedRe[36] = 0;
        MixedRe[37] = 0;
        MixedRe[38] = 0;
        MixedRe[39] = 1;

    }
    void GiveSessioIDMix2()
    {
        Session_Number[0] = 1;
        Session_Number[1] = 5;
        Session_Number[2] = 1;
        Session_Number[3] = 3;
        Session_Number[4] = 4;
        Session_Number[5] = 4;
        Session_Number[6] = 2;
        Session_Number[7] = 2;
        Session_Number[8] = 5;
        Session_Number[9] = 3;
        Session_Number[10] = 2;
        Session_Number[11] = 5;
        Session_Number[12] = 1;
        Session_Number[13] = 5;
        Session_Number[14] = 1;
        Session_Number[15] = 3;
        Session_Number[16] = 4;
        Session_Number[17] = 4;
        Session_Number[18] = 2;
        Session_Number[19] = 3;
        Session_Number[20] = 4;
        Session_Number[21] = 3;
        Session_Number[22] = 2;
        Session_Number[23] = 1;
        Session_Number[24] = 1;
        Session_Number[25] = 3;
        Session_Number[26] = 5;
        Session_Number[27] = 4;
        Session_Number[28] = 5;
        Session_Number[29] = 5;
        Session_Number[30] = 4;
        Session_Number[31] = 2;
        Session_Number[32] = 1;
        Session_Number[33] = 2;
        Session_Number[34] = 4;
        Session_Number[35] = 3;
        Session_Number[36] = 5;
        Session_Number[37] = 3;
        Session_Number[38] = 1;
        Session_Number[39] = 2;
        /////////////
        MixedRe[0] = 1;
        MixedRe[1] = 1;
        MixedRe[2] = 1;
        MixedRe[3] = 1;
        MixedRe[4] = 1;
        MixedRe[5] = 1;
        MixedRe[6] = 1;
        MixedRe[7] = 1;
        MixedRe[8] = 1;
        MixedRe[9] = 1;
        MixedRe[10] = 1;
        MixedRe[11] = 1;
        MixedRe[12] = 1;
        MixedRe[13] = 1;
        MixedRe[14] = 1;
        MixedRe[15] = 1;
        MixedRe[16] = 1;
        MixedRe[17] = 1;
        MixedRe[18] = 1;
        MixedRe[19] = 1;
        MixedRe[20] = 0;
        MixedRe[21] = 1;
        MixedRe[22] = 0;
        MixedRe[23] = 1;
        MixedRe[24] = 0;
        MixedRe[25] = 1;
        MixedRe[26] = 0;
        MixedRe[27] = 0;
        MixedRe[28] = 1;
        MixedRe[29] = 0;
        MixedRe[30] = 1;
        MixedRe[31] = 0;
        MixedRe[32] = 0;
        MixedRe[33] = 1;
        MixedRe[34] = 0;
        MixedRe[35] = 0;
        MixedRe[36] = 0;
        MixedRe[37] = 0;
        MixedRe[38] = 1;
        MixedRe[39] = 1;

    }
    void GiveSessioIDMix4()
    {
        Session_Number[20] = 1;
        Session_Number[21] = 5;
        Session_Number[22] = 1;
        Session_Number[23] = 4;
        Session_Number[24] = 2;
        Session_Number[25] = 3;
        Session_Number[26] = 4;
        Session_Number[27] = 3;
        Session_Number[28] = 5;
        Session_Number[29] = 2;
        Session_Number[30] = 5;
        Session_Number[31] = 4;
        Session_Number[32] = 3;
        Session_Number[33] = 5;
        Session_Number[34] = 3;
        Session_Number[35] = 4;
        Session_Number[36] = 1;
        Session_Number[37] = 2;
        Session_Number[38] = 1;
        Session_Number[39] = 2;
        Session_Number[0] = 3;
        Session_Number[1] = 5;
        Session_Number[2] = 3;
        Session_Number[3] = 4;
        Session_Number[4] = 4;
        Session_Number[5] = 5;
        Session_Number[6] = 2;
        Session_Number[7] = 1;
        Session_Number[8] = 2;
        Session_Number[9] = 1;
        Session_Number[10] = 5;
        Session_Number[11] = 2;
        Session_Number[12] = 5;
        Session_Number[13] = 1;
        Session_Number[14] = 3;
        Session_Number[15] = 3;
        Session_Number[16] = 1;
        Session_Number[17] = 4;
        Session_Number[18] = 4;
        Session_Number[19] = 2;
        /////////////
        MixedRe[20] = 1;
        MixedRe[21] = 1;
        MixedRe[22] = 1;
        MixedRe[23] = 1;
        MixedRe[24] = 1;
        MixedRe[25] = 1;
        MixedRe[26] = 1;
        MixedRe[27] = 1;
        MixedRe[28] = 1;
        MixedRe[29] = 1;
        MixedRe[30] = 1;
        MixedRe[31] = 1;
        MixedRe[32] = 1;
        MixedRe[33] = 1;
        MixedRe[34] = 1;
        MixedRe[35] = 1;
        MixedRe[36] = 1;
        MixedRe[37] = 1;
        MixedRe[38] = 1;
        MixedRe[39] = 1;
        MixedRe[0] = 0;
        MixedRe[1] = 0;
        MixedRe[2] = 0;
        MixedRe[3] = 0;
        MixedRe[4] = 1;
        MixedRe[5] = 0;
        MixedRe[6] = 0;
        MixedRe[7] = 1;
        MixedRe[8] = 1;
        MixedRe[9] = 0;
        MixedRe[10] = 1;
        MixedRe[11] = 0;
        MixedRe[12] = 0;
        MixedRe[13] = 1;
        MixedRe[14] = 1;
        MixedRe[15] = 1;
        MixedRe[16] = 0;
        MixedRe[17] = 0;
        MixedRe[18] = 0;
        MixedRe[19] = 1;

    }
    void GiveSessioIDMix3()
    {
        Session_Number[20] = 1;
        Session_Number[21] = 5;
        Session_Number[22] = 1;
        Session_Number[23] = 3;
        Session_Number[24] = 4;
        Session_Number[25] = 4;
        Session_Number[26] = 2;
        Session_Number[27] = 2;
        Session_Number[28] = 5;
        Session_Number[29] = 3;
        Session_Number[30] = 2;
        Session_Number[31] = 5;
        Session_Number[32] = 1;
        Session_Number[33] = 5;
        Session_Number[34] = 1;
        Session_Number[35] = 3;
        Session_Number[36] = 4;
        Session_Number[37] = 4;
        Session_Number[38] = 2;
        Session_Number[39] = 3;
        Session_Number[0] = 4;
        Session_Number[1] = 3;
        Session_Number[2] = 2;
        Session_Number[3] = 1;
        Session_Number[4] = 1;
        Session_Number[5] = 3;
        Session_Number[6] = 5;
        Session_Number[7] = 4;
        Session_Number[8] = 5;
        Session_Number[9] = 5;
        Session_Number[10] = 4;
        Session_Number[11] = 2;
        Session_Number[12] = 1;
        Session_Number[13] = 2;
        Session_Number[14] = 4;
        Session_Number[15] = 3;
        Session_Number[16] = 5;
        Session_Number[17] = 3;
        Session_Number[18] = 1;
        Session_Number[19] = 2;
        /////////////
        MixedRe[20] = 1;
        MixedRe[21] = 1;
        MixedRe[22] = 1;
        MixedRe[23] = 1;
        MixedRe[24] = 1;
        MixedRe[25] = 1;
        MixedRe[26] = 1;
        MixedRe[27] = 1;
        MixedRe[28] = 1;
        MixedRe[29] = 1;
        MixedRe[30] = 1;
        MixedRe[31] = 1;
        MixedRe[32] = 1;
        MixedRe[33] = 1;
        MixedRe[34] = 1;
        MixedRe[35] = 1;
        MixedRe[36] = 1;
        MixedRe[37] = 1;
        MixedRe[38] = 1;
        MixedRe[39] = 1;
        MixedRe[0] = 0;
        MixedRe[1] = 1;
        MixedRe[2] = 0;
        MixedRe[3] = 1;
        MixedRe[4] = 0;
        MixedRe[5] = 1;
        MixedRe[6] = 0;
        MixedRe[7] = 0;
        MixedRe[8] = 1;
        MixedRe[9] = 0;
        MixedRe[10] = 1;
        MixedRe[11] = 0;
        MixedRe[12] = 0;
        MixedRe[13] = 1;
        MixedRe[14] = 0;
        MixedRe[15] = 0;
        MixedRe[16] = 0;
        MixedRe[17] = 0;
        MixedRe[18] = 1;
        MixedRe[19] = 1;

    }
    void GiveSessioIDMix()
    {
        //int[] Session_Number11;
        //Session_Number11 = new int[128];
        var names = new List<int>() { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 };//6//8
        int p1;
        System.Random rnd = new System.Random();
        int kk, kl, r1, ii, jj;
        int tedad = 0;
        int[,] UnRe;
        UnRe = new int[5, 2];//#unrelaible feedback
        //for (kk = FirstN; kk < TT; kk++)
        for (r1 = 0; r1 < 2; r1++)//two times one relaible and one unreliable
        {
            UnRe[0, 0] = 2;//bump-1
            UnRe[0, 1] = 2;//bump-1
            UnRe[1, 0] = 2;//hole-2
            UnRe[1, 1] = 2;//hole-2
            UnRe[2, 0] = 2;//flat-3
            UnRe[2, 1] = 2;//flat-3
            UnRe[3, 0] = 1;//illusion bump-4
            UnRe[3, 1] = 3;//illusion bump-4
            UnRe[4, 0] = 1;//illusion hole-5
            UnRe[4, 1] = 3;//illusion hole-5

            //ii = 4;//illusion bump
            //jj = 4;//illusion hole
            for (kk = 0; kk < 2; kk++)//2*10=20//4*8=32//for (kk = 0; kk < 4; kk++)
            {
                for (kl = 0; kl < 10; kl++)//length of above list//for (kl = 0; kl < 8; kl++)
                {
                    p1 = rnd.Next(names.Count);
                    Session_Number[tedad] = (int)names.ElementAt(p1);
                    names.Remove(Session_Number[tedad]);
                    int gh = Random.Range(1, 3);
                    /*if (Session_Number[tedad] == 4)
                    {
                        if (gh == 1)//bump
                        {
                            if (ii > 0)//bump
                                ii--;
                            else//hole
                            {
                                Session_Number[tedad] = 5;
                                jj--;
                            }
                            
                        }
                        else//hole
                        {
                            if (jj > 0)//hole
                            {
                                Session_Number[tedad] = 5;
                                jj--;
                            }
                            else//bump
                                ii--;
                        }
                    }*/
                    if (r1 == 0)// || r1 == 2)
                        MixedRe[tedad] = 1;
                    else //if(r1==1)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (Session_Number[tedad] == (i + 1))
                            {
                                if (gh == 1)//relaible
                                {
                                    if (UnRe[i, 0] > 0)//if its capacity is not full
                                    {
                                        MixedRe[tedad] = 1;//make it relaible
                                        UnRe[i, 0]--;
                                    }
                                    else
                                    {
                                        MixedRe[tedad] = 0;//otherwise unrelaible
                                        UnRe[i, 1]--;
                                    }
                                }
                                else//unreliable
                                {
                                    if (UnRe[i, 1] > 0)//if its capacity is not full
                                    {
                                        MixedRe[tedad] = 0;//make it unrelaible
                                        UnRe[i, 1]--;
                                    }
                                    else
                                    {
                                        MixedRe[tedad] = 1;//otherwise relaible
                                        UnRe[i, 0]--;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    tedad++;
                }
                names = new List<int>() { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 };
                if (kk > 0)
                {
                    if (Session_Number[tedad - 10 - 1] == Session_Number[tedad - 10])//10 length of the list//if (Session_Number[tedad - 8 - 1] == Session_Number[tedad - 8])
                    {
                        //Console.WriteLine("it happened");
                        int temp1, temp2;
                        temp1 = Session_Number[tedad - 10 - 1];
                        temp2 = MixedRe[tedad - 10 - 1];
                        Session_Number[tedad - 10 - 1] = Session_Number[tedad - 10 - 3];
                        MixedRe[tedad - 10 - 1] = MixedRe[tedad - 10 - 3];
                        Session_Number[tedad - 10 - 3] = temp1;
                        MixedRe[tedad - 10 - 3] = temp2;
                    }
                }
            }
        }
        /*for (int d = 0; d < 128; d++)
        {
            print("Session_Number[d]: " + Session_Number11[d]);
            print("MixedRe[d]: " + MixedRe[d]);
        }*/
        ///Console.WriteLine(" {0}--{1}", d, Session_Number[d]);
    }
    //different functions for given session ID*********************************************

}


//producing random session ID//It does mot work
/*void GiveSessioID(int FirstN, int T1, int R1)
{
    //print("int FirstN:  " + FirstN );
    //print("int T1:  " +  T1 );
    //print("int R1:  " +  R1);
    trial_type = new int[5];//before 3
    trial_type[0] = Bump_trial;
    trial_type[1] = hole_trial;
    trial_type[2] = flat_trial;
    trial_type[3] = Nobump_trial;
    trial_type[4] = Nohole_trial;
    int a1, a2;
    a1 = 0;
    a2 = 0;
    int TT = Bump_trial + hole_trial + flat_trial;// FirstN + T1;
                                                  //Session_Number[0] = 1;//Lida//I want to put the first surface always bump so in below kk started from 1
    int co1;
    int kk;
    //for (kk = FirstN; kk < TT; kk++)
    for (kk = 0; kk < TT; kk++)
    {
        Random rnd = new Random();
        co1 = Random.Range(1, R1);
        //bool flag_enter = false;
        if (kk > 1)//(FirstN+1)
        {
            if ((a1 == a2) && (a2 == co1))
                while ((co1 == a1) || trial_type[co1 - 1] == 0)
                {
                    co1 = Random.Range(1, R1);
                }
            if (trial_type[co1 - 1] == 0)
            {
                while (trial_type[co1 - 1] == 0)
                    co1 = Random.Range(1, R1);
                if ((a1 == a2) && (a2 == co1))
                    while ((co1 == a1) || trial_type[co1 - 1] == 0)
                    {
                        co1 = Random.Range(1, R1);
                    }
            }
            //flag_enter = true;
        }
        else//if (!flag_enter)
        {
            while (trial_type[co1 - 1] == 0)
                co1 = Random.Range(1, R1);
        }

        trial_type[co1 - 1]--;
        Session_Number[kk] = co1;
        print("Session_Number[kk]   " + Session_Number[kk]);
        a1 = a2;
        a2 = Session_Number[kk];
    }
}*/

/*void UnReliableResponse1()//creating UnReliableResponse
{
    if (flag_concave)//bump
    {
        //print("bumpbbb1");
        textsys = system_res.GetComponent<Text>();
        Random rnd = new Random();
        int gh = Random.Range(1, 3);//random chosing it will be a correct answer or wrong
        if (gh == 1)//reliable answer
        {
            if (bump[0] > 0)//reliable answer
            {
                textsys.text = "System recommendation: Bump";
                feedback = "UnReliable. CBump";
                bump[0]--;
            }
            else//unreliable answer as the reliable answers are already finished
            {
                //print("bumpbbb2");
                //gh = Random.Range(1, 3);//random slection between the other two wrong answers
                if (gh == 1)
                {
                    textsys.text = "System recommendation: Hole";
                    feedback = "UnReliable. Hole";
                }
                else
                {
                    textsys.text = "System recommendation: Flat";
                    feedback = "UnReliable. Flat";
                }
                bump[1]--;
            }
        }
        else//unreliable answer
        {
            if (bump[1] > 0)//unreliable answer
            {
                //print("bumpbbb3");
                //gh = Random.Range(1, 3);//random slection between the other two wrong answers
                if (gh == 1)
                {
                    textsys.text = "System recommendation: Hole";
                    feedback = "UnReliable. Hole";
                }
                else
                {
                    textsys.text = "System recommendation: Flat";
                    feedback = "UnReliable. Flat";
                }
                bump[1]--;
            }
            else//unreliable answer as the reliable answers are already finished
            {
                textsys.text = "System recommendation: Bump";
                feedback = "UnReliable. CBump";
                bump[0]--;
            }
        }
    }//if (flag_concave)//bump
    else if (flag_convex)//hole
    {
        //print("holeeeeee");
        textsys = system_res.GetComponent<Text>();
        Random rnd = new Random();
        int gh = Random.Range(1, 3);//random chosing it will be a correct answer or wrong
        if (gh == 1)//reliable answer
        {
            if (hole[0] > 0)//reliable answer
            {
                textsys.text = "System recommendation: Hole";
                feedback = "UnReliable. CHole";
                hole[0]--;
            }
            else//unreliable answer as the reliable answers are already finished
            {
                //print("holeeeeee1");
                //gh = Random.Range(1, 3);//random slection between the other two wrong answers
                if (gh == 1)
                {
                    textsys.text = "System recommendation: Bump";
                    feedback = "UnReliable. Bump";
                }
                else
                {
                    textsys.text = "System recommendation: Flat";
                    feedback = "UnReliable. Flat";
                }
                hole[1]--;
            }
        }
        else//unreliable answer
        {
            if (hole[1] > 0)//unreliable answer
            {
                //print("holeeeeee2");
                //gh = Random.Range(1, 3);//random slection between the other two wrong answers
                if (gh == 1)
                {
                    textsys.text = "System recommendation: Bump";
                    feedback = "UnReliable. Bump";
                }
                else
                {
                    textsys.text = "System recommendation: Flat";
                    feedback = "UnReliable. Flat";
                }
                hole[1]--;
            }
            else//unreliable answer as the reliable answers are already finished
            {
                textsys.text = "System recommendation: Hole";
                feedback = "UnReliable. CHole";
                hole[0]--;
            }
        }
    }//else if (flag_convex)//hole
    else //if (flag_flat)//flag_fla//flat
    {
        //print("flatttt");
        textsys = system_res.GetComponent<Text>();
        Random rnd = new Random();
        int gh = Random.Range(1, 3);//random chosing it will be a correct answer or wrong
        if (gh == 1)//reliable answer
        {
            if (flat[0] > 0)//reliable answer
            {
                textsys.text = "System recommendation: Flat";
                feedback = "UnReliable. CFlat";
                flat[0]--;
            }
            else//unreliable answer as the reliable answers are already finished
            {
                //print("flatttt1");
                //gh = Random.Range(1, 3);//random slection between the other two wrong answers
                if (gh == 1)
                {
                    textsys.text = "System recommendation: Bump";
                    feedback = "UnReliable. Bump";
                }
                else
                {
                    textsys.text = "System recommendation: Hole";
                    feedback = "UnReliable. Hole";
                }
                flat[1]--;
            }
        }
        else//unreliable answer
        {
            if (flat[1] > 0)//unreliable answer
            {
                //print("flatttt2");
                //gh = Random.Range(1, 3);//random slection between the other two wrong answers
                if (gh == 1)
                {
                    textsys.text = "System recommendation: Bump";
                    feedback = "UnReliable. Bump";
                }
                else
                {
                    textsys.text = "System recommendation: Hole";
                    feedback = "UnReliable. Hole";
                }
                flat[1]--;
            }
            else//unreliable answer as the reliable answers are already finished
            {
                textsys.text = "System recommendation: Flat";
                feedback = "UnReliable. CFlat";
                flat[0]--;
            }
        }
    }//else//flag_fla//flat
}//void UnReliableResponse()
*/

