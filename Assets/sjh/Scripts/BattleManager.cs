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

    // 플레이어 변수
    public GameObject player;
    private PlayerController playerController; // PlayerScript로 가정
    public int defaultPlayerHP;
    public int playerHP;
    //private Rigidbody2D PlayerRigidBody;
    private Vector3 playerSpawnPos;
    

    // 보스 변수 
    //public GameObject Monster;
    public int  MonsterHP;
    private Vector3 MonsterSpawnPos;

    private State curState;
    public float limitTime;
    private Winner winner; // 열거형으로 변경

    void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        //PlayerRigidBody = player.GetComponent<Rigidbody2D>();
        MonsterHP = 100; // 나중에 보스 스크립트에서 가져오는 걸로 수정 필요
    }

    void Start()
    {
        playerSpawnPos = new Vector3(-5, 2, -2);
        InitGame();
        playerController.OnDamageTaken += HandlePlayerDamage;
    }

    void InitGame()
    {
        // 게임 상태를 Ready로 전환
        ChangeState(State.Ready); 
        playerHP = defaultPlayerHP;
        playerController.InitCommandArray();
        player.transform.localPosition = playerSpawnPos;
        limitTime = 100.0f; // 게임 시간 예시
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
            winner = Winner.Monster;
            ChangeState(State.KO);
            GameOver();
    
        }
        else if (MonsterHP == 0)
        {
            winner = Winner.Player;
            ChangeState(State.KO);
            GameOver();
        }

        else if (limitTime == 0) // TKO 판정
        {
            winner = playerHP > MonsterHP ? Winner.Player : Winner.Monster;
            ChangeState(State.TKO);
            GameOver();
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
        limitTime -= Time.deltaTime;       
        //Debug.Log(limitTime);
        if (limitTime <= 0)
        {
            limitTime = 0;
            Debug.Log("Time is Over.");
            Decision(); // 시간이 다 되면 승자 결정
        }
    }

    void HandlePlayerDamage(int damage)
    {
        if (curState != State.Play) return;
        playerHP -= damage;
        if (playerHP <= 0) playerHP = 0;

        if (playerHP == 0) Decision();
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
        if (curState != State.Play || playerController == null || playerController.isKnockedDown) 
            return;
        playerController.Move();
        playerController.Jump();
        playerController.Guard();
    }
    void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnDamageTaken -= HandlePlayerDamage;
        }
    }
}