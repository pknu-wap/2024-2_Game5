using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeManager : MonoBehaviour
{
    public Text VisibleTime;
    public GameObject Gameover;  //Hierarchy에서 게임오버화면 붙여줘야함
    int Time;
    

    // Start is called before the first frame update
    void Start()
    {        
        Time = 60;
        Invoke("TimeGoes", 1f);

    }
    void TimeGoes() //시간의 ㅎ름을 텍스트에 적용, 시간이 다 지났는지도 확인
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
