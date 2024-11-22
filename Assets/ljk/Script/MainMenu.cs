using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject Start_Btn;
    public GameObject Load_Btn;
    public GameObject Exit_Btn;
    //public GameObject mainMenu; �ɼǸ���� �̰� SetActive(false)�ϸ鼭
    //public GameObject optionMenu;; �ɼǸ޴� SetActive(true)�ϸ� ��

    // Start is called before the first frame update
    public void Start_Btn_click()
    {
        //���θ޴��� ��0�̶�� ����
        SceneManager.LoadScene("PrologueStory");
    }


    public void Exit_Btn_click()
    {
        Application.Quit();
    }




}
