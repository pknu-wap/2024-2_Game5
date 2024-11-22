using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobAI : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public float detectionRadius = 5f; // 감지 반경
    public float moveSpeed = 2f; // 이동 속도
    public float attackRange = 1f; // 공격 범위
    public Animator animator; // 애니메이터

    public int health = 100; // 몹의 체력
    public int attackPower = 10; // 몹의 공격력
    public float attackCooldown = 1f; // 공격 쿨타임
    private float lastAttackTime;

    private bool canTakeDamage = true; // 데미지를 받을 수 있는 상태인지 확인
    public float invulnerabilityDuration = 1f; // 데미지 무시 시간
    private bool isDead = false; // 몹의 생사 상태

    private void Update()
    {
        if (isDead) return; // 몹이 죽었으면 모든 활동을 멈춤

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            // 플레이어를 바라보게 하기
            LookAtPlayer();

            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                Attack();
                animator.SetTrigger("isAttacking");
            }
        }
        else
        {
            StopMoving();
        }
    }

    private void MoveTowardsPlayer()
    {
        // 플레이어를 향해 이동
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 애니메이션 상태 변경
        animator.SetBool("isMoving", true);
    }

    private void StopMoving()
    {
        // 애니메이션 상태 변경
        animator.SetBool("isMoving", false);
    }

    private void Attack()
    {
        // 공격 쿨타임 체크
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("공격! 공격력: " + attackPower);
            player.GetComponent<Player>().TakeDamage(attackPower);
            lastAttackTime = Time.time; // 마지막 공격 시간 업데이트
        }
    }

    private void LookAtPlayer()
    {
        // 플레이어의 방향 계산
        Vector3 direction = player.position - transform.position;
        direction.Normalize(); // 방향을 정규화하여 사용

        // Y축 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f; // -180도 회전 보정
        transform.rotation = Quaternion.Euler(0f, angle, 0f); // Y축만 회전
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage || isDead) return; // 죽었거나 데미지를 받을 수 없는 상태일 경우 무시

        Debug.Log("몹이 데미지를 받음: " + damage);
        health -= damage;
        Debug.Log("남은 체력: " + health);

        if (health <= 0)
        {
            Die(); // 죽음 처리
        }
        else
        {
            StartCoroutine(ResetDamage()); // 일정 시간 후 데미지를 받을 수 있게 설정
        }
    }

    public void Die()
    {
        isDead = true; // 몹이 죽었음을 표시
        Debug.Log("몹이 죽었습니다.");
        animator.SetBool("isDead", true); // 죽었을 때 애니메이션 상태 변경
        Destroy(gameObject, 2f); // 2초 후에 삭제
    }

    private IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(invulnerabilityDuration); // 특정 시간 대기
        canTakeDamage = true; // 다시 데미지를 받을 수 있는 상태로 복구
    }
}