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
    public float attackRange = 7f;
    public float distance;
    private bool isAttacking = false;

    private Rigidbody2D barbarianRigidBody;
    private Animator barbarianAnimator;
    private SpriteRenderer barbarianSpriteRenderer;

    private bool isFacingLeft = true;    // 적이 왼쪽을 바라보는지 여부

    public class Player : MonoBehaviour
    {
        public float playerHP = 100f;  // 플레이어의 HP

        // 플레이어의 HP를 깎는 함수
        public void TakeDamage(float damage)
        {
            playerHP -= damage;
            Debug.Log("Player HP: " + playerHP);

            if (playerHP <= 0) // HP가 0이 되면 game over
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
        // 이동
        barbarianRigidBody.velocity = new Vector2(currentSpeed * (isFacingLeft ? -1 : 1), barbarianRigidBody.velocity.y);
    }

    // 무작위로 행동 결정
    void DecideNextAction()
    {
        nextMove = Random.Range(0, 2);  // 0: 가만히, 1: 걷기
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
        barbarianSpriteRenderer.flipX = !barbarianSpriteRenderer.flipX;
    }
    IEnumerator DelayedAttack() // 공격 코루틴(2초 딜레이 후 공격)
    {
        isAttacking = true;  // 공격 중 상태로 변경
        Attack();        // 무작위 공격

        yield return new WaitForSeconds(2f);  // 1초 대기
        isAttacking = false;  // 공격 완료 후 다시 공격 가능 상태로 변경
    }
    void Attack()
    {
        int attackType = Random.Range(0, 3);  // 0: 기본 공격, 1: 킥, 2: 어퍼컷

        switch (attackType) // 무작위로 공격 애니메이션 선택
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
