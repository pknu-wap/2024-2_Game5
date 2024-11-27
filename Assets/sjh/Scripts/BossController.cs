using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject player;             // �÷��̾� ������Ʈ�� Transform
    private PlayerController playerController;

    public float walkSpeed = 2f;         // �ȱ� �ӵ�
    private float currentSpeed = 0f;     // ���� �ӵ� (0�� �� ������)
    private int nextMove;                // ������ ������ ����
    public float decisionTime = 2f;      // �ൿ ���� �ֱ�
    public float bossHP;
    public float attackRange = 7f;
    public float distance;
    public bool isAttacking = false;
    private int attackType;
    private bool isDamaging = false;

    private Rigidbody2D bossRigidBody;
    private Animator bossAnimator;
    private SpriteRenderer bossSpriteRenderer;

    private bool isFacingLeft = true;    // ���� ������ �ٶ󺸴��� ����

    void Start()
    {
        bossHP = 100f;
        Debug.Log(bossHP);
        player = GameObject.FindWithTag("Player");
        bossRigidBody = GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
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
        bossAnimator.SetBool("isWalk", currentSpeed == walkSpeed);  // �ȱ� �ִϸ��̼� ����
     

        distance = Vector3.Distance(this.transform.position, player.transform.position); // �÷��̾���� �Ÿ� ���

        if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(DelayedAttack());
            Debug.Log("Update " + isAttacking);

        }
        if(bossHP == 0)
        {
            currentSpeed = 0f;
            bossAnimator.SetBool("isDeath", true);
        }
    }
    private void FixedUpdate()
    {
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

        if (collision == null) Debug.Log("collision is null");

        if (collision.gameObject.CompareTag("Player") && !isDamaging) // �÷��̾���� �浹, ���� ���� ��
        {
            Debug.Log("if entered");
            Debug.Log("if�� ���� "+ isDamaging);

            Debug.Log("in if  " + isDamaging);
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && isDamaging == true)
            {
                playerController.TakeDamage(4f, this.transform.position); // �÷��̾�� 4 ������ ����
                Debug.Log("Player HP -= 4");
            }
        }
        isDamaging = false;
    }
 
    public void TakeDamage(float damage, Vector2 monsterPosition)
    {
        //
    }
    IEnumerator DelayedAttack() // ���� �ڷ�ƾ(2�� ������ �� ����)
    {
        currentSpeed = 0f;
        isAttacking = true;  // ���� �� ���·� ����
        isDamaging = true;
        Attack();        // ������ ����

        yield return new WaitForSeconds(2.4f);
        isAttacking = false;
        bossAnimator.SetBool("isAttack", false);
        bossAnimator.SetBool("isAttack2", false);
        bossAnimator.SetBool("isAttack3", false);
        // ���� �Ϸ� �� �ٽ� ���� ���� ���·� ����
    }
    void Attack()
    {
        Debug.Log(bossHP);

        attackType = Random.Range(0, 4);
        Debug.Log(attackType);
        switch (attackType) // �������� ���� �ִϸ��̼� ����
        {
            case 0:
                bossAnimator.SetBool("isAttack", true);
                Debug.Log("Basic Attack");
                break;
            case 1:
                bossAnimator.SetBool("isAttack2", true);
                Debug.Log("Kick Attack");
                break;
            case 2:
                bossAnimator.SetBool("isAttack3", true);
                Debug.Log("Uppercut Attack");
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
            if (transform.position.x < player.transform.position.x && isFacingLeft)
                Flip();
            else if (transform.position.x > player.transform.position.x && !isFacingLeft)
                Flip();
        }
    }
    void Flip() // ������ �ٲٴ� �Լ� (��������Ʈ ����)
    {
        isFacingLeft = !isFacingLeft;
        bossSpriteRenderer.flipX = !bossSpriteRenderer.flipX;
    }
}
