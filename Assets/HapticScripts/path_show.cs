using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class path_show : MonoBehaviour
{

    public static path_show path_show_instace;
    public int horizontal_number;//lida//2
    public int vertical_number;//lida//2
    public float path_coverage; // 0.90//lida//0.7//converted it to 1 to prolong the path
    public float plane_wide;//lida//8

    public float horizontal_step = 0;//lida//1
    public float vertical_step = 0;//lida//1
    public float path_length = 0;//lida//8
    public float path_half_length = 0;//lida//4

    public float[,] path_points = null;
    public float[] path_origin = null; // 0 for x, 1 for y, 2 for z
    public float[] plane_size = null; // 0 for width, 1 for height

    public float z = 0;

    public bool is_first = true;
    public int next_index = 0;//lida//0

    // Start is called before the first frame update
    void Start()
    {
        path_show_instace = this;
        plane_size = new float[2];
        path_origin = new float[3];
        path_points = new float[horizontal_number + vertical_number, 6];//lida//[4,6]//4 paths-each path with the start point (x1,y1,z1) and the end point (x2,y2,z2)

        GameObject plane = GameObject.Find("Plane");
        plane_size[0] = plane.transform.localScale.x * plane_wide;// 0 for width//plane_wide=8
        plane_size[1] = plane.transform.localScale.z * plane_wide;// 1 for height//plane_wide=8
        path_origin[0] = plane.transform.position.x;//0
        path_origin[1] = plane.transform.position.y;//0
        path_origin[2] = plane.transform.position.z;//0

        //horizantal step 3 but the same place
        //horizontal_step = plane_size[1] / (horizontal_number + 1);//lida(Deng has this part + 0.8f);//(8/3+0.8)=3.33
        horizontal_step = plane_size[1] / (1 + 1);//chenges here
        vertical_step = plane_size[0] / (vertical_number + 1);//lida(Deng has this part + 0.8f);//(8/3+0.8)=3.33

        z = path_origin[2];
        path_length = path_coverage * (plane_size[0] + plane_size[1]) / 2;//lida//(0.7*(8+8)/2)=5.6//in scene=8
        path_half_length = path_length / 2;//lida//in scene=4

        float y_top = path_origin[1] + plane_size[1] / 2;//Lida(Deng has this part + 1.6f;)//0+8/2+1.6=5.6
        float y_first = y_top - horizontal_step;

        float x_left = path_origin[0] - plane_size[0] / 2;//Lida (Deng has this part- 1.6f; )
        float x_first = x_left + vertical_step;

        for (int i = 0; i < horizontal_number; i++)//horizontal_number=2
        {
            //lida//start and end of the horizontal paths
            //float delta_y = horizontal_step * i;
            float delta_y = 0.0f;//chenges here
            path_points[i, 0] = path_origin[0] - path_half_length; // x1
            path_points[i, 1] = y_first - delta_y;                 // y1
            path_points[i, 2] = z;                                 // z1
            path_points[i, 3] = path_origin[0] + path_half_length; // x2
            path_points[i, 4] = path_points[i, 1];                 // y2
            path_points[i, 5] = z;                                 // z2
        }

        for (int i = 0; i < vertical_number; i++)
        {
            //lida//start and end of the vertical paths
            float delta_x = vertical_step * i;
            path_points[i + horizontal_number, 0] = x_first + delta_x;                   // x1
            path_points[i + horizontal_number, 1] = path_origin[1] + path_half_length;  // y1
            path_points[i + horizontal_number, 2] = z;                                  // z1
            path_points[i + horizontal_number, 3] = x_first + delta_x;                   // x2
            path_points[i + horizontal_number, 4] = path_origin[1] - path_half_length;  // y2
            path_points[i + horizontal_number, 5] = z;                                  // z2

        }

        //lida//3 horizaltal path in three same vertical distance 
        /*
        horizontal_step = plane_size[1] / (horizontal_number + 1);//lida(Deng has this part + 0.8f);//(8/3+0.8)=3.33
        vertical_step = plane_size[0] / (vertical_number + 1);//lida(Deng has this part + 0.8f);//(8/3+0.8)=3.33

        z = path_origin[2];
        path_length = path_coverage * (plane_size[0] + plane_size[1]) / 2;//lida//(0.7*(8+8)/2)=5.6//in scene=8
        path_half_length = path_length / 2;//lida//in scene=4

        float y_top = path_origin[1] + plane_size[1] / 2;//Lida(Deng has this part + 1.6f;)//0+8/2+1.6=5.6
        float y_first = y_top - horizontal_step;

        float x_left = path_origin[0] - plane_size[0] / 2;//Lida (Deng has this part- 1.6f; )
        float x_first = x_left + vertical_step;

        for (int i = 0; i < horizontal_number; i++)//horizontal_number=2
        {
            //lida//start and end of the horizontal paths
            float delta_y = horizontal_step * i;
            path_points[i, 0] = path_origin[0] - path_half_length; // x1
            path_points[i, 1] = y_first - delta_y;                 // y1
            path_points[i, 2] = z;                                 // z1
            path_points[i, 3] = path_origin[0] + path_half_length; // x2
            path_points[i, 4] = path_points[i, 1];                 // y2
            path_points[i, 5] = z;                                 // z2
        }

        for (int i = 0; i < vertical_number; i++)
        {
            //lida//start and end of the vertical paths
            float delta_x = vertical_step * i;
            path_points[i + horizontal_number, 0] = x_first + delta_x;                   // x1
            path_points[i + horizontal_number, 1] = path_origin[1] + path_half_length;  // y1
            path_points[i + horizontal_number, 2] = z;                                  // z1
            path_points[i + horizontal_number, 3] = x_first + delta_x;                   // x2
            path_points[i + horizontal_number, 4] = path_origin[1] - path_half_length;  // y2
            path_points[i + horizontal_number, 5] = z;                                  // z2

        }*/
        //lida//3 horizaltal path in three same vertical distance 

        //for(int i = 0; i < vertical_number; i++)
        //{
        //    for(int j = 0; j < 6; j++)
        //    {
        //        Debug.Log(path_points[horizontal_number+i, j]);
        //    }
        //}

        //lida//changed the beging and end of the path
        /*plane_size[0] = plane.transform.localScale.x * plane_wide;// 0 for width//plane_wide=8
        plane_size[1] = plane.transform.localScale.z * plane_wide;// 1 for height//plane_wide=8
        path_origin[0] = plane.transform.position.x;//0
        path_origin[1] = plane.transform.position.y;//0
        path_origin[2] = plane.transform.position.z;//0

        horizontal_step = plane_size[1] / (horizontal_number + 1) + 0.8f;//lida//(8/3+0.8)=3.33
        vertical_step = plane_size[0] / (vertical_number + 1) + 0.8f; //lida//(8/3+0.8)=3.33

        z = path_origin[2];
        path_length = path_coverage * (plane_size[0] + plane_size[1]) / 2;//lida//(0.7*(8+8)/2)=5.6//in scene=8
        path_half_length = path_length / 2;//lida//in scene=4

        float y_top = path_origin[1] + plane_size[1] / 2 + 1.6f;//Lida//0+8/2+1.6=5.6
        float y_first = y_top - horizontal_step;

        float x_left = path_origin[0] - plane_size[0] / 2 - 1.6f;
        float x_first = x_left + vertical_step;

        for (int i = 0; i < horizontal_number; i++)//horizontal_number=2
        {
            //lida//start and end of the horizontal paths
            float delta_y = horizontal_step * i;
            path_points[i, 0] = path_origin[0] - path_half_length; // x1
            path_points[i, 1] = y_first - delta_y;                 // y1
            path_points[i, 2] = z;                                 // z1
            path_points[i, 3] = path_origin[0] + path_half_length; // x2
            path_points[i, 4] = path_points[i, 1];                 // y2
            path_points[i, 5] = z;                                 // z2
        }

        for (int i = 0; i < vertical_number; i++)
        {
            //lida//start and end of the vertical paths
            float delta_x = vertical_step * i;
            path_points[i + horizontal_number, 0] = x_first + delta_x;                   // x1
            path_points[i + horizontal_number, 1] = path_origin[1] + path_half_length;  // y1
            path_points[i + horizontal_number, 2] = z;                                  // z1
            path_points[i + horizontal_number, 3] = x_first + delta_x;                   // x2
            path_points[i + horizontal_number, 4] = path_origin[1] - path_half_length;  // y2
            path_points[i + horizontal_number, 5] = z;                                  // z2

        }*/
        //lida//changed the beging and end of the path
    }


    void next_path()
    {   //lida//seem//switching to a new path and putting the cone in the position  
        //if (next_index <= (horizontal_number + vertical_number))
        //{
            //print("next_pathhhhhhhhhhhhhhhhh");
            PathFollower.shared.switch_to(
                path_points[next_index, 0],
                path_points[next_index, 1],
                path_points[next_index, 2],
                path_points[next_index, 3],
                path_points[next_index, 4],
                path_points[next_index, 5]

        );
       // }
        next_index++;
    }

    // Update is called once per frame
    void Update()//lida//this is used when one path is finished 
    {
        //prasdfint(HapticHand.FindObjectOfType<HapticHand>().buttonHoldDown);
        //if (HapticHand.FindObjectOfType<HapticHand>().buttonHoldDown == true)
        //{
        //    prisdfnt(HapticHand.FindObjectOfType<HapticHand>().buttonHoldDown);
        //    HapticHand.FindObjectOfType<HapticHand>().buttonHoldDown = false;
        if (is_first)//lida//it is true at first and at the end of each block we make it true for the next block and after starting each block we make it false 
        {
            if (PathFollower.shared != null)
            {
                next_index = 0; PathFollower.shared.all_end = false;
                is_first = false;//lida//after starting each block we make it false
                next_path();
                //print("is_firstttttttttttttttttt");
            }
        }
        //// Detect current state of pathfollower : finished or not
        if (PathFollower.shared != null)
        {
            if (PathFollower.shared.path_finished)//lida//seems//just one path is finished
            {
                //print("PathFollower.shared.path_finisheddddddddddddddddddddddddddddd");
                //PathFollower.shared.path_finished = true;
                if (next_index == (horizontal_number + vertical_number))//lida//seems//all the paths are finished
                {
                    PathFollower.shared.all_end = true;
                    PathFollower.shared.started = false; //System.Threading.Thread.Sleep(5000);                
                                                         //PathFollower.shared.path_finished = false;
                                                         //System.Threading.Thread.Sleep(2000);
                    next_index++;//lida it was here before
                                 //print("Tracying all paths done!"); sinHEffect.FindObjectOfType<sinHEffect>().marker.Write(7); //print("-------------end");
                    return;
                }

                // switch to next
                //print("path_shawwww");
                bool flag_button = false;
                if (sinHEffect.FindObjectOfType<sinHEffect>().trust_yes.gameObject.activeSelf || sinHEffect.FindObjectOfType<sinHEffect>().YesBu.gameObject.activeSelf || sinHEffect.FindObjectOfType<sinHEffect>().nextBu.gameObject.activeSelf || sinHEffect.FindObjectOfType<sinHEffect>().doneText_session_all.activeSelf)
                    flag_button = true;
                if(!flag_button)
                    next_path();

            }
        }
        //}
    }
}
