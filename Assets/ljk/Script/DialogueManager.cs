using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour //, IPointerDownHandler //Ŭ�� �Ǵ� ���� �浹 �� ���
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

    //------------��ư���� �۵� ��Ű�� ���------------------
    public void ToNextText()
    {
        if (CurrentText != NextText) //�������� ���� == ������ ��
        {
            CurrentText.SetActive(false);
            NextText.SetActive(true);
            CurrentText = NextText;
            NextText = ThirdText;
            //ThirdText = 
       }
       else  //�������� ��� ����ȭ ��Ŵ, �ؽ�Ʈ ��ü ��Ȱ��ȭ
        {
            DialogueGroup.alpha = 0;
            CurrentText.SetActive(false);
            //NextText.SetActive(false);
        }

    }
}
