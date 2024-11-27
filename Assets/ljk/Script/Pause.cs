using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    //public GameObject ThisScreen;
    public GameObject Resume_Btn;
    public GameObject Restart_Btn;
    public GameObject Main_Btn;
    public GameObject Exit_Btn;
    private Scene Seen; //save Current Scene to restart

    [SerializeField] private GameObject PauseScreen;  //Paste this to Player Script

    /* Player 스크립트에 추가 (Paste this on Player Script-Belt, Boss both)
    public void update()
    {
        //active pause screen with 'esc' key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key was pressed");
            
            PauseScreen.gameObject.SetActive(true);  
            Time.timeScale = 0;
        }
        
    }
    */
    private void Awake()
    {
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Seen = SceneManager.GetActiveScene();
    }

    public void Resume_Btn_click()
    {
        Time.timeScale = 1;
        PauseScreen.gameObject.SetActive(false);
    }

    public void Restart_Btn_click()
    {
        SceneManager.LoadScene(Seen.buildIndex);
    }

    public void Main_Btn_clik()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit_Btn_click()
    {
        Application.Quit();
    }

}
