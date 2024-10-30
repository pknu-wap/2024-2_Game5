using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;             // �÷��̾� ������Ʈ�� Transform
    public float walkSpeed = 2f;         // �ȱ� �ӵ�
    private float currentSpeed = 0f;     // ���� �ӵ� (0�� �� ������)
    private int nextMove;                // ������ ������ ����
    public float decisionTime = 2f;      // �ൿ ���� �ֱ�
    public float bossHP;
    public float attackRange = 7f;
    public float distance;
    private bool isAttacking = false;

    private Rigidbody2D barbarianRigidBody;
    private Animator barbarianAnimator;
    private SpriteRenderer barbarianSpriteRenderer;

    private bool isFacingLeft = true;    // ���� ������ �ٶ󺸴��� ����

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
    void Start()
    {
        bossHP = 100f;

        barbarianRigidBody = GetComponent<Rigidbody2D>();
        barbarianAnimator = GetComponent<Animator>();
        barbarianSpriteRenderer = GetComponent<SpriteRenderer>();

        // �ൿ ���� �ݺ�
        InvokeRepeating("DecideNextAction", 0f, decisionTime);
    }

    void Update()
    {
        FacePlayer();  // �÷��̾ �ٶ󺸰� ��

        // �ִϸ��̼� ���� ������Ʈ
        barbarianAnimator.SetBool("isGround", true);  // �׻� ���鿡 �ִٰ� ����
        barbarianAnimator.SetBool("isWalk", currentSpeed == walkSpeed);  // �ȱ� �ִϸ��̼� ����
     

        distance = Vector3.Distance(this.transform.position, player.position); // �÷��̾���� �Ÿ� ���

        if (distance <= attackRange && !isAttacking)
        {
            Debug.Log("Attack triggered!");
            StartCoroutine(DelayedAttack());
        }
        else if(isAttacking)
        {
            ResetAttackAnimations();
        }
    }

    private void FixedUpdate()
    {
        // �̵�
        barbarianRigidBody.velocity = new Vector2(currentSpeed * (isFacingLeft ? -1 : 1), barbarianRigidBody.velocity.y);
    }

    // �������� �ൿ ����
    void DecideNextAction()
    {
        nextMove = Random.Range(0, 2);  // 0: ������, 1: �ȱ�
        currentSpeed = nextMove == 1 ? walkSpeed : 0f;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAttacking)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(4f);
                Debug.Log("Player HP -= 4");
            }
        }
    }
    void FacePlayer()// �÷��̾ �ٶ󺸰� �ϴ� �Լ�
    {
        if (player != null)
        {
            if (transform.position.x < player.position.x && isFacingLeft)
                Flip();
            else if (transform.position.x > player.position.x && !isFacingLeft)
                Flip();
        }
    }

    void Flip() // ������ �ٲٴ� �Լ� (��������Ʈ ����)
    {
        isFacingLeft = !isFacingLeft;
        barbarianSpriteRenderer.flipX = !barbarianSpriteRenderer.flipX;
    }
    IEnumerator DelayedAttack() // ���� �ڷ�ƾ(2�� ������ �� ����)
    {
        isAttacking = true;  // ���� �� ���·� ����
        Attack();        // ������ ����

        yield return new WaitForSeconds(2f);  // 1�� ���
        isAttacking = false;  // ���� �Ϸ� �� �ٽ� ���� ���� ���·� ����
    }
    void Attack()
    {
        int attackType = Random.Range(0, 3);  // 0: �⺻ ����, 1: ű, 2: ������

        switch (attackType) // �������� ���� �ִϸ��̼� ����
        {
            case 0:
                barbarianAnimator.SetBool("isAttack2", true);
                Debug.Log("Basic Attack");
                break;
            case 1:
                barbarianAnimator.SetBool("isKick", true);
                Debug.Log("Kick Attack");
                break;
            case 2:
                barbarianAnimator.SetBool("isUppercut", true);
                Debug.Log("Uppercut Attack");
                break;
        }
    }
    void ResetAttackAnimations()
    {
        barbarianAnimator.SetBool("isAttack2", false);
        barbarianAnimator.SetBool("isKick", false);
        barbarianAnimator.SetBool("isUppercut", false);
    }
}
