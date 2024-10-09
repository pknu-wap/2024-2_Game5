using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Text field�� ����� �� �ֵ��� �ϴ� header

[System.Serializable] //���� ���� class�� ������ �� �ֵ��� ����. 
public class DialogueList {
    [TextArea]//���� ���� ���� �� �� �� �ְ� ����
    public string dialogue;

}
public class Dialog : MonoBehaviour
{
    //SerializeField : inspectorâ���� ���� ������ �� �ֵ��� �ϴ� ������.
    [SerializeField] private Image sprite_DialogueBox; //���â �̹���(crop)�� �����ϱ� ���� ����
    [SerializeField] private Text txt_Dialogue; // �ؽ�Ʈ�� �����ϱ� ���� ����

    private bool isDialogue = false; //��ȭ�� ���������� �˷��� ����
    private int count = 0; //��簡 �󸶳� ����ƴ��� �˷��� ����

    [SerializeField] private DialogueList[] dialogue;

   void Start()
    {
        Invoke("ShowDialogue", 3f);
    }
    public void ShowDialogue()
    {
        ONOFF(true); //��ȭ�� ���۵�
        count = 0;
        NextDialogue(); //ȣ����ڸ��� ��簡 ����� �� �ֵ��� 
    }

    private void ONOFF(bool _flag)
    {
        sprite_DialogueBox.gameObject.SetActive(_flag);        
        txt_Dialogue.gameObject.SetActive(_flag);
        isDialogue = _flag;
    }

    private void NextDialogue() { 
        //ù��° ���� ù��° cg���� ��� ���� cg�� ����Ǹ鼭 ȭ�鿡 ���̰� �ȴ�. 
        txt_Dialogue.text = dialogue[count].dialogue;
        count++; //���� ���� cg�� �������� 
    
    }
 

    // Update is called once per frame
    void Update()
    {
        //spacebar ���� ������ ��簡 ����ǵ���. 
        if (isDialogue) //Ȱ��ȭ�� �Ǿ��� ���� ��簡 ����ǵ���
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                //��ȭ�� ���� �˾ƾ���.
                if (count < dialogue.Length) NextDialogue(); //���� ��簡 �����
                else ONOFF(false); //��簡 ����
            
            }
        }

    }
}