using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;             // �÷��̾� ������Ʈ�� Transform
    public float walkSpeed = 2f;         // �ȱ� �ӵ�
    public float runSpeed = 4f;          // �޸��� �ӵ�
    private float currentSpeed = 0f;     // ���� �ӵ� (0�� �� ������)
    private int nextMove;                // ������ ������ ����
    public float decisionTime = 2f;      // �ൿ ���� �ֱ�
    public float bossHP;
    public float attackRange = 7f;
    public float distance;

    private Rigidbody2D barbarianRigidBody;
    private Animator barbarianAnimator;
    private SpriteRenderer barbarianSpriteRenderer;

    private bool isFacingLeft = true;    // ���� ������ �ٶ󺸴��� ����

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        bossHP = 100;


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
        Debug.Log("Distance to player: " + distance);
        Debug.Log("Attack Range: " + attackRange);

        if (distance <= attackRange)
        {
            Debug.Log("Attack triggered!");
            Attack();
        }
        else
        {
            Debug.Log("Outside attack range");
            barbarianAnimator.SetBool("isAttack2", false); // ���� ���� ���� ��� ���� ����
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

        switch (nextMove)
        {
            case 0:
                currentSpeed = 0f;  // ������ ����
                break;
            case 1:
                currentSpeed = walkSpeed;  // �ȱ�
                break;
        }
    }

        // �÷��̾ �ٶ󺸰� �ϴ� �Լ�
    void FacePlayer()
    {
        if (player != null)
        {
            if (transform.position.x < player.position.x && isFacingLeft)
                Flip();
            else if (transform.position.x > player.position.x && !isFacingLeft)
                Flip();
        }
    }

        // ������ �ٲٴ� �Լ� (��������Ʈ ����)
    void Flip()
    {
        isFacingLeft = !isFacingLeft;
        barbarianSpriteRenderer.flipX = !barbarianSpriteRenderer.flipX;
    }
    
    void Attack()
    {
        barbarianAnimator.SetBool("isAttack2", true);


    }
}
