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

    public static nDialogueManager instance;   //���󿡼� �ִ���
    private void Awake()
    {
        //instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Sentences = new Queue<string>(); //�ʱ�ȭ
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
            //��ȭ����, ������ȭ ��Ȱ��ȭ
            istyping = true;
            NextText.SetActive(false); 
            //�ڷ�ƾ
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
        DialogueText.text = ""; //�ʱ�ȭ
        foreach(char letter in line.ToCharArray()) //�� ���ھ� ����
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(DialogueText.text.Equals(currentSentence)) //���� ��簡 �� ��µǸ�
        {
            //��ȭ ��, ������ȭ Ȱ��ȭ
            NextText.SetActive(true);
            istyping = false;
        }
    }

    //���콺Ŭ�� �� �Ǵ� �����浹 �� ��½�Ű��, NPC��ũ��Ʈ�� ����
    public void OnPointerDown(PointerEventData eventData) 
    {
        if(!istyping)
        NextSentence();
    }
}
