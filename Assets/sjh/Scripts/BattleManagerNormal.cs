/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleManagerNormal : MonoBehaviour
{
    public enum State
    {
        Ready, // 0
        Play,  // 1
        End,
    }




    private SceneSetting sceneSetting;
    private bool isBossScene;

    // 플레이어 변수
    public GameObject player;
    private PlayerController playerController; 
    public int defaultPlayerHP;
    public int playerHP;
    public int playerDamage;
    //private Rigidbody2D PlayerRigidBody;
    private Vector3 playerSpawnPos;
    

    // 보스 변수 
    public GameObject monster;
    private MonsterController monsterController;
    public int  MonsterHP;
    public int monsterDamage;
    private Vector3 MonsterSpawnPos;
    public State curState;


    void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        monsterController = monster.GetComponent<MonsterController>();
        MonsterHP = 100; // 나중에 보스 스크립트에서 가져오는 아래 코드로 수정
        MonsterHP = monsterController.monsterHP;
    }

    

    void Start()
    {
        playerSpawnPos = new Vector3(-5, 2, -2);
        InitGame();
    }

    void InitGame()
    {
        // 게임 상태를 Ready로 전환
        ChangeState(State.Ready); 
        playerHP = defaultPlayerHP;
        playerController.InitCommandArray();
        player.transform.localPosition = playerSpawnPos;
        playerController.playerAnimator.SetTrigger("Start");
    }

    void ChangeState(State state)
    {
        curState = state;
    }

    void Decision() // 승자 판정
    {
        if (playerHP == 0)
        {
            playerController.playerAnimator.SetTrigger("Death");
            ChangeState(State.End);
            GameOver();
    
        }        
    }

    void GameOver()
    {
        Debug.Log("Game Over");
    }


    public void HandleCombatCollision(GameObject attacker, GameObject defender, int damage, Vector2 hitPosition)
    {
        if (curState != State.Play) return;

        // 플레이어가 몬스터를 공격
        if (attacker.CompareTag("Player") && defender.CompareTag("Monster"))
        {
            MonsterController monster = defender.GetComponent<MonsterController>();
            monster.monsterHP -= damage;
            if (monster.monsterHP <= 0)
            {
                monster.monsterHP = 0;
                GameOver();
                ChangeState(State.End);
            }
            Debug.Log($"Player hit Monster for {damage} damage. Monster HP: {monster.monsterHP}");
        }
        // 몬스터가 플레이어를 공격
        else if (attacker.CompareTag("Monster") && defender.CompareTag("Player"))
        {
            playerHP -= damage;  
            if (playerHP <= 0)
            {
                playerHP = 0;
                playerController.playerAnimator.SetTrigger("Death");
                GameOver();
                ChangeState(State.End);
            }
            playerController.TakeDamage(damage, hitPosition);  // 넉백 등 시각적 효과용
            Debug.Log($"Monster hit Player for {damage} damage. Player HP: {playerHP}");
        }
    }

    void AttackMonstersInRange()
    {
        // 플레이어의 공격 범위 내 몬스터 감지
        Collider2D[] monsters = Physics2D.OverlapCircleAll(player.transform.position, playerController.attackRange);
        
        foreach (var monster in monsters)
        {
            if (monster.CompareTag("Monster"))
            {
                // 몬스터에게 데미지 입히기
                HandleCombatCollision(player, monster.gameObject, playerController.playerDamage, Vector2.zero);
            }
        }
    }

    void DrawGizmons()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


  
    void Update()
    {
        switch(curState)
        {
             case State.Ready:
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    ChangeState(State.Play);
                    Debug.Log("Pressed : Play");
                    Debug.Log(curState);   
                }
                  break;
            case State.Play:
                if (curState != State.Play || playerController == null || playerController.isKnockedDown) return;
                playerController.Move();
                playerController.Guard();
                // 넉다운 상태, 공격중, 피격중일 때는 입력을 받지 않음
                if (!playerController.isKnockedDown || playerController.isAttacking || playerController.ishitted)
                {
                    Decision();
                    playerController.CheckAndAddCommand(playerController.W);
                    playerController.CheckAndAddCommand(playerController.A);
                    playerController.CheckAndAddCommand(playerController.S);
                    playerController.CheckAndAddCommand(playerController.D);
                }
                break;
            case State.End:
                GameOver();
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    InitGame();
                }
                break;
        }
    }

    void FixedUpdate()
    {
        
    }
}


*/