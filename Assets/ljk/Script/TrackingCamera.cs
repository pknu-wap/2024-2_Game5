using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{

    [SerializeField]
    Transform CameraTransform;
    Vector3 CameraLastPosition;
    float Distance;

    // Start is called before the first frame update
    void Start()
    {
        //카메라 위치 저장 - 이동 거리 계산에 사용
        CameraLastPosition = CameraTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //카메라가 이동한 거리 = 현재 위치 - 이전 위치
        Distance = CameraTransform.position.x - CameraLastPosition.x;
        //배경의 x 위치를 현재 카메라의 x 위치로 설정
        transform.position = new(CameraTransform.position.x, transform.position.y, transform.position.z);
    }
}
