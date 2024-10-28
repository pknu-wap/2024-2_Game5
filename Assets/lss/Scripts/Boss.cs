using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;             // 플레이어 오브젝트의 Transform
    public float walkSpeed = 2f;         // 걷기 속도
    public float runSpeed = 4f;          // 달리기 속도
    private float currentSpeed = 0f;     // 현재 속도 (0일 때 가만히)
    private int nextMove;                // 무작위 움직임 설정
    public float decisionTime = 2f;      // 행동 결정 주기
    public float bossHP;
    public float attackRange = 7f;
    public float distance;

    private Rigidbody2D barbarianRigidBody;
    private Animator barbarianAnimator;
    private SpriteRenderer barbarianSpriteRenderer;

    private bool isFacingLeft = true;    // 적이 왼쪽을 바라보는지 여부

    void Start()
    {
        bossHP = 100;

        barbarianRigidBody = GetComponent<Rigidbody2D>();
        barbarianAnimator = GetComponent<Animator>();
        barbarianSpriteRenderer = GetComponent<SpriteRenderer>();

        // 행동 결정 반복
        InvokeRepeating("DecideNextAction", 0f, decisionTime);
    }

    void Update()
    {
        FacePlayer();  // 플레이어를 바라보게 함

        // 애니메이션 상태 업데이트
        barbarianAnimator.SetBool("isGround", true);  // 항상 지면에 있다고 가정
        barbarianAnimator.SetBool("isWalk", currentSpeed == walkSpeed);  // 걷기 애니메이션 조건
     

        distance = Vector3.Distance(this.transform.position, player.position); // 플레이어와의 거리 계산
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
            barbarianAnimator.SetBool("isAttack2", false); // 공격 범위 밖일 경우 공격 중지
        }
    }

    private void FixedUpdate()
    {
        // 이동
        barbarianRigidBody.velocity = new Vector2(currentSpeed * (isFacingLeft ? -1 : 1), barbarianRigidBody.velocity.y);
    }

    // 무작위로 행동 결정
    void DecideNextAction()
    {
        nextMove = Random.Range(0, 2);  // 0: 가만히, 1: 걷기

        switch (nextMove)
        {
            case 0:
                currentSpeed = 0f;  // 가만히 있음
                break;
            case 1:
                currentSpeed = walkSpeed;  // 걷기
                break;
        }
    }

        // 플레이어를 바라보게 하는 함수
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

        // 방향을 바꾸는 함수 (스프라이트 반전)
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
