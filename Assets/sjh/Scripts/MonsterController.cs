using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public bool isAttacking = false;
    public int monsterHP = 200 ;
    public int monsterDamage = 10;  // 몬스터가 플레이어에게 주는 데미지

    public GameObject player;
    private PlayerController playerController;
    public bool isInvincible = false;
    public bool isGuarding = false;
    public bool ishitted = false;
    public float knockBackForce = 3f;

    private BattleManager battleManager;

    public void TakeDamage(int damage, Vector2 playerPosition)
    {
 
        ishitted = true;
        monsterHP -= damage;
        ishitted = false;

   
    }
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isAttacking)
        {
            Debug.Log("플레이어와 공격 중 충돌");
            Vector2 hitPosition = transform.position;

            if (playerController != null)
            {
                playerController.TakeDamage(monsterDamage, hitPosition);
            }
        }
    }
}
