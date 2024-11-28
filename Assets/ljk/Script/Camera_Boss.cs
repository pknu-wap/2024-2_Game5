using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;




public class Camera_Boss : MonoBehaviour
{
    Transform Player_tr;
    Transform Boss_tr;
    Transform Cam_tr;

    float LastX = 0;
    float MidPoint;

    Transform LastPosition;
    Transform TargetPosition;
    // Start is called before the first frame update
    void Start()
    {
        Player_tr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Boss_tr = GameObject.FindGameObjectWithTag("Monster").GetComponent<Transform>();
        Cam_tr = GetComponent<Transform>();

        //CameraMove();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (-5 < LastX && LastX < 5)
        {
            MidPoint = (Player_tr.transform.position.x + Boss_tr.transform.position.x) / 2;
            Cam_tr.position = new Vector3(Mathf.Lerp(LastX, MidPoint, 0.5f), 0, -10);
            LastX = Cam_tr.position.x;
        }
        if ( LastX < -5)
        {
            Cam_tr.position = new Vector3(-5, 0, -10);
            LastX = -4.9f;
        }
        if  ( LastX > 5)
        {
            Cam_tr.position = new Vector3(5, 0, -10);
            LastX = 4.9f;
        }
    }
}
