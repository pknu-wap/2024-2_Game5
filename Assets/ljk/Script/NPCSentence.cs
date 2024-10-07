using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSentence : MonoBehaviour
{

    public string[] Senetences; //대화 저장

    private void OnmouseDown()  //마우스 눌렸을 때
    {
        if(DialogueManager.instance.DialogueGroup.alpha == 0)
        DialogueManager.instance.Ondialogue(Senetences);
    }
}
