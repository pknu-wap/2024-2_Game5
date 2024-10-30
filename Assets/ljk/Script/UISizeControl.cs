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
        //화면비율만큼 기본 크기에 곱하기 - 이미지 크기 조절(다양한 화면 크기에서 완벽하진 않지만, 대체적으로 비슷하게 보임)
        //recttransform.localscale = new Vector2(transform.localScale.x * (Screen_w / Screen_h), transform.localScale.y * (Screen_w / Screen_h));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
