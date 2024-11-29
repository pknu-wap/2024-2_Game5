using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public GameObject gameOverUI;
    public GameObject gameClearUI;
    private bool isGamePaused;
    
    private GameObject dialogBox;
    private MiddleDialog middleDialog;
    private bool isDialogueFinished;

    public string sceneName;

    void Awake()
    {

        sceneSetting = FindObjectOfType<SceneSetting>();
        isBossScene = System.Convert.ToBoolean(PlayerPrefs.GetInt("isBossScene"));
        
    }

    

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameClearUI.SetActive(false);
        dialogBox = GameObject.FindWithTag("Dialog");
        middleDialog = dialogBox.GetComponent<MiddleDialog>();
        

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
        
        if (!isBossScene) return;
        playerHP = defaultPlayerHP;
        playerController.InitCommandArray();
        player.transform.position = playerSpawnPos;
        monster.transform.position = monsterSpawnPos;
        limitTime = defaultLimittime;
        if (isBossScene) playerController.playerAnimator.SetTrigger("Start");
        ChangeState(State.Ready); 
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
        if (playerController.playerHP <= 0)
        {
            winner = Winner.Monster;
            GameOver();
            ChangeState(State.KO);
        }

        if (!isBossScene) return; 

        if (bossController.bossHP <= 0)
        {
            winner = Winner.Player;
            GameOver();
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
            gameOverUI.SetActive(true);
        }
        else if (winner == Winner.Player)
        {
            gameClearUI.SetActive(true);
        }
    }

    void TimeCount()
    {
        if (!isBossScene) return;

        limitTime -= Time.deltaTime;
        int currentTime = Mathf.FloorToInt(limitTime);
        timeText.text = currentTime.ToString();
        if (currentTime <= 0)
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
                isDialogueFinished = middleDialog.isDialogueFinished;
                if (isDialogueFinished) ChangeState(State.Play);
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
                
                if(winner == Winner.Player && isBossScene)
                {
                    if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene(sceneName);
                }
                break;
        }
    }







}