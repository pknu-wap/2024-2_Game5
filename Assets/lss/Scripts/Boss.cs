using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;             // 플레이어 오브젝트의 Transform
    public float walkSpeed = 2f;         // 걷기 속도
    private float currentSpeed = 0f;     // 현재 속도 (0일 때 가만히)
    private int nextMove;                // 무작위 움직임 설정
    public float decisionTime = 2f;      // 행동 결정 주기
    public float bossHP;
    public float attackRange = 5f;
    public float distance;
    public bool isAttacking = false;
    private int attackType;

    private Rigidbody2D bossRigidBody;
    private Animator bossAnimator;
    private SpriteRenderer bossSpriteRenderer;

    private bool isFacingLeft = true;    // 적이 왼쪽을 바라보는지 여부

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
        bossAnimator.SetBool("isWalk", currentSpeed == walkSpeed);  // 걷기 애니메이션 조건
     

        distance = Vector3.Distance(this.transform.position, player.position); // 플레이어와의 거리 계산

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

        if (collision == null) Debug.Log("collision is null");

        if (collision.gameObject.CompareTag("Player") && !isAttacking) // 플레이어와의 충돌, 공격 중일 때
        {
            Debug.Log("if entered");
            Debug.Log("if문 들어옴 "+isAttacking);

            Debug.Log("in if  " + isAttacking);
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && isAttacking == true)
            {
                player.TakeDamage(4f); // 플레이어에게 4 데미지 입힘
                Debug.Log("Player HP -= 4");
            }
        }
    }
    IEnumerator DelayedAttack() // 공격 코루틴(2초 딜레이 후 공격)
    {
        currentSpeed = 0f;
        isAttacking = true;  // 공격 중 상태로 변경
        Attack();        // 무작위 공격

        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
        bossAnimator.SetBool("isAttack", false);
        bossAnimator.SetBool("isKick", false);
        bossAnimator.SetBool("isAttack2", false);
        // 공격 완료 후 다시 공격 가능 상태로 변경
    }
    void Attack()
    {
        if (bossHP >= 50f)
            attackRange = Random.Range(0, 3);
        else
            attackType = Random.Range(0, 4);
        Debug.Log(attackType);
        switch (attackType) // 무작위로 공격 애니메이션 선택
        {
            case 0:
                bossAnimator.SetBool("isAttack", true);
                Debug.Log("Basic Attack");
                break;
            case 1:
                bossAnimator.SetBool("isKick", true);
                Debug.Log("Kick Attack");
                break;
            case 2:
                bossAnimator.SetBool("isAttack2", true);
                Debug.Log("Uppercut Attack");
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
