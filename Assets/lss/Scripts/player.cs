using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public class Player : MonoBehaviour
    {
        public float playerHP = 100f;  // �÷��̾��� HP

        // �÷��̾��� HP�� ��� �Լ�
        public void TakeDamage(float damage)
        {
            playerHP -= damage;
            Debug.Log("Player HP: " + playerHP);

            if (playerHP <= 0) // HP�� 0�� �Ǹ� game over
            {
                Debug.Log("Player is dead!");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
