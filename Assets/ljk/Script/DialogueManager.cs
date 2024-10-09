using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour //, IPointerDownHandler //클릭 또는 물리 충돌 시 사용
{
    public GameObject CurrentText;
    public GameObject NextText;
    public GameObject ThirdText;
    public CanvasGroup DialogueGroup;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        DialogueGroup = GetComponent<CanvasGroup>();         
    }

    //------------버튼으로 작동 시키는 기능------------------
    public void ToNextText()
    {
        if (CurrentText != NextText) //마지막엔 현재 == 다음이 됨
        {
            CurrentText.SetActive(false);
            NextText.SetActive(true);
            CurrentText = NextText;
            NextText = ThirdText;
            //ThirdText = 
       }
       else  //마지막인 경우 투명화 시킴, 텍스트 전체 비활성화
        {
            DialogueGroup.alpha = 0;
            CurrentText.SetActive(false);
            //NextText.SetActive(false);
        }

    }
}
