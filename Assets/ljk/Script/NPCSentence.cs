using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSentence : MonoBehaviour
{

    public string[] Senetences; //��ȭ ����

    private void OnmouseDown()  //���콺 ������ ��
    {
        if(DialogueManager.instance.DialogueGroup.alpha == 0)
        DialogueManager.instance.Ondialogue(Senetences);
    }
}
