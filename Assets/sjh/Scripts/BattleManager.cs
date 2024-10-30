using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleManager : MonoBehaviour
{
    public enum State
    {
        Ready, // 0
        Play,  // 1
        KO,    // 2
        TKO    // 3
    }

    public enum Winner
    {
        Player, // 0
        Monster    // 1

    }


    private SceneSetting sceneSetting;
    private bool isBossScene;

    // 플레이어 변수
    private GameObject player;
    private PlayerController playerController; 
    public int defaultPlayerHP;
    public int playerHP;
    public int playerDamage;
    //private Rigidbody2D PlayerRigidBody;
    public Vector3 playerSpawnPos;
    

    // 보스 변수 
    private GameObject monster;
    private MonsterController monsterController;
    public int  MonsterHP;
    public int monsterDamage;
    private Vector3 MonsterSpawnPos;

    private State curState;
    public float defaultLimittime = 100.0f;
    public float limitTime;
    private Winner winner; // 열거형으로 변경

    void Awake()
    {

        sceneSetting = FindObjectOfType<SceneSetting>();
        isBossScene = System.Convert.ToBoolean(PlayerPrefs.GetInt("isBossScene"));
        
    }

    

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (isBossScene) monster = GameObject.FindWithTag("Monster");
        playerController = player.GetComponent<PlayerController>();
        monsterController = monster.GetComponent<MonsterController>();
        MonsterHP = monsterController.monsterHP;
        
        InitGame();
    }

    void InitGame()
    {
        // 게임 상태를 Ready로 전환
        ChangeState(State.Ready); 
        playerHP = defaultPlayerHP;
        playerController.InitCommandArray();
        player.transform.position = playerSpawnPos;
        limitTime = defaultLimittime;
        if (isBossScene) playerController.playerAnimator.SetTrigger("Start");
    }

    void ChangeState(State state)
    {
        curState = state;
    }

    void Decision() // 승자 판정
    {
        if (!isBossScene) return; 

        if (limitTime == 0) // TKO 판정
        {
            winner = playerHP > MonsterHP ? Winner.Player : Winner.Monster;
            GameOver();
            ChangeState(State.TKO);
        }

        
    }

    void GameOver()
    {
        if (winner == Winner.Monster)
        {
            Debug.Log("Monster Wins! Game Over." + curState);
        }
        else if (winner == Winner.Player)
        {
            Debug.Log("Player Wins! Congratulations!" + curState);
        }
    }

    void TimeCount()
    {
        if (!isBossScene) return;

        limitTime -= Time.deltaTime;       
        //Debug.Log(limitTime);
        if (limitTime <= 0)
        {
            limitTime = 0;
            Debug.Log("Time is Over.");
            Decision(); // 시간이 다 되면 승자 결정
        }
    }

    public void HandleCombatCollision(GameObject attacker, GameObject defender, int damage, Vector2 hitPosition)
    {
        if (curState != State.Play) return;

        // 플레이어가 몬스터를 공격
        if (attacker.CompareTag("Player") && defender.CompareTag("Monster"))
        {
            MonsterHP -= damage;
            if (MonsterHP <= 0)
            {
                MonsterHP = 0;
                
                if (isBossScene)
                {
                    winner = Winner.Player;
                    ChangeState(State.KO);
                    GameOver();
                }
            }
            Debug.Log($"Player hit Monster for {damage} damage. Monster HP: {MonsterHP}");
        
        }
        // 몬스터가 플레이어를 공격
        else if (attacker.CompareTag("Monster") && defender.CompareTag("Player"))
        {
            playerHP -= damage;  
            if (playerHP <= 0)
            {
                playerHP = 0;

                playerController.playerAnimator.SetTrigger("Death");

                if (isBossScene)
                {
                    winner = Winner.Monster;
                    ChangeState(State.KO);
                    GameOver();
                }
            }
            playerController.TakeDamage(damage, hitPosition);  // 넉백 등 시각적 효과용
            Debug.Log($"Monster hit Player for {damage} damage. Player HP: {playerHP}");
        }
        return; 
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
                TimeCount();
                playerController.AttackMonstersInRange();
                if (curState != State.Play || playerController == null || playerController.isKnockedDown) return;
                playerController.Move();
                playerController.Guard();
                playerController.Jump();
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
            case State.KO: case State.TKO:
                Debug.Log(curState+ "THE WINNER IS " + winner);
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