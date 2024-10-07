using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_move : MonoBehaviour
{
    private Rigidbody2D bossRigidBody;
    private Animator bossAnimator;

    public float moveSpeed = 1f;          // Boss�� �̵� �ӵ�
    public float jumpPower = 1f;
    public float distance = 3f;  // Boss�� player ������ �Ÿ�
    private float BossHP;
    public int nextMove;

    public SpriteRenderer bossSpriteRenderer;
    public Transform player;

    private bool isFacingLeft = true;    // ���� ������ �ٶ󺸴��� Ȯ��

    void Start()
    {
        bossRigidBody = GetComponent<Rigidbody2D>();
        bossAnimator = GetComponent<Animator>();
        bossSpriteRenderer = GetComponent<SpriteRenderer>();

        InvokeRepeating("RandomMove", 0f, 2f);
    }
    void Update()
    {
        bossAnimator.SetBool("isGround", true);
        bossAnimator.SetBool("isWalk", Mathf.Abs(bossRigidBody.velocity.x) > 0);
        
        float distance = Vector3.Distance(this.transform.position, player.position);
        Debug.Log(distance);
    }
    
    private void FixedUpdate()
    {
        bossRigidBody.velocity = new Vector2(nextMove, bossRigidBody.velocity.y);

        FacePlayer();
    }
    void RandomMove()
    {   // �������� �����̱�, ���߱�, ���������� �����̱� ���� ����
        nextMove = Random.Range(-1, 2);
    }
    void FacePlayer()
    {
        if(player != null)
        {
            if (this.transform.position.x < player.position.x && !isFacingLeft)
                Flip();
            else if(this.transform.position.x > player.position.x && isFacingLeft)
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
