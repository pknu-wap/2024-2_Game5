using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Boss : MonoBehaviour
{
    public Transform player;             // 플레이어 오브젝트의 Transform
    public float walkSpeed = 2f;         // 걷기 속도
    private float currentSpeed = 0f;     // 현재 속도 (0일 때 가만히)
    private int nextMove;                // 무작위 움직임 설정
    public float decisionTime = 2f;      // 행동 결정 주기
    public float bossHP;
    public float attackRange = 3f;

    public bool isAttacking = false;
    private int attackType;
    private bool isDamaging = false;

    private Rigidbody2D bossRigidBody;
    private Animator bossAnimator;
    private SpriteRenderer bossSpriteRenderer;

    private bool isFacingLeft = true;    // 적이 왼쪽을 바라보는지 여부

    private Vector2 bossRay;

    void Start()
    {
        bossHP = 100f;
        Debug.Log(bossHP);
        bossRigidBody = GetComponent<Rigidbody2D>();
        bossAnimator = GetComponent<Animator>();
        bossSpriteRenderer = GetComponent<SpriteRenderer>();
        // 행동 결정 반복
        InvokeRepeating("DecideNextAction", 0f, decisionTime);
    }

    void Update()
    {
        FacePlayer();  // 플레이어를 바라보게 함

        // 애니메이션 상태 업데이트
        bossAnimator.SetBool("isGround", true);  // 항상 지면에 있다고 가정

        if (!isAttacking && Ray())
        {
            currentSpeed = 0f;
            StartCoroutine(DelayedAttack());
        }
        else
            bossAnimator.SetBool("isWalk", currentSpeed == walkSpeed);  // 걷기 애니메이션 조건
        if (bossHP == 0)
        {
            currentSpeed = 0f;
            bossAnimator.SetBool("isDeath", true);
        }
    }
    private void FixedUpdate()
    {
        Ray();
        // 이동
        bossRigidBody.velocity = new Vector2(currentSpeed * (isFacingLeft ? -1 : 1), bossRigidBody.velocity.y);
    }

    // 무작위로 행동 결정
    void DecideNextAction()
    {
        nextMove = Random.Range(0, 2);  // 0: 가만히, 1: 걷기
        currentSpeed = nextMove == 1 ? walkSpeed : 0f;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision == null) Debug.Log("collision is null");

        if (collision.gameObject.CompareTag("Player")) // 플레이어와의 충돌, 공격 중일 때
        {
            Debug.Log("if문 들어옴 isDamaging : "+ isDamaging);
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && isDamaging)
            {
                if (attackType == 3)
                {
                    player.TakeDamage(10f);
                    isDamaging = false;
                }
                else
                {
                    player.TakeDamage(4f); // 플레이어에게 4 데미지 입힘
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
    IEnumerator DelayedAttack() // 공격 코루틴(2초 딜레이 후 공격)
    {
        isAttacking = true;  // 공격 중 상태로 변경
        isDamaging = true;
        Attack();        // 무작위 공격

        yield return new WaitForSeconds(0.01f);
        float bossAnimationTime = bossAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(bossAnimationTime);

        bossAnimator.SetBool("isAttack", false);
        bossAnimator.SetBool("isAttack2", false);
        bossAnimator.SetBool("isAttack3", false);
        bossAnimator.SetBool("isSkill", false);
        // 공격 완료 후 다시 공격 가능 상태로 변경

        isAttacking = false;
    }
    void Attack()
    {
        if(bossHP > 50)
            attackType = Random.Range(0, 3);
        else
            attackType = Random.Range(0, 4);
        switch (attackType) // 무작위로 공격 애니메이션 선택
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
    void FacePlayer()// 플레이어를 바라보게 하는 함수
    {
        if (player != null)
        {
            if (transform.position.x < player.position.x && isFacingLeft)
                Flip();
            else if (transform.position.x > player.position.x && !isFacingLeft)
                Flip();
        }
    }
    void Flip() // 방향을 바꾸는 함수 (스프라이트 반전)
    {
        isFacingLeft = !isFacingLeft;
        bossSpriteRenderer.flipX = !bossSpriteRenderer.flipX;
    }
}
