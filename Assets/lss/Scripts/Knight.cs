using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public Transform player;             // 플레이어 오브젝트의 Transform

    public int KnightSpeed = 2;
    public float walkSpeed = 2f;         // 걷기 속도
    public float currentSpeed = 2f;
    public float decisionTime = 2f;      // 행동 결정 주기

    private int nextMove;                // 무작위 움직임 설정

    private bool isFacingLeft = false;    // 적이 왼쪽을 바라보는지 여부


    private Rigidbody2D bossRigidBody;
    private SpriteRenderer bossSpriteRenderer;
    private Animator bossAnimator;
    // Start is called before the first frame update
    void Start()
    {
        bossRigidBody = GetComponent<Rigidbody2D>();
        bossAnimator = GetComponent<Animator>();
        bossSpriteRenderer = GetComponent<SpriteRenderer>();

        InvokeRepeating("DecideNextAction", 0f, decisionTime);
    }

    // Update is called once per frame
    void Update()
    {
        
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
