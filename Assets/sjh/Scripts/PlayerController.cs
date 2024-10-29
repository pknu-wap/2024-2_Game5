using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D PlayerRigidBody;
    private Vector3 movement;
    
    public float movePower = 8f;
    public float jumpPower = 8f;
    public int playerHP;
    public int playerDamage;
    public Animator playerAnimator;

    public bool isWalking = false;
    public bool isJumping = false;
    public bool isGround = false;
    public bool isAttacking = false;
    public bool isGuarding = false;
    public bool isUsingSkill = false;
    public bool ishitted = false;
    public bool isInvincible = false;
    public bool isKnockedDown = false;
    public float knockDownDuration = 3f;
    public float knockBackForce= 20f;
    public int knockDownThreshold = 30;


    public bool isStage1 = true;
    public bool isStage2 = false;
    public bool isStage3 = false;

    int[] commandArray = new int[4];
    private Coroutine resetCoroutine;

    public (KeyCode key, int value) W = (KeyCode.W, 1);
    public (KeyCode key, int value) A = (KeyCode.A, 2);
    public (KeyCode key, int value) S = (KeyCode.S, 3);
    public (KeyCode key, int value) D = (KeyCode.D, 4);

    private BattleManager battleManager;

    public float attackRange = 2f;

    void Awake()
    {
        PlayerRigidBody = this.GetComponent<Rigidbody2D>();
        playerAnimator = this.GetComponent<Animator>();
        battleManager = FindObjectOfType<BattleManager>();
    }

    void Start()
    {
        playerHP = 100;
        InitCommandArray();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            isJumping = false;
        }

        // 플레이어의 공격이 몬스터에게 히트
        if (collision.gameObject.CompareTag("Monster") && isAttacking)
        {
            Vector2 hitPosition = transform.position;
            battleManager.HandleCombatCollision(gameObject, collision.gameObject, playerDamage, hitPosition);
        }
    }

    

    public void Move()
    {   
        // 공격 중에는 이동 불가
        if (isAttacking) return;

        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isWalking = true;
            moveVelocity = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            isWalking = true;
            moveVelocity = Vector3.right;
        }
        else
        {
            isWalking = false;
        }
        
        playerAnimator.SetBool("isWalking", isWalking);
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    public void Jump() //점프 키 여러번 눌러야만 작동, 아마 update? 
    {
        if (isJumping || isAttacking || isUsingSkill) return;

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJumping = true;
            PlayerRigidBody.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            PlayerRigidBody.AddForce(jumpVelocity, ForceMode2D.Impulse);
            isGround = false;
        }

    }

   public void NormalAttack((KeyCode key, int value) keyValuePair)
    {
        // 이미 공격 중이거나 스킬 사용 중이면 새로운 공격 불가
        if (isAttacking || isUsingSkill) return;

        // 공격 시작
        isAttacking = true;

        // 트리거 설정
        switch(keyValuePair.value)
        {
            case 1:
                playerAnimator.SetTrigger("W");
                Debug.Log("W공격");
                break;
            case 2:
                playerAnimator.SetTrigger("A");
                Debug.Log("A공격");
                break;
            case 3:
                playerAnimator.SetTrigger("S");
                Debug.Log("S공격");
                break;
            case 4:
                playerAnimator.SetTrigger("D");
                Debug.Log("D공격");
                break;
        }

        // 데미지 계산
        playerDamage = !isGround ? 8 : 5;
        Debug.Log("공격 데미지: " + playerDamage);
    }

    public void Guard()
    {
        if (isJumping || isAttacking || isUsingSkill || isWalking) return;


        if (Input.GetKey(KeyCode.E))
        {
            isGuarding = true; 
            playerAnimator.SetBool("isGuarding", true); 
        }
        else
        {
            isGuarding = false; 
            playerAnimator.SetBool("isGuarding", false); 
        }
    }

    public void TakeDamage(int damage, Vector2 monsterPosition)
    {
        if (isInvincible) return;
        ApplyKnockBackDown(monsterPosition, knockBackForce);

        if(isGuarding)
        {
            ApplyKnockBackDown(monsterPosition, knockBackForce);
        }
        
        ishitted = true;

        if(damage >= knockDownThreshold && !isKnockedDown)
        {
            StartCoroutine(KnockDownSequence(monsterPosition));
        }
        else
        {
            playerAnimator.SetTrigger("Hit");
            ApplyKnockBackDown(monsterPosition, knockBackForce);
        }
    }

    void AttackMonstersInRange()
    {
        // 플레이어의 공격 범위 내 몬스터 감지
        Collider2D[] monsters = Physics2D.OverlapCircleAll(transform.position, attackRange);
        
        foreach (var monster in monsters)
        {
            if (monster.CompareTag("Monster"))
            {
                // 몬스터에게 데미지 입히기
                battleManager.HandleCombatCollision(this.gameObject, monster.gameObject,playerDamage, transform.position);
            }
        }
    }

    void DrawGizmons()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private IEnumerator KnockDownSequence(Vector2 monsterPosition)
    {
        isKnockedDown = true;
        isInvincible = true;

        playerAnimator.SetTrigger("KnockDown");
        ApplyKnockBackDown(monsterPosition, knockBackForce * 10f);

        yield return new WaitForSeconds(knockDownDuration);

        playerAnimator.SetTrigger("GetUp");
    }

    public void ApplyKnockBackDown(Vector2 monsterPosition, float force)
    {
        Vector2 knockBackDirection = ((Vector2)transform.position - monsterPosition).normalized;
        PlayerRigidBody.velocity = Vector2.zero;
        PlayerRigidBody.AddForce(knockBackDirection * force, ForceMode2D.Impulse);
    }
    public void OnGetUpFinished()
    {
        isInvincible = false;
        isKnockedDown = false;
    }
    


    // 애니메이션 이벤트로 호출될 메서드
    public void OnAttackFinished()
    {
        isAttacking = false;
        if (isUsingSkill)
        {
            isUsingSkill = false;
            Debug.Log("스킬 사용 종료");
        }
        Debug.Log("공격 종료");
    }
    
    public void OnHitFinished()
    {
        ishitted = false;
    }

    public void InitCommandArray()
    {
        for (int i = 0; i < commandArray.Length; i++)
        {
            commandArray[i] = 0;
        }
    }

    public void CheckAndAddCommand((KeyCode key, int value) keyValuePair)
    {
        if (isUsingSkill || isAttacking || isKnockedDown) return;
        if (Input.GetKeyDown(keyValuePair.key))
        {
            AddValue(keyValuePair.value);
            NormalAttack(keyValuePair);
            Debug.Log("커맨드 배열에 추가: " + keyValuePair.value + " 배열: " + string.Join(", ", commandArray));
        }
    }

    void AddValue(int newValue)
    {
        for (int i = 0; i < commandArray.Length - 1; i++)
        {
            commandArray[i] = commandArray[i+1];
        }
        commandArray[commandArray.Length - 1] = newValue;

        CommandInput();

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(ResetCommandArray());
    }

    private IEnumerator ResetCommandArray()
    {
        yield return new WaitForSeconds(3f);
        InitCommandArray();
        Debug.Log("커맨드 배열 초기화.");
    }
    void CommandInput()
    {
        // 이미 공격 중이거나 스킬 사용 중이면 리턴
        if (isAttacking || isUsingSkill) return;

        if (isStage1 && commandArray.SequenceEqual(new int[] { 2, 4, 2, 3 }))
        {
            Debug.Log("커맨드 배열이 2424입니다.");
            isAttacking = true;
            isUsingSkill = true;  // 스킬 사용 상태 설정
            playerAnimator.SetTrigger("Skill1");
            playerDamage = 15;
            InitCommandArray();
        }
        
        if (isStage2 && commandArray.SequenceEqual(new int[] { 2, 4, 2, 2 }))
        {
            Debug.Log("커맨드 배열이 2422입니다.");
            isAttacking = true;
            isUsingSkill = true;
            playerAnimator.SetTrigger("Skill2");
            playerDamage = 20;
        }

        if (isStage3 && commandArray.SequenceEqual(new int[] { 1, 2, 3, 4}))
        {
            Debug.Log("커맨드 배열이 1234입니다.");
            isAttacking = true;
            isUsingSkill = true;
            playerAnimator.SetTrigger("Skill3");
            playerDamage = 25;
        }
    }

}