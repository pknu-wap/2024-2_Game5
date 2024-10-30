using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public bool isAttacking = false;
    public int monsterHP;
    public int monsterDamage = 10;  // 몬스터가 플레이어에게 주는 데미지

    private BattleManager battleManager;

    void Start()
    {
        // BattleManager를 씬에서 찾기
        battleManager = FindObjectOfType<BattleManager>();

        if (battleManager == null)
        {
            Debug.LogError("BattleManager를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        monsterHP = battleManager.MonsterHP;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isAttacking)
        {
            Debug.Log("플레이어와 공격 중 충돌");
            Vector2 hitPosition = transform.position;

            if (battleManager != null)
            {
                battleManager.HandleCombatCollision(gameObject, collision.gameObject, monsterDamage, hitPosition);
            }
        }
    }
}
