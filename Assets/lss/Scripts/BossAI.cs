using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_move : MonoBehaviour
{
    private Rigidbody2D bossRigidBody;
    private Animator bossAnimator;

    public float moveSpeed = 2f; // Boss�� �̵� �ӵ�
    public float distance = 7f;  // Boss�� player ������ �Ÿ�
    private float BossHP;
    public int nextMove;

    public SpriteRenderer bossSpriteRenderer;
    public Transform player;

    private bool isFacingLeft = true;    // ���� ������ �ٶ󺸴��� Ȯ��


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
    {   // �������� �����̱�, ���߱�, ���������� �����̱� ���� ����
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

        // ��������Ʈ�� �������� ������ �÷��̾ �ٶ󺸰� ��
        bossSpriteRenderer.flipX = !bossSpriteRenderer.flipX;
    }
}
