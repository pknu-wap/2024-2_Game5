using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobAI : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public float detectionRadius = 5f; // ���� �ݰ�
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float attackRange = 1f; // ���� ����
    public Animator animator; // �ִϸ�����

    public int health = 100; // ���� ü��
    public int attackPower = 10; // ���� ���ݷ�
    public float attackCooldown = 1f; // ���� ��Ÿ��
    private float lastAttackTime;

    private bool canTakeDamage = true; // �������� ���� �� �ִ� �������� Ȯ��
    public float invulnerabilityDuration = 1f; // ������ ���� �ð�
    private bool isDead = false; // ���� ���� ����

    private void Update()
    {
        if (isDead) return; // ���� �׾����� ��� Ȱ���� ����

        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            // �÷��̾ �ٶ󺸰� �ϱ�
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
        // �÷��̾ ���� �̵�
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // �ִϸ��̼� ���� ����
        animator.SetBool("isMoving", true);
    }

    private void StopMoving()
    {
        // �ִϸ��̼� ���� ����
        animator.SetBool("isMoving", false);
    }

    private void Attack()
    {
        // ���� ��Ÿ�� üũ
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("����! ���ݷ�: " + attackPower);
            player.GetComponent<PlayerController>().TakeDamage(attackPower,player.transform.position);
            lastAttackTime = Time.time; // ������ ���� �ð� ������Ʈ
        }
    }

    private void LookAtPlayer()
    {
        // �÷��̾��� ���� ���
        Vector3 direction = player.position - transform.position;
        direction.Normalize(); // ������ ����ȭ�Ͽ� ���

        // Y�� ȸ��
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f; // -180�� ȸ�� ����
        transform.rotation = Quaternion.Euler(0f, angle, 0f); // Y�ุ ȸ��
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage || isDead) return; // �׾��ų� �������� ���� �� ���� ������ ��� ����

        Debug.Log("���� �������� ����: " + damage);
        health -= damage;
        Debug.Log("���� ü��: " + health);

        if (health <= 0)
        {
            Die(); // ���� ó��
        }
        else
        {
            StartCoroutine(ResetDamage()); // ���� �ð� �� �������� ���� �� �ְ� ����
        }
    }

    public void Die()
    {
        isDead = true; // ���� �׾����� ǥ��
        Debug.Log("���� �׾����ϴ�.");
        animator.SetBool("isDead", true); // �׾��� �� �ִϸ��̼� ���� ����
        Destroy(gameObject, 2f); // 2�� �Ŀ� ����
    }

    private IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(invulnerabilityDuration); // Ư�� �ð� ���
        canTakeDamage = true; // �ٽ� �������� ���� �� �ִ� ���·� ����
    }
}