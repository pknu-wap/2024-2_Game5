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
        //ī�޶� ��ġ ���� - �̵� �Ÿ� ��꿡 ���
        CameraLastPosition = CameraTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //ī�޶� �̵��� �Ÿ� = ���� ��ġ - ���� ��ġ
        Distance = CameraTransform.position.x - CameraLastPosition.x;
        //����� x ��ġ�� ���� ī�޶��� x ��ġ�� ����
        transform.position = new(CameraTransform.position.x, transform.position.y, transform.position.z);
    }
}
