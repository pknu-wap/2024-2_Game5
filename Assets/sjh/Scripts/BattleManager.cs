using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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


    public Text timeText;
    // 플레이어 변수
    private GameObject player;
    private PlayerController playerController; 
    public int defaultPlayerHP;
    public int playerHP;
    //private Rigidbody2D PlayerRigidBody;
    public Vector3 playerSpawnPos;
    

    // 보스 변수 
    private GameObject monster;
    private BossController monsterController;
    public int  bossHP;
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
        monsterController = monster.GetComponent<BossController>();
        bossHP = monsterController.bossHP;
        
        InitGame();
    }

    void InitGame()
    {
        // 게임 상태를 Ready로 전환
        ChangeState(State.Play); 
        playerHP = defaultPlayerHP;
        playerController.InitCommandArray();
        player.transform.position = playerSpawnPos;
        limitTime = defaultLimittime;
        if (isBossScene) playerController.playerAnimator.SetTrigger("Start");
    }

    public void ChangeState(State state)
    {
        curState = state;
    }

    void Decision() // 승자 판정
    {
        if (!isBossScene) return; 

        if (playerController.playerHP <= 0)
        {
            ChangeState(State.KO);
        }

        if (limitTime == 0) // TKO 판정
        {
            winner = playerController.playerHP > monsterController.bossHP ? Winner.Player : Winner.Monster;
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

        int previousTime = Mathf.FloorToInt(limitTime);
        limitTime -= Time.deltaTime;
        int currentTime = Mathf.FloorToInt(limitTime);
        timeText.text = currentTime.ToString();
        if (limitTime <= 0)
        {
            limitTime = 0;
            Debug.Log("Time is Over.");
            Decision();
        }
    }


  
    void Update()
    {
        switch(curState)
        {
             case State.Ready:
                //if (Input.GetKeyDown(KeyCode.LeftShift))
                //{
                //    ChangeState(State.Play);
                //    Debug.Log("Pressed : Play");
                //    Debug.Log(curState);   
                //}
                ChangeState(State.Play);
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


}