using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;   // �̵� �ӵ�
    public int health = 100;   // ü��
    public int attackPower = 20; // ���ݷ�
    public float attackRange = 1.5f; // ���� ����

    void Update()
    {
        Move();
        Attack();
    }

    // �÷��̾� �̵�
    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // �¿� ����Ű �Է� �ޱ�
        float moveVertical = Input.GetAxis("Vertical"); // ���� ����Ű �Է� �ޱ�

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);
    }

    // ���� �޼���
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // �����̽��� �Է� �ޱ�
        {
            Debug.Log("����! ���ݷ�: " + attackPower);

            // ���� ����: ��ó ������ ���� �ֱ�
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

            // �ֺ� ������ ����
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy")) // ���� �±� Ȯ��
                {
                    // ������ ���Ͽ� ���� ü���� ����
                    enemy.GetComponent<MobAI>().TakeDamage(attackPower);
                    Debug.Log("������ ���ظ� �־����ϴ�! ���� ü��: " + enemy.GetComponent<MobAI>().health);

                    // ���� ü���� 0 ���ϰ� �Ǹ� ���� ó��
                    if (enemy.GetComponent<MobAI>().health <= 0)
                    {
                        enemy.GetComponent<MobAI>().Die(); // ���� Die �޼��� ȣ��
                    }
                }
            }
        }
    }

    // ü�� ���� �޼���
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("�÷��̾ ������ �޾ҽ��ϴ�! ���� ü��: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("�÷��̾ �׾����ϴ�.");
        // ��� ó�� ���� �߰� (��: ���� ���� ȭ��, �ִϸ��̼� ��)
        gameObject.SetActive(false); // �÷��̾� ������Ʈ ��Ȱ��ȭ (����)
    }

    private void OnDrawGizmosSelected()
    {
        // ���� ������ �ð������� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}