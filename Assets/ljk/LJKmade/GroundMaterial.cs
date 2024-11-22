using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMaterial : MonoBehaviour
{   
    [SerializeField]
    Transform CameraTransform;
    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float ParallaxSpeed;

    Vector3 CameraLastPosition;
    float Distance;
    
    private Material material;


    // Start is called before the first frame update
    private void Awake()
    {
        //카메라 위치 저장 - 이동 거리 계산에 사용
        CameraLastPosition = CameraTransform.position;

        //int BackGroundCount = transform.childCount;
        //GameObject[] BackGrounds = new GameObject[BackGroundCount];


        material = GetComponent<Renderer>().material;

    }



    // Update is called once per frame
    private void Update()
    {
        //카메라가 이동한 거리 = 현재 위치 - 이전 위치
        Distance = CameraTransform.position.x - CameraLastPosition.x;
        //배경의 x 위치를 현재 카메라의 x 위치로 설정
        //transform.position = new Vector2(CameraTransform.position.x, transform.position.y);

        //배경순환(움직임)
        material.SetTextureOffset("_MainTex", new Vector2(Distance, 0) * ParallaxSpeed);
        

    }

}
