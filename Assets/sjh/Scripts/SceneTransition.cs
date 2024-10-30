using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Text text;
    public bool isPlayerEntered = false;
    // Start is called before the first frame update
    void Start()
    {
        text.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        Debug.Log("Player Entered.");
        if (col.gameObject.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);
            isPlayerEntered = true;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.L) && isPlayerEntered)
            {
                Debug.Log("L key is pressed");
                SceneManager.LoadScene("shjTestScene");
            }
    }
}
