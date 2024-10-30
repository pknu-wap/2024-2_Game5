using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable] //직접 만든 class에 접근할 수 있도록 해줌. 
public class DialogueList //대화 저장할 리스트임. 다른 게임오브젝트같은 거 넣지말기
{     
    [TextArea]//한줄 말고 여러 줄 쓸 수 있게 해줌
    public string dialogue;
}
public class Dialog : MonoBehaviour
{
    //SerializeField : inspector창에서 직접 접근할 수 있도록 하는 변수임.
    [SerializeField] private Image sprite_DialogueBox; //대사창 이미지(crop)를 제어하기 위한 변수
    [SerializeField] private Text txt_Dialogue; // 텍스트를 제어하기 위한 변수

    private bool isDialogue = false; //대화가 진행중인지 알려줄 변수
    private int count = 0; //대사가 얼마나 진행됐는지 알려줄 변수

    

    [SerializeField] private DialogueList[] dialogue; //위에 리스트 가져오기 
    private Scene Seen; //현재 씬을 저장할 함수

    void Start()
    {
        Seen = SceneManager.GetActiveScene();
        Invoke("ShowDialogue", 1.5f);
    }
    public void ShowDialogue()
    {
        ONOFF(true); //대화가 시작됨
        count = 0;
        NextDialogue(); //호출되자마자 대사가 진행될 수 있도록 
    }

    private void ONOFF(bool _flag)
    {
        sprite_DialogueBox.gameObject.SetActive(_flag);        
        txt_Dialogue.gameObject.SetActive(_flag);
        isDialogue = _flag;
    }

    private void NextDialogue() { 
        //첫번째 대사와 첫번째 cg부터 계속 다음 cg로 진행되면서 화면에 보이게 된다. 
        txt_Dialogue.text = dialogue[count].dialogue;
        count++; //다음 대사와 cg가 나오도록 
    }
 

    // Update is called once per frame
    void Update()
    {
        
        if (isDialogue) //활성화가 되었을 때만 대사가 진행되도록
        {
            if (Input.GetKeyDown(KeyCode.Space)) //spacebar 누를 때마다 대사가 진행되도록
            { 
                //대화가 안 끝났는가
                if (count < dialogue.Length) NextDialogue(); //다음 대사가 진행됨
                else
                {
                    ONOFF(false); //대사가 끝남
                    //SceneManager.LoadScene(Seen.buildIndex+1); //여기 위에 페이드아웃 넣어주고 로드씬 천천히 하면 좋을 듯
                }
            }
        }

    }
}