using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittest : MonoBehaviour
{
    public DummyHit dummy; //����ƺ��� DummyHit ����
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // ����:�����̽� Ű
        {
            dummy.OnHit(); // ����ƺ� ����
        }
    }
}
