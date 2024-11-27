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
    private BossController bossController;
    public float  bossHP;
    public int monsterDamage;
    public Vector3 monsterSpawnPos;

    private State curState;
    public float defaultLimittime = 100.0f;
    public float limitTime;
    private Winner winner; // 열거형으로 변경

    public GameObject pauseUI;
    private bool isGamePaused;

    void Awake()
    {

        sceneSetting = FindObjectOfType<SceneSetting>();
        isBossScene = System.Convert.ToBoolean(PlayerPrefs.GetInt("isBossScene"));
        
    }

    

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pauseUI.SetActive(false);
        playerController = player.GetComponent<PlayerController>();
        if (isBossScene) 
        {
            monster = GameObject.FindWithTag("Monster");
            bossController = monster.GetComponent<BossController>();
            bossHP = bossController.bossHP;
        }
        
        
        InitGame();
    }

    void InitGame()
    {
        
        ChangeState(State.Play); 
        if (!isBossScene) return;
        playerHP = defaultPlayerHP;
        playerController.InitCommandArray();
        player.transform.position = playerSpawnPos;
        monster.transform.position = monsterSpawnPos;
        limitTime = defaultLimittime;
        if (isBossScene) playerController.playerAnimator.SetTrigger("Start");
    }

    public void ChangeState(State state)
    {
        curState = state;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        isGamePaused = false;
        Debug.Log("Pause Screen Closed");
        pauseUI.SetActive(false);
        ChangeState(State.Play);
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
            winner = playerController.playerHP > bossController.bossHP ? Winner.Player : Winner.Monster;
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
                PauseUIDelay();
                if (Input.GetKeyDown(KeyCode.Escape) && isGamePaused)
                {
                    Time.timeScale = 1;
                    isGamePaused = false;
                    Debug.Log("Pause Screen Closed");
                    pauseUI.SetActive(false);
                    ChangeState(State.Play);
                }
                  break;
            case State.Play:
                if (Input.GetKeyDown(KeyCode.Escape) && !isGamePaused)
                {
                    isGamePaused = true;
                    Debug.Log("Pause Screen Opened");
                    pauseUI.SetActive(true);
                    Time.timeScale = 0;
                    PauseUIDelay();
                    ChangeState(State.Ready);
                    
                }
                TimeCount();
                if (!isBossScene)playerController.AttackMonstersInRange();
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

    private IEnumerator PauseUIDelay()
    {
        yield return new WaitForSeconds(1f);
    }






}