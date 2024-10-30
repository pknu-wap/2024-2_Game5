using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject Start_Btn;
    public GameObject Load_Btn;
    public GameObject Exit_Btn;
    //public GameObject mainMenu; 옵션만들면 이거 SetActive(false)하면서
    //public GameObject optionMenu;; 옵션메뉴 SetActive(true)하면 됨

    // Start is called before the first frame update
    public void Start_Btn_click()
    {
        //메인메뉴가 씬0이라는 가정
        SceneManager.LoadScene(1);
    }


    public void Exit_Btn_click()
    {
        Application.Quit();
    }




}
