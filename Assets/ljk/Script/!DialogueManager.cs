using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class nDialogueManager : MonoBehaviour, IPointerDownHandler
{
    public Text DialogueText;
    public GameObject NextText;
    public CanvasGroup DialogueGroup;
    public Queue<string> Sentences;

    public string currentSentence;

    public float typingSpeed = 0.1f;
    private bool istyping;

    public static nDialogueManager instance;   //영상에선 있던데
    private void Awake()
    {
        //instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Sentences = new Queue<string>(); //초기화
    }

    public void Ondialogue(string[] lines) 
    {
        Sentences.Clear();
        foreach (string line in lines)
        {
            Sentences.Enqueue(line);
        }
        DialogueGroup.alpha = 1;
        DialogueGroup.blocksRaycasts = true;  

        NextSentence();

    }

    public void NextSentence()
    {
        if(Sentences.Count != 0)
        {
            currentSentence = Sentences.Dequeue();
            //대화시작, 다음대화 비활성화
            istyping = true;
            NextText.SetActive(false); 
            //코루틴
            StartCoroutine(Typing(currentSentence));
        }
        else
        {
            DialogueGroup.alpha = 1;
            //DialogueGroup.blockRaycasts = false;
        }
    }

    IEnumerator Typing(string line)
    {
        DialogueText.text = ""; //초기화
        foreach(char letter in line.ToCharArray()) //한 글자씩 더함
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(DialogueText.text.Equals(currentSentence)) //현재 대사가 다 출력되면
        {
            //대화 끝, 다음대화 활성화
            NextText.SetActive(true);
            istyping = false;
        }
    }

    //마우스클릭 시 또는 물리충돌 시 출력시키기, NPC스크립트랑 연결
    public void OnPointerDown(PointerEventData eventData) 
    {
        if(!istyping)
        NextSentence();
    }
}
