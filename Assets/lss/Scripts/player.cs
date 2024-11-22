using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float playerHP = 100f;  // �÷��̾��� HP

    // �÷��̾��� HP�� ��� �Լ�
    public void TakeDamage(float damage)
    {
        playerHP -= damage;
        Debug.Log("Player HP: " + playerHP);

        if (playerHP <= 0) // HP�� 0�� �Ǹ� ���� ����
        {
            Debug.Log("Player is dead!");
        }
    }
}
