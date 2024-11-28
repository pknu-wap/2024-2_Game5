using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Scene Seen; //���� �� ������ ����

    void Start()
    {
        Seen = SceneManager.GetActiveScene();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(Seen.buildIndex); //����� �ٽ� �����ϱ�
        }
    }


}
