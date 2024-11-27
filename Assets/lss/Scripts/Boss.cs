using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Boss : MonoBehaviour
{
    public Transform player;             // �÷��̾� ������Ʈ�� Transform
    public float walkSpeed = 2f;         // �ȱ� �ӵ�
    private float currentSpeed = 0f;     // ���� �ӵ� (0�� �� ������)
    private int nextMove;                // ������ ������ ����
    public float decisionTime = 2f;      // �ൿ ���� �ֱ�
    public float bossHP;
    public float attackRange = 3f;

    public bool isAttacking = false;
    private int attackType;
    private bool isDamaging = false;

    private Rigidbody2D bossRigidBody;
    private Animator bossAnimator;
    private SpriteRenderer bossSpriteRenderer;

    private bool isFacingLeft = true;    // ���� ������ �ٶ󺸴��� ����

    private Vector2 bossRay;

    void Start()
    {
        bossHP = 100f;
        Debug.Log(bossHP);
        bossRigidBody = GetComponent<Rigidbody2D>();
        bossAnimator = GetComponent<Animator>();
        bossSpriteRenderer = GetComponent<SpriteRenderer>();
        // �ൿ ���� �ݺ�
        InvokeRepeating("DecideNextAction", 0f, decisionTime);
    }

    void Update()
    {
        FacePlayer();  // �÷��̾ �ٶ󺸰� ��

        // �ִϸ��̼� ���� ������Ʈ
        bossAnimator.SetBool("isGround", true);  // �׻� ���鿡 �ִٰ� ����

        if (!isAttacking && Ray())
        {
            currentSpeed = 0f;
            StartCoroutine(DelayedAttack());
        }
        else
            bossAnimator.SetBool("isWalk", currentSpeed == walkSpeed);  // �ȱ� �ִϸ��̼� ����
        if (bossHP == 0)
        {
            currentSpeed = 0f;
            bossAnimator.SetBool("isDeath", true);
        }
    }
    private void FixedUpdate()
    {
        Ray();
        // �̵�
        bossRigidBody.velocity = new Vector2(currentSpeed * (isFacingLeft ? -1 : 1), bossRigidBody.velocity.y);
    }

    // �������� �ൿ ����
    void DecideNextAction()
    {
        nextMove = Random.Range(0, 2);  // 0: ������, 1: �ȱ�
        currentSpeed = nextMove == 1 ? walkSpeed : 0f;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision == null) Debug.Log("collision is null");

        if (collision.gameObject.CompareTag("Player")) // �÷��̾���� �浹, ���� ���� ��
        {
            Debug.Log("if�� ���� isDamaging : "+ isDamaging);
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && isDamaging)
            {
                if (attackType == 3)
                {
                    player.TakeDamage(10);
                    isDamaging = false;
                }
                else
                {
                    player.TakeDamage(4); // �÷��̾�� 4 ������ ����
                    Debug.Log("Player HP -= 4");
                    isDamaging = false;
                }
            }
        }
    }
    bool Ray()
    {
        if (isFacingLeft)
            bossRay = Vector2.left;
        else
            bossRay = Vector2.right;

        Debug.DrawRay(transform.position, bossRay * attackRange, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, bossRay, attackRange, LayerMask.GetMask("Player"));
        bool check = hit.collider != null;
        return check;
    }
    IEnumerator DelayedAttack() // ���� �ڷ�ƾ(2�� ������ �� ����)
    {
        isAttacking = true;  // ���� �� ���·� ����
        isDamaging = true;
        Attack();        // ������ ����

        yield return new WaitForSeconds(0.01f);
        float bossAnimationTime = bossAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(bossAnimationTime);

        bossAnimator.SetBool("isAttack", false);
        bossAnimator.SetBool("isAttack2", false);
        bossAnimator.SetBool("isAttack3", false);
        bossAnimator.SetBool("isSkill", false);
        // ���� �Ϸ� �� �ٽ� ���� ���� ���·� ����

        isAttacking = false;
    }
    void Attack()
    {
        if(bossHP > 50)
            attackType = Random.Range(0, 3);
        else
            attackType = Random.Range(0, 4);
        switch (attackType) // �������� ���� �ִϸ��̼� ����
        {
            case 0:
                bossAnimator.SetBool("isAttack", true);
                break;
            case 1:
                bossAnimator.SetBool("isAttack2", true);
                break;
            case 2:
                bossAnimator.SetBool("isAttack3", true);
                break;
            case 3:
                bossAnimator.SetBool("isSkill", true);
                break;
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
        bossSpriteRenderer.flipX = !bossSpriteRenderer.flipX;
    }
}
