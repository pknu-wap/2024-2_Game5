using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeManager : MonoBehaviour
{
    public Text VisibleTime;
    public GameObject Gameover;  //Hierarchy���� ���ӿ���ȭ�� �ٿ������
    int Time;
    

    // Start is called before the first frame update
    void Start()
    {        
        Time = 60;
        Invoke("TimeGoes", 1f);

    }
    void TimeGoes() //�ð��� ������ �ؽ�Ʈ�� ����, �ð��� �� ���������� Ȯ��
    {
        Time -= 1;
        VisibleTime.text = Time.ToString();
        if (Time == 0) Gameover.SetActive(true);
        else Invoke("TimeGoes", 1f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
