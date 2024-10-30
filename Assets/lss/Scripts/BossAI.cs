using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_move : MonoBehaviour
{
    private Rigidbody2D bossRigidBody;
    private Animator bossAnimator;

    public float moveSpeed = 2f; // Boss의 이동 속도
    public float distance = 7f;  // Boss와 player 사이의 거리
    private float BossHP;
    public int nextMove;

    public SpriteRenderer bossSpriteRenderer;
    public Transform player;

    private bool isFacingLeft = true;    // 적이 왼쪽을 바라보는지 확인


    void Start()
    {
        BossHP = 100f;
        bossRigidBody = GetComponent<Rigidbody2D>();
        bossAnimator = GetComponent<Animator>();
        bossSpriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("RandomMove", 2f);

    }
    void Update()
    {
        bossAnimator.SetBool("isGround", true);
        bossAnimator.SetBool("isWalk", Mathf.Abs(bossRigidBody.velocity.x) > 0);
        
        float distance = Vector3.Distance(this.transform.position, player.position);
    }
    
    private void FixedUpdate()
    {
        bossRigidBody.velocity = new Vector2(nextMove, bossRigidBody.velocity.y);

        FacePlayer();
    }
    void RandomMove()
    {   // 왼쪽으로 움직이기, 멈추기, 오른쪽으로 움직이기 랜덤 지정
        nextMove = Random.Range(-1, 1);
    }
    void FacePlayer()
    {
        if(player != null)
        {
            if (this.transform.position.x < player.position.x && isFacingLeft)
                Flip();
            else if(this.transform.position.x > player.position.x && !isFacingLeft)
                Flip();
        }
    }
    void Flip()
    {
        isFacingLeft = !isFacingLeft;

        // 스프라이트를 반전시켜 보스가 플레이어를 바라보게 함
        bossSpriteRenderer.flipX = !bossSpriteRenderer.flipX;
    }
}
