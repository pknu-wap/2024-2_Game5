using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public Transform player;             // �÷��̾� ������Ʈ�� Transform

    public int KnightSpeed = 2;
    public float walkSpeed = 2f;         // �ȱ� �ӵ�
    public float currentSpeed = 2f;
    public float decisionTime = 2f;      // �ൿ ���� �ֱ�

    private int nextMove;                // ������ ������ ����

    private bool isFacingLeft = false;    // ���� ������ �ٶ󺸴��� ����


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
        // �̵�
        bossRigidBody.velocity = new Vector2(currentSpeed * (isFacingLeft ? -1 : 1), bossRigidBody.velocity.y);
    }

    // �������� �ൿ ����
    void DecideNextAction()
    {
        nextMove = Random.Range(0, 2);  // 0: ������, 1: �ȱ�
        currentSpeed = nextMove == 1 ? walkSpeed : 0f;
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
