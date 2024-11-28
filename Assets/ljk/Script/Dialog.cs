using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable] //���� ���� class�� ������ �� �ֵ��� ����. 
public class DialogueList //��ȭ ������ ����Ʈ��. �ٸ� ���ӿ�����Ʈ���� �� ��������
{     
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
    public string sceneName = "NormalSceneTuto";

    

    [SerializeField] private DialogueList[] dialogue; //���� ����Ʈ �������� 
    private Scene Seen; //���� ���� ������ �Լ�

    void Start()
    {
        Seen = SceneManager.GetActiveScene();
        Invoke("ShowDialogue", 1.5f);
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
        
        if (isDialogue) //Ȱ��ȭ�� �Ǿ��� ���� ��簡 ����ǵ���
        {
            if (Input.GetKeyDown(KeyCode.Space)) //spacebar ���� ������ ��簡 ����ǵ���
            { 
                //��ȭ�� �� �����°�
                if (count < dialogue.Length) NextDialogue(); //���� ��簡 �����
                else
                {
                    ONOFF(false); //��簡 ����
                    SceneManager.LoadScene(sceneName); //���� ���� ���̵�ƿ� �־��ְ� �ε�� õõ�� �ϸ� ���� ��
                }
            }
        }

    }
}