using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerHP = 100f;  // 플레이어의 HP

    // 플레이어의 HP를 깎는 함수
    public void TakeDamage(float damage)
    {
        playerHP -= damage;
        Debug.Log("Player HP: " + playerHP);

        if (playerHP <= 0) // HP가 0이 되면 게임 오버
        {
            Debug.Log("Player is dead!");
        }
    }
}
