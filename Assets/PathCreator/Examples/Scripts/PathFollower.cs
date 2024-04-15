using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//using Assets.LSL4Unity.Scripts;
using System;
using System.IO;
//lida//putting markers
//using Assets.HapticScripts;
//using LSL;
//lida//putting markers

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {

        //lida for putting markers
        public int sessionID1;
        //GameObject visual_sur = null;
        public bool[] LSLMarkerCone1;
        //public LSLMarkerStream marker;
        public static path_show path_show_instace;
        public HapticPlugin[] devices;
        public bool[] Flag_Markers;
        //lida//putting markers
        /*public StreamInfo streamInfo;
        public StreamOutlet outlet;*/
        //public int[] currentSample;
        //lida//putting markers
        //Lida//putting markers in 1 mm, 2mm and 3 mm heights
        //lida
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed, test;//lida//5 or 4 or 3
        public float distanceTravelled;
        //GameObject ans = null;
        public static PathFollower shared = null;

        //string path_name = "Path"; //Jing1
        public Transform path_head = null; //Jing
        public Transform path_tail = null; //Jing

        public DataLogger logger = null;
        public log_info info = new log_info();
        //lida
        /* in DataLogger.cs
        public struct log_info
        {
            public Vector3 pointer_position;//lida//move it to the pointer position 
            public Quaternion pointer_rotation;
            public Vector3 cone_position;//lida//move it to the cone position 
            public bool started;
            public bool path_finished;
            public bool all_end;
            public float distanceTravelled;
        }

        */
        //lida
        private bool buttonHoldDown = false;

        public GameObject cone = null;
        public GameObject pointer = null;
        public GameObject heliCopter = null;

        public bool started = false;
        public bool path_finished;
        public bool all_end = false;
        System.DateTime time_path_be; System.DateTime time_path_en;

        void Start()
        {
            //test = 0;
            //Lida//putting markers in 1 mm, 2mm and 3 mm heights
            /*streamInfo = new StreamInfo("Inlet", "Markers", 1, 0, LSL.channel_format_t.cf_int32, "Outlet");//(StreamName, StreamType, 3, Time.fixedDeltaTime * 1000, LSL.channel_format_t.cf_float32);
            outlet = new StreamOutlet(streamInfo);
            currentSample = new int[1];*/
            //GameObject heliCopter = GameObject.Find("R22_GRP");
            Flag_Markers = new bool[5];//for each 5 below markers use one flag to send it one time
            LSLMarkerCone1 = new bool[5];//each path hase 5 markers (left 1 mm,left 2mm, middle 3 mm,right 1 mm, right 2mm height)
            for (int i = 0; i < 5; i++)
            {
                Flag_Markers[i] = true;
                LSLMarkerCone1[i] = false;
            }
            
            //Lida//putting markers in 1 mm, 2mm and 3 mm heights
            //lida//logging the stylus position
            //devices = (HapticPlugin[])Object.FindObjectsOfType(typeof(HapticPlugin));
            ////lida//logging the stylus position
            //lida//for puting markers first get the type of the surface
            sessionID1 = sinHEffect.FindObjectOfType<sinHEffect>().sessionID;
            /*if (sessionID1 == 1)
            {
                //print("if (sessionID == 1)");
                //print("0.3e(-x__2div2__2)*********************");
                visual_sur = GameObject.Find("0.3e(-x__2div2__2)"); //concave/
            }
            else if (sessionID1 == 2)
            {
                //print("-0.3e(-x__2div2__2)*********************");
                visual_sur = GameObject.Find("-0.3e(-x__2div2__2)"); //convex
            }*/
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }

            shared = this;
            path_head = GameObject.Find("path_head").transform;//Jing2
            path_tail = GameObject.Find("path_tail").transform;//Jing
            logger = new DataLogger();//lida//creating text file for logging the pointer/stylus position and rotation and cone position
        }
        //Jing3switch function
        public void switch_to(float x1, float y1, float z1, float x2, float y2, float z2)//lida//seem//switching to a new path and putting the cone in the position
        {
            //updating the initialOffsetX of the helicopter
            //heliCopter.transform.position = UpdatePosition.FindObjectOfType<UpdatePosition>().heliInitialPosition;
            //UpdatePosition.FindObjectOfType<UpdatePosition>().initialOffsetX= UpdatePosition.FindObjectOfType<UpdatePosition>().objectToFollow.position.x - UpdatePosition.FindObjectOfType<UpdatePosition>().heliInitialPosition.position.x;
            //print("switch tooooooooooooo"+ test++);
            //Lida//putting markers in 1 mm, 2mm and 3 mm heights
            for (int i = 0; i < 5; i++)
                Flag_Markers[i] = true;
            //Lida//putting markers in 1 mm, 2mm and 3 mm heights
            path_head.position = new Vector3(x1, y1, z1);
            path_tail.position = new Vector3(x2, y2, z2);
            Transform[] waypoints = new Transform[3];
            waypoints[0] = path_head;
            waypoints[1] = path_tail;
            waypoints[2] = path_head;

            //Debug.Log("path switched!");
            //GetComponent<PathCreator>().bezierPath = new BezierPath(waypoints, false, PathSpace.xyz);
            pathCreator.bezierPath = new BezierPath(waypoints, true, PathSpace.xyz);
            pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Aligned;
            pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;


            distanceTravelled = 0;
            path_finished = false;
            sinHEffect.FindObjectOfType<sinHEffect>().finishedpathFlag = 0;
            int timeLeft = 1000;
            while (timeLeft > 0)
            {
                timeLeft--; //Debug.Log("Countdown: " + timeLeft);
            }
            started = false;
            GameObject cone = GameObject.Find("cone");
            Quaternion rot;
            if (cone)
            {
                rot = cone.transform.rotation;
                GameObject.Find("cone").transform.SetPositionAndRotation(new Vector3(x1, y1, z1), rot);
            }
            //print("switch to:    " + x1);

        }


        void log()
        {
            if (cone == null)
                cone = GameObject.Find("cone");
            if (pointer == null)
                pointer = GameObject.Find("pointer");

            info.all_end = all_end;
            info.started = started;
            info.path_finished = path_finished;
            info.distanceTravelled = distanceTravelled;

            if (pointer != null)
            {
                info.pointer_position = pointer.transform.position;
                info.pointer_rotation = pointer.transform.rotation;
            }
            else
            {
                info.pointer_position = new Vector3(0, 0, 0);
                info.pointer_rotation = new Quaternion(0, 0, 0, 0);
            }

            if (cone != null)
            {
                info.cone_position = cone.transform.position;
            }
            else
            {
                info.cone_position = new Vector3(0, 0, 0);
            }

            logger.log_epoch(info);
        }
        void FixedUpdate()
        {
            //currentSample[0] = 301;// "s-1mm-left2";
            //print("c-1L");
            //outlet.push_sample(currentSample);
            sessionID1 = sinHEffect.FindObjectOfType<sinHEffect>().sessionID;
            log();
            Distance();
        }

        //public bool started = false;
        //public bool path_finished = false;//Jing4
        //public bool all_end = false;//Jing4
        public void ifstarted()
        {
            started = true;
            //path_head.position = new Vector3(-3.5f, 0.0f, 0f);
            //path_tail.position = new Vector3(3.5f, 0.0f, 0f);
            //ans = GameObject.Find("StartButton");
            //ans.SetActive(false);
        }
        /*public void judge_distance(double distance)
        {
            if (distance <= distance_threshold)
            {
                if (PathFollower.shared != null)
                {
                    PathFollower.shared.started = true;
                }
            }
            else
            {
                //PathFollower.shared.started = false;
            }
        }*/
        void Distance()
        {
            if (pathCreator != null)
            {
                //print("if (pathCreator != null)");
                //if(Input.GetKey(KeyCode.Space))
                //{
                //    started = true;

                //}
                if (started && !path_finished)
                {
                    //print("if (started && !path_finished)");
                    distanceTravelled += speed * Time.deltaTime;
                    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                    //public Vector3 GetPointAtDistance // Gets point on path based on distance travelled.

                    sinHEffect.FindObjectOfType<sinHEffect>().conePosition.x = transform.position.x;
                    sinHEffect.FindObjectOfType<sinHEffect>().conePosition.y = transform.position.y;
                    sinHEffect.FindObjectOfType<sinHEffect>().conePosition.z = transform.position.z;
                    //print("cM:    " + transform.position);
                    //print("conePosition    " + transform.position);
                    //lida//puting markers in every 1mm height


                    if (sessionID1 != 3)//((sessionID1 == 1) || (sessionID1 == 2))
                    {
                        //print("visual_sur");
                        if (path_show.path_show_instace.next_index < 3)
                        {
                            //print("x:    " + transform.position.x);
                            //print("path_show.path_show_instace.next_index    " + path_show.path_show_instace.next_index); 
                            //lida//putting markers after passing 1 mm 2mm and 3 mm according to the position of cone
                            if (Flag_Markers[0] && (transform.position.x + 2.096f) > 0.0f && (transform.position.x + 2.096f) < 0.2f)//else if (Math.Abs(transform.position.x + 2.0962f) < 0.2f)//lida//put a marker when the surface has 2mm height
                            {
                                //lida//marker//here///marker.Write(1002);
                                //currentSample[0] = 301;// "s-1mm-left2";
                                //print("c-1L-301");
                                LSLMarkerCone1[0] = true;
                                //outlet.push_sample(currentSample);
                                //lida//marker
                                Flag_Markers[0] = false;//Lida//putting markers in 1 mm, 2mm and 3 mm heights
                                // lida//logging the cone position
                                string lline3 = "Cone in 1left ===================================================================";//1mm
                                string lline2 = "best position: -2.096";
                                string time1 = "( " + System.DateTime.Now.ToString("HH:mm:ss.fff") + " )";
                                string lline4 = "=================================================================================";
                                string lines1 = "( " + transform.position.x.ToString("0.000") + ", " + transform.position.y.ToString("0.000") + ", " + transform.position.z.ToString("0.000") + " )";
                                //lida//logging the cone position
                                //lida//logging the stylus position
                                HapticPlugin ldevice = sinHEffect.FindObjectOfType<sinHEffect>().devices[0];
                                Vector3 StylusPos = ldevice.stylusPositionWorld;
                                string lines2 = "( " + StylusPos.x.ToString("0.000") + ", " + StylusPos.y.ToString("0.000") + ", " + StylusPos.z.ToString("0.000") + " )";

                                //lida//logging the stylus position
                                string[] lines = { lline3, lline2, lines1, lines2, time1, lline4 };
                                File.AppendAllLines(Path.Combine(sinHEffect.FindObjectOfType<sinHEffect>().docPath, sinHEffect.FindObjectOfType<sinHEffect>().marker_result), lines);

                            }
                            else if (Flag_Markers[1] && (transform.position.x + 1.273f) > 0.0f && (transform.position.x + 1.273f) < 0.2f)//else if (Math.Abs(transform.position.x + 1.273f) < 0.2f)//lida//put a marker when the surface has 1mm height//(transform.position.x == -1.274f)
                            {
                                //lida//marker//here////marker.Write(1001);
                                //currentSample[0] = 302;// "s-2mm-left2";
                                //print("c-2L-302");
                                LSLMarkerCone1[1] = true;
                                //outlet.push_sample(currentSample);
                                //lida//marker
                                Flag_Markers[1] = false;//Lida//putting markers in 1 mm, 2mm and 3 mm heights
                                // lida//logging the cone position
                                string lline3 = "Cone in 2left ===================================================================";//2mm
                                string lline2 = "best position: -1.273";
                                string time1 = "( " + System.DateTime.Now.ToString("HH:mm:ss.fff") + " )";
                                string lline4 = "=================================================================================";
                                string lines1 = "( " + transform.position.x.ToString("0.000") + ", " + transform.position.y.ToString("0.000") + ", " + transform.position.z.ToString("0.000") + " )";
                                //lida//logging the cone position
                                //lida//logging the stylus position
                                HapticPlugin ldevice = sinHEffect.FindObjectOfType<sinHEffect>().devices[0];
                                Vector3 StylusPos = ldevice.stylusPositionWorld;
                                string lines2 = "( " + StylusPos.x.ToString("0.000") + ", " + StylusPos.y.ToString("0.000") + ", " + StylusPos.z.ToString("0.000") + " )";

                                //lida//logging the stylus position
                                string[] lines = { lline3, lline2, lines1, lines2, time1,lline4 };
                                File.AppendAllLines(Path.Combine(sinHEffect.FindObjectOfType<sinHEffect>().docPath, sinHEffect.FindObjectOfType<sinHEffect>().marker_result), lines);

                            }
                            else if (Flag_Markers[2] && ((transform.position.x == 0.0f) || ((transform.position.x > 0.0f) && (transform.position.x < 0.2f))))//if (Math.Abs(transform.position.x - 0.0f) < 0.2f)//lida//put a marker when we are in the top of the hole or bump
                            {//lida//after passsing that point not before it

                                //lida//marker//here////marker.Write(1003);
                                //currentSample[0] = 303;// "s-topbottom";
                                //print("c-TB-303");//topbottom
                                LSLMarkerCone1[2] = true;
                                //outlet.push_sample(currentSample);
                                //lida//marker
                                Flag_Markers[2] = false;//Lida//putting markers in 1 mm, 2mm and 3 mm heights
                                //lida//logging the cone position
                                string lline3 = "Cone in middle ==================================================================";
                                string lline2 = "best position: 0";
                                string time1 = "( " + System.DateTime.Now.ToString("HH:mm:ss.fff") + " )";
                                string lline4 = "=================================================================================";
                                string lines1 = "( " + transform.position.x.ToString("0.000") + ", " + transform.position.y.ToString("0.000") + ", " + transform.position.z.ToString("0.000") + " )";
                                //lida//logging the cone position
                                //lida//logging the stylus position
                                HapticPlugin ldevice = sinHEffect.FindObjectOfType<sinHEffect>().devices[0];
                                Vector3 StylusPos = ldevice.stylusPositionWorld;
                                string lines2 = "( " + StylusPos.x.ToString("0.000") + ", " + StylusPos.y.ToString("0.000") + ", " + StylusPos.z.ToString("0.000") + " )";

                                //lida//logging the stylus position
                                string[] lines = { lline3, lline2, lines1, lines2, time1, lline4 };
                                File.AppendAllLines(Path.Combine(sinHEffect.FindObjectOfType<sinHEffect>().docPath, sinHEffect.FindObjectOfType<sinHEffect>().marker_result), lines);
                            }
                            else if (Flag_Markers[3] && (transform.position.x - 1.273f) > 0.0f && (transform.position.x - 1.273f) < 0.2f)//else if (Math.Abs(transform.position.x - 1.273f) < 0.2f)//lida//put a marker when the surface has 1mm height
                            {
                                //lida//marker//here////marker.Write(1001);

                                //currentSample[0] = 304;// "s-2mm-right2";
                                //print("c-2R-304");//("c-2mm-right");
                                LSLMarkerCone1[3] = true;
                                //outlet.push_sample(currentSample);
                                //lida//marker
                                Flag_Markers[3] = false;//Lida//putting markers in 1 mm, 2mm and 3 mm heights
                                                        //lida//logging the cone position
                                                        //sinHEffect.FindObjectOfType<sinHEffect>().finishedpathFlag = 0;

                                string lline3 = "Cone in 2right ==================================================================";
                                string lline2 = "best position: 1.273";
                                string time1 = "( " + System.DateTime.Now.ToString("HH:mm:ss.fff") + " )";
                                string lline4 = "=================================================================================";
                                string lines1 = "( " + transform.position.x.ToString("0.000") + ", " + transform.position.y.ToString("0.000") + ", " + transform.position.z.ToString("0.000") + " )";
                                //lida//logging the cone position
                                //lida//logging the stylus position
                                HapticPlugin ldevice = sinHEffect.FindObjectOfType<sinHEffect>().devices[0];
                                Vector3 StylusPos = ldevice.stylusPositionWorld;
                                string lines2 = "( " + StylusPos.x.ToString("0.000") + ", " + StylusPos.y.ToString("0.000") + ", " + StylusPos.z.ToString("0.000") + " )";

                                //lida//logging the stylus position
                                string[] lines = { lline3, lline2, lines1, lines2, time1, lline4 };
                                File.AppendAllLines(Path.Combine(sinHEffect.FindObjectOfType<sinHEffect>().docPath, sinHEffect.FindObjectOfType<sinHEffect>().marker_result), lines);

                            }
                            else if (Flag_Markers[4] && (transform.position.x - 2.096f) > 0.0f && (transform.position.x - 2.096f) < 0.2f)//else if (Math.Abs(transform.position.x - 2.0962f) < 0.2f)//lida//put a marker when the surface has 2mm height
                            {
                                //lida//marker//here////marker.Write(1002);

                                //currentSample[0] = 305;// "s-1mm-right2";
                                //print("c-1R-305");
                                LSLMarkerCone1[4] = true;
                                //outlet.push_sample(currentSample);
                                //lida//marker
                                Flag_Markers[4] = false;//Lida//putting markers in 1 mm, 2mm and 3 mm heights
                                //lida//logging the cone position
                                string lline3 = "Cone in 1right ==================================================================";
                                string lline2 = "best position: 2.096";
                                string time1 = "( " + System.DateTime.Now.ToString("HH:mm:ss.fff") + " )";
                                string lline4 = "=================================================================================";
                                string lines1 = "( " + transform.position.x.ToString("0.000") + ", " + transform.position.y.ToString("0.000") + ", " + transform.position.z.ToString("0.000") + " )";
                                //lida//logging the cone position
                                //lida//logging the stylus position
                                HapticPlugin ldevice = sinHEffect.FindObjectOfType<sinHEffect>().devices[0];
                                Vector3 StylusPos = ldevice.stylusPositionWorld;
                                string lines2 = "( " + StylusPos.x.ToString("0.000") + ", " + StylusPos.y.ToString("0.000") + ", " + StylusPos.z.ToString("0.000") + " )";

                                //lida//logging the stylus position
                                string[] lines = { lline3,lline2 ,lines1, lines2, time1, lline4 };
                                File.AppendAllLines(Path.Combine(sinHEffect.FindObjectOfType<sinHEffect>().docPath, sinHEffect.FindObjectOfType<sinHEffect>().marker_result), lines);

                            }


                            //float dis_ref = visual_sur.GetDistanceToPoint(transform.position);//transform.position=conePosition
                            //float dist = Vector3.Distance(sinHEffect.FindObjectOfType<sinHEffect>().conePosition, visual_sur.transform.position);//lida//this distance calculate the distance between the center of two subjects that is not correct
                            //print("Distance       " + dist);
                        }

                    }

                    //else print("nooooo");
                    //lida //puting markers in every 1mm height
                    if (distanceTravelled >= pathCreator.path.length)//lida//finishing one path
                    {
                        //print("distanceTravelled >= pathCreator.path.length");
                        //lida//making the  path_finished, true after finishing one path and at the beginning of each path make it false in switch_to() in PathFollower script
                        path_finished = true; //sinHEffect.FindObjectOfType<sinHEffect>().finishedpathFlag = 1; //prsdfint(path_finished);
                                              //time_path_en = System.DateTime.Now; print(time_path_en.Subtract(time_path_be));
                                              //   print(System.DateTime.Now.ToString("HH:mm:ss.fff tt"));
                    }
                }

                //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }

        }


        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

    }
}
