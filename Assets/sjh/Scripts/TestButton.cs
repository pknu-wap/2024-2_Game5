using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour
{
    
    public GameObject monsters;
    public void LoadNextScene(string sceneName)
    {
        // 지정한 씬으로 이동
        SceneManager.LoadScene(sceneName);
    }

    public void KillAllMonster()
    {
        monsters.SetActive(false);
    }
}
