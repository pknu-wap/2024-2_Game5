using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISizeControl : MonoBehaviour
{
    float Screen_w = (float)Screen.width;
    float Screen_h = (float)Screen.height;
    // Start is called before the first frame update
    void Start()
    {
        //ȭ�������ŭ �⺻ ũ�⿡ ���ϱ� - �̹��� ũ�� ����(�پ��� ȭ�� ũ�⿡�� �Ϻ����� ������, ��ü������ ����ϰ� ����)
        //recttransform.localscale = new Vector2(transform.localScale.x * (Screen_w / Screen_h), transform.localScale.y * (Screen_w / Screen_h));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
