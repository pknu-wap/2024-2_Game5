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
        //ī�޶� ��ġ ���� - �̵� �Ÿ� ��꿡 ���
        CameraLastPosition = CameraTransform.position;

        //int BackGroundCount = transform.childCount;
        //GameObject[] BackGrounds = new GameObject[BackGroundCount];


        material = GetComponent<Renderer>().material;

    }



    // Update is called once per frame
    private void Update()
    {
        //ī�޶� �̵��� �Ÿ� = ���� ��ġ - ���� ��ġ
        Distance = CameraTransform.position.x - CameraLastPosition.x;
        //����� x ��ġ�� ���� ī�޶��� x ��ġ�� ����
        //transform.position = new Vector2(CameraTransform.position.x, transform.position.y);

        //����ȯ(������)
        material.SetTextureOffset("_MainTex", new Vector2(Distance, 0) * ParallaxSpeed);
        

    }

}
