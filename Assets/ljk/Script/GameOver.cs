using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject Player;
    private Scene Seen; //현재 씬 저장할 변수

    // Start is called before the first frame update
    void Start()
    {
        Seen = SceneManager.GetActiveScene();
    }

    void Awake()
    {
        Player.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(Seen.buildIndex); //현재씬 다시 시작하기
        }
    }
}
