using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Boss : MonoBehaviour
{
    public Transform player;
    public PlayerController player2;
    public float walkSpeed = 2f;
    private float currentSpeed = 0f;
    private int nextMove;
    public float decisionTime = 2f;
    public float bossHP;
    public float attackRange = 3f;

    public bool isAttacking = false;
    private int attackType;
    private bool isDamaging = false;
    
    private Rigidbody2D bossRigidBody;
    private Animator bossAnimator;
    private SpriteRenderer bossSpriteRenderer;

    private bool isFacingLeft = true;

    private Vector2 bossRay;

    void Start()
    {
        bossHP = 100f;
        Debug.Log(bossHP);
        bossRigidBody = GetComponent<Rigidbody2D>();
        bossAnimator = GetComponent<Animator>();
        bossSpriteRenderer = GetComponent<SpriteRenderer>();
        
        InvokeRepeating("DecideNextAction", 0f, decisionTime);
    }

    void Update()
    {
        FacePlayer();

        bossAnimator.SetBool("isGround", true);

        if (bossHP == 0)
        {
            currentSpeed = 0f;
            bossAnimator.SetBool("isDeath", true);
        }
    }
    private void FixedUpdate()
    {
        if (!isAttacking && Ray())
        {
            currentSpeed = 0f;
            StartCoroutine(DelayedAttack());
        }
        else
            bossAnimator.SetBool("isWalk", currentSpeed == walkSpeed);

        bossRigidBody.velocity = new Vector2(currentSpeed * (isFacingLeft ? -1 : 1), bossRigidBody.velocity.y);
    }

    void DecideNextAction()
    {
        nextMove = Random.Range(0, 2);
        currentSpeed = nextMove == 1 ? walkSpeed : 0f;
    }
    bool Ray()
    {
        if (isFacingLeft)
            bossRay = Vector2.left;
        else
            bossRay = Vector2.right;

        Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), bossRay * attackRange, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.1f, 0), bossRay, attackRange, LayerMask.GetMask("Player"));
        if(hit.collider != null)
        {
            player2 = hit.transform.GetComponent<PlayerController>();
            return true;
        }
        else
            return false;
    }
    public void TakeDamage(float damage, Vector2 monsterPosition)
    {
        //
    }
    IEnumerator DelayedAttack()
    {
        isAttacking = true;
        isDamaging = true;
        Attack();

        yield return new WaitForSeconds(0.01f);
        float bossAnimationTime = bossAnimator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(bossAnimationTime / 2);

        if (Ray() && player2 != null)
        {
            if(attackType == 3)
            {
                player2.TakeDamage(10, player2.transform.position);
                Debug.Log("Damaged 10");
                isDamaging = false;
            }
            else
            {
                player2.TakeDamage(4, player2.transform.position);
                Debug.Log("Damaged 4");
                isDamaging = false;
            }
        }

        yield return new WaitForSeconds(bossAnimationTime / 2);

        bossAnimator.SetBool("isAttack", false);
        bossAnimator.SetBool("isAttack2", false);
        bossAnimator.SetBool("isAttack3", false);
        bossAnimator.SetBool("isSkill", false);

        isAttacking = false;
    }
    void Attack()
    {
        if(bossHP > 50)
            attackType = Random.Range(0, 3);
        else
            attackType = Random.Range(0, 4);
        switch (attackType)
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
    void Flip()
    {
        isFacingLeft = !isFacingLeft;
        bossSpriteRenderer.flipX = !bossSpriteRenderer.flipX;
    }
}
