using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittest : MonoBehaviour
{
    public DummyHit dummy; //허수아비의 DummyHit 연결
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 공격:스페이스 키
        {
            dummy.OnHit(); // 허수아비 때림
        }
    }
}
