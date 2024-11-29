using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable] //���� ���� class�� ������ �� �ֵ��� ����. 
public class MiddleDialogueList //��ȭ ������ ����Ʈ��. �ٸ� ���ӿ�����Ʈ���� �� ��������
{
    [TextArea]//���� ���� ���� �� �� �� �ְ� ����
    public string dialogue;
}
public class MiddleDialog : MonoBehaviour
{
    //SerializeField : inspectorâ���� ���� ������ �� �ֵ��� �ϴ� ������.
    [SerializeField] private Image sprite_DialogueBox; //���â �̹���(crop)�� �����ϱ� ���� ����
    [SerializeField] private Text txt_Dialogue; // �ؽ�Ʈ�� �����ϱ� ���� ����

    private bool isDialogue = false; //��ȭ�� ���������� �˷��� ����
    private int Count = 0; //��簡 �󸶳� ����ƴ��� �˷��� ����

    public bool isDialogueFinished;



    [SerializeField] private DialogueList[] dialogue; //���� ����Ʈ �������� 
    //private Scene Seen; //���� ���� ������ �Լ�

    void Start()
    {
        ShowDialogue();
        //Seen = SceneManager.GetActiveScene();        
    }
    public void ShowDialogue()
    {
        isDialogueFinished = false;

        ONOFF(true); //��ȭ�� ���۵�
        Count = 0;
        NextDialogue(); //ȣ����ڸ��� ��簡 ����� �� �ֵ��� 
    }

    private void ONOFF(bool _flag)
    {
        sprite_DialogueBox.gameObject.SetActive(_flag);
        txt_Dialogue.gameObject.SetActive(_flag);
        isDialogue = _flag;
    }

    private void NextDialogue()
    {
        //ù��° ���� ù��° cg���� ��� ���� cg�� ����Ǹ鼭 ȭ�鿡 ���̰� �ȴ�. 
        txt_Dialogue.text = dialogue[Count].dialogue;
        Count++; //���� ���� cg�� �������� 
    }


    // Update is called once per frame
    void Update()
    {

        if (isDialogue) //Ȱ��ȭ�� �Ǿ��� ���� ��簡 ����ǵ���
        {
            if (Input.GetKeyDown(KeyCode.Space)) //spacebar ���� ������ ��簡 ����ǵ���
            {
                //��ȭ�� �� �����°�
                if (Count < dialogue.Length) NextDialogue(); //���� ��簡 �����
                else
                {
                    ONOFF(false); //��簡 ����   
                    isDialogueFinished = true;                 
                }
            }
        }

    }
}