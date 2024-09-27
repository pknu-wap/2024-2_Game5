using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject Player;
    private Rigidbody2D PlayerRigidBody;
    private Vector3 movement;
    
    public float movePower = 1f;
    public float jumpPower = 1f;
    private float playerHP;
    private float monsterDamage;
    private float playerDamage;

    bool isJumping = false;
    bool isGround = false;
    bool isAttacking = false;

    //커맨드(스킬) 해금 테스트 용 불
    public bool isStage1 = true;
    public bool isStage2 = false;
    public bool isStage3 = false;
    //

    int[] commandArray = new int[4];

    private Coroutine resetCoroutine;

    //커맨드 입력을 위한 네임드 튜플
    private (KeyCode key, int value) W = (KeyCode.W, 1);
    private (KeyCode key, int value) A = (KeyCode.A, 2);
    private (KeyCode key, int value) S = (KeyCode.S, 3);
    private (KeyCode key, int value) D = (KeyCode.D, 4);

    void Start()
    {
        playerHP = 100;

        Player = gameObject.GetComponent<GameObject>();
        PlayerRigidBody = gameObject.GetComponent<Rigidbody2D>();

        InitCommandArray(); //커맨드 배열 초기화
    }


    void Update()
    {
        CheckAndAddCommand(W);
        CheckAndAddCommand(A);
        CheckAndAddCommand(S);
        CheckAndAddCommand(D);


        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJumping = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            isAttacking = true;
        }
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        NormalAttack();
    }

    void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                isGround = true;
            }
                
        }


    void Move()
    {   
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveVelocity = Vector3.left;
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveVelocity = Vector3.right;
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }


    void Jump()
    {
        if (!isJumping)
        {
            return;
        }

        
        PlayerRigidBody.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        PlayerRigidBody.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;
        isGround = false;
    }

    void NormalAttack() // 일반 공격
    {
        if (!isAttacking)
        {
            return;
        }

        if (!isGround) //점프공격
        {
            monsterDamage = 8;
        }
        else //일반공격
        {
            monsterDamage = 5;
        }
        

        PlayerRigidBody.AddForce(Vector3.right * movePower / 2, ForceMode2D.Impulse);
        Debug.Log("일반(약) 공격, 몬스터 HP -" + monsterDamage);
        isAttacking = false;
    }

    void Guard()
    {

    }

    void InitCommandArray() //커맨드 배열 초기화 함수
    {
        for (int i = 0; i < commandArray.Length; i++)
        {
            commandArray[i] = 0;
        }
    }

    void CheckAndAddCommand((KeyCode key, int value) keyValuePair) // 키 확인 및 값 배열에 추가
    {
        if (Input.GetKeyDown(keyValuePair.key))
        {
            AddValue(keyValuePair.value);
            Debug.Log("커맨드 배열에 추가: " + keyValuePair.value + " 배열: " + string.Join(", ", commandArray));
        }
    }

    void AddValue(int newValue) // 값 추가
    {

        for (int i = 0; i < commandArray.Length - 1; i++)
        {
            commandArray[i]  = commandArray[i+1];

        }
        commandArray[commandArray.Length - 1] = newValue;


        CommandInput();


        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        resetCoroutine = StartCoroutine(ResetCommandArray()); //4초 지연을 위한 코루틴
    }

    private IEnumerator ResetCommandArray() // 4초 지연 코루틴
    {
        yield return new WaitForSeconds(3f);

        InitCommandArray();
        Debug.Log("커맨드 배열 초기화.");

    }

    void CommandInput() // 강중약 공격 커맨드 확인한 후 스킬 사용
    {
        if (isStage1)
        {
            if (commandArray.SequenceEqual(new int[] { 2, 4, 2, 4 }))
            {
                Debug.Log("커맨드 배열이 2424입니다.");
            }
        }
        
        if (isStage2)
        {
            if (commandArray.SequenceEqual(new int[] { 2, 4, 2, 2 }))
            {
                Debug.Log("커맨드 배열이 2422입니다.");
            }            
        }

        if (isStage3)
        {
            if (commandArray.SequenceEqual(new int[] { 1, 2, 3, 4}))
            {
                Debug.Log("커맨드 배열이 1234입니다.");
            }            
        }

    }

}
