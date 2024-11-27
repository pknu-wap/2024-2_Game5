using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;   // 이동 속도
    public int health = 100;   // 체력
    public int attackPower = 20; // 공격력
    public float attackRange = 1.5f; // 공격 범위

    void Update()
    {
        Move();
        Attack();
    }

    // 플레이어 이동
    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // 좌우 방향키 입력 받기
        float moveVertical = Input.GetAxis("Vertical"); // 상하 방향키 입력 받기

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);
    }

    // 공격 메서드
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바 입력 받기
        {
            Debug.Log("공격! 공격력: " + attackPower);

            // 공격 로직: 근처 몹에게 피해 주기
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

            // 주변 몹에게 공격
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy")) // 몹의 태그 확인
                {
                    // 공격을 가하여 몹의 체력을 감소
                    enemy.GetComponent<MobAI>().TakeDamage(attackPower);
                    Debug.Log("몹에게 피해를 주었습니다! 남은 체력: " + enemy.GetComponent<MobAI>().health);

                    // 몹이 체력이 0 이하가 되면 죽음 처리
                    if (enemy.GetComponent<MobAI>().health <= 0)
                    {
                        enemy.GetComponent<MobAI>().Die(); // 몹의 Die 메서드 호출
                    }
                }
            }
        }
    }

    // 체력 감소 메서드
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("플레이어가 공격을 받았습니다! 남은 체력: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("플레이어가 죽었습니다.");
        // 사망 처리 로직 추가 (예: 게임 오버 화면, 애니메이션 등)
        gameObject.SetActive(false); // 플레이어 오브젝트 비활성화 (예시)
    }

    private void OnDrawGizmosSelected()
    {
        // 공격 범위를 시각적으로 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}