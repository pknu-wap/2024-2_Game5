using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D PlayerRigidBody;
    
    public float movePower = 8f;
    public float jumpPower = 8f;
    public float playerHP;
    public int playerDamage;
    public Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;

    private bool isWalking = false;
    private bool isJumping = false;
    public bool isGround = false;
    public bool isAttacking = false;
    private bool isGuarding = false;
    private bool isUsingSkill = false;
    public bool ishitted = false;
    private bool ishitting = false;
    private bool isInvincible = false;
    public bool isKnockedDown = false;
    private float knockDownDuration = 3f;
    private float knockBackForce= 20f;
    private int knockDownThreshold = 30;
    private float lastAttackTime = 0f;
    private float attackCooldown = 0.7f; // 데미지가 들어가는 간격 (초)


    private bool isBossScene;
    private bool isStage1;
    private bool isStage2;
    private bool isStage3;

    int[] commandArray = new int[4];
    private Coroutine resetCoroutine;

    public (KeyCode key, int value) W = (KeyCode.W, 1);
    public (KeyCode key, int value) A = (KeyCode.A, 2);
    public (KeyCode key, int value) S = (KeyCode.S, 3);
    public (KeyCode key, int value) D = (KeyCode.D, 4);

    private BattleManager battleManager;

    public GameObject monster;
    private MonsterController monsterController;
    private BossController bossController;
    private float attackRange = 1.6f;
    public LayerMask objectLayer;

    [SerializeField] private Slider _hpBar;

    public GameObject gameOvereUI;
    public GameObject gameClearUI;

    public Vector3 minPos;
    public Vector3 maxPos;

    private GameObject dummy;
    private bool isDummy=false;


    void Awake()
    {
        PlayerRigidBody = this.GetComponent<Rigidbody2D>();
        playerAnimator = this.GetComponent<Animator>();
        playerSpriteRenderer = this.GetComponent<SpriteRenderer>();
        battleManager = FindObjectOfType<BattleManager>();
        dummy = GameObject.FindWithTag("Dummy");
        monster = GameObject.FindWithTag("Monster");

    }

    void Start()
    {
        gameOvereUI.SetActive(false);
        isBossScene = System.Convert.ToBoolean(PlayerPrefs.GetInt("isBossScene"));
        isStage1 = System.Convert.ToBoolean(PlayerPrefs.GetInt("isStage1"));
        isStage2 = System.Convert.ToBoolean(PlayerPrefs.GetInt("isStage2"));
        isStage3 = System.Convert.ToBoolean(PlayerPrefs.GetInt("isStage3"));

        if(isBossScene) bossController = monster.GetComponent<BossController>();

        playerHP = 100;
        _hpBar.maxValue = playerHP;
        _hpBar.value = playerHP;
        
        InitCommandArray();
    }

    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            isJumping = false;
        }


        // 플레이어의 공격이 몬스터에게 히트
        if (isBossScene &&collision.gameObject.CompareTag("Monster")&&isAttacking)
        {
            ishitting = true;
        }
    }

    void DummyCall()
    {
        Debug.Log("Dummy Hit");
        DummyHit dummyHit = dummy.GetComponent<DummyHit>();
        dummyHit.OnHit();
        isDummy = false;
    }

    public void Move()
    {   
        // 공격 중에는 이동 불가
        if (isAttacking || ishitted) return;

        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isWalking = true;
            moveVelocity = Vector3.left;
            if(!isBossScene) playerSpriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            isWalking = true;
            moveVelocity = Vector3.right;
            if(!isBossScene) playerSpriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !isBossScene)
        {
            isWalking = true;
            moveVelocity = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && !isBossScene)
        {
            isWalking = true;
            moveVelocity = Vector3.up;
        }
        else
        {
            isWalking = false;
        }
        
        playerAnimator.SetBool("isWalking", isWalking);
        transform.position += moveVelocity * movePower * Time.deltaTime;
        
    
        if(!isBossScene)
        {
            Vector3 newPosition = transform.position;
            newPosition.x= Mathf.Clamp(newPosition.x, minPos.x, maxPos.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minPos.y, maxPos.y);

            // 새로운 위치로 이동
            transform.position = newPosition;
        }
        
        
    }

    public void Jump() 
    {
        if (isJumping || isAttacking || isUsingSkill || !isBossScene) return;

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

        if(ishitting)
        {
            if(isBossScene)bossController.TakeDamage(5, transform.position);
            if(!isBossScene)monsterController.TakeDamage(5, transform.position);
            ishitting = false;
        }

        
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

    public void TakeDamage(float damage, Vector2 playerPosition)
    {
        
        if (isInvincible || isKnockedDown) return;
        
        ApplyKnockBackDown(playerPosition, knockBackForce);

        if(isGuarding) return;

        ishitted = true;
        playerHP -= damage;
        _hpBar.value = playerHP;

        if(damage >= knockDownThreshold && !isKnockedDown)
        {
            StartCoroutine(KnockDownSequence(playerPosition));
        }
        else
        {
            playerAnimator.SetTrigger("Hit");
            ApplyKnockBackDown(playerPosition, knockBackForce);
        }
        Decision();
    }

    private void Decision()
    {
        if (playerHP <= 0)
        {
            playerAnimator.SetTrigger("Death");
            gameOvereUI.SetActive(true);
            
        }
    }

    public void AttackMonstersInRange()
    {
        if(isBossScene) return;

        // 쿨다운 체크
        if (Time.time - lastAttackTime < attackCooldown) return;

        // 플레이어의 공격 범위 내 몬스터 감지
        Collider2D[] monsters = Physics2D.OverlapCircleAll(transform.position, attackRange, objectLayer);
        Debug.Log(monsters.Length);
        
        foreach (var monster in monsters)
        {
            if ((monster.CompareTag("Monster") || monster.CompareTag("Dummy"))&& isAttacking &&!isDummy)
            {
                if (monster.CompareTag("Dummy"))
                {
                    isDummy = true;
                    DummyCall();
                }
                // 몬스터에게 데미지 입히기
                monsterController = monster.GetComponent<MonsterController>();
                monsterController.TakeDamage(playerDamage, transform.position);
                //battleManager.HandleCombatCollision(this.gameObject, monster.gameObject,playerDamage, Vector2.zero);
            }
        }

        lastAttackTime = Time.time;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private IEnumerator KnockDownSequence(Vector2 playerPosition)
    {
        isKnockedDown = true;
        isInvincible = true;

        playerAnimator.SetTrigger("KnockDown");
        ApplyKnockBackDown(playerPosition, knockBackForce * 10f);

        yield return new WaitForSeconds(knockDownDuration);

        playerAnimator.SetTrigger("GetUp");
    }

    public void ApplyKnockBackDown(Vector2 playerPosition, float force)
    {
        // 몬스터가 오른쪽에서 때렸으면 왼쪽으로, 왼쪽에서 때렸으면 오른쪽으로
        float direction = transform.position.x < playerPosition.x ? -1f : 1f;
        
        // x축으로만 속도 적용
        PlayerRigidBody.velocity = new Vector2(direction * force, 3f);
        
        // 짧은 시간 후에 속도를 0으로 만들어서 미끄러지지 않게 함
        StartCoroutine(StopKnockback());
    }

    private IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.2f);
        PlayerRigidBody.velocity = Vector2.zero;
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
            playerAnimator.SetBool("IsUsingSkill", isUsingSkill);
            Debug.Log("스킬 사용 종료");
        }

        playerAnimator.SetBool("isAttacking", isAttacking);
        Debug.Log("공격 종료");
    }
    
    public void OnHitFinished()
    {
        ishitted = false;
        isAttacking = false;
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

        if (isStage1 && commandArray.SequenceEqual(new int[] { 2, 4, 2, 4 }))
        {
            Debug.Log("커맨드 배열이 2424입니다.");
            isAttacking = true;
            isUsingSkill = true;  // 스킬 사용 상태 설정
            playerAnimator.SetTrigger("Skill1");
            playerDamage = 15;
            InitCommandArray();
        }
        
        if (isStage2 && commandArray.SequenceEqual(new int[] { 2, 4, 1, 3 }))
        {
            Debug.Log("커맨드 배열이 2422입니다.");
            isAttacking = true;
            isUsingSkill = true;
            playerAnimator.SetTrigger("Skill2");
            playerDamage = 20;
            InitCommandArray();
        }

        if (isStage3 && commandArray.SequenceEqual(new int[] { 2, 1, 4, 3}))
        {
            Debug.Log("커맨드 배열이 1234입니다.");
            isAttacking = true;
            isUsingSkill = true;
            playerAnimator.SetTrigger("Skill3");
            playerDamage = 25;
            InitCommandArray();
        }
        playerAnimator.SetBool("IsUsingSkill", isUsingSkill);
    }

}