using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    // HpBar Slider를 연동하기 위한 Slider 객체
    [SerializeField] private Slider _hpBar;
    private Transform PlayerTransform;

    public GameObject Gameover;  //Hierarchy에서 게임오버화면 붙여줘야함
    private GameObject Player;
    private Rigidbody2D PlayerRigidBody;
    private Vector3 movement;

    public float movePower = 5f;
    public float jumpPower = 1f;
    private float playerHP;
    private float monsterDamage;
    private float playerDamage;


    void Start()
    {
        Player = gameObject.GetComponent<GameObject>();
        PlayerRigidBody = gameObject.GetComponent<Rigidbody2D>();
        playerHP = 100;

        _hpBar.maxValue = playerHP;
        _hpBar.value = playerHP;
        PlayerTransform = gameObject.GetComponent<Transform>();

    }


    void Update()
    {
        _hpBar.value -= 1;
        if (_hpBar.value <= 0)
        {
            Gameover.SetActive(true);   //게임오버 띄우기
        }

    }

    void FixedUpdate()
    {
        Move();
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

        else if (Input.GetKey(KeyCode.DownArrow) && PlayerTransform.position.y > 0)
        {
            moveVelocity = Vector3.down;
        }

        else if (Input.GetKey(KeyCode.UpArrow)   && PlayerTransform.position.y < 4)
        {
                moveVelocity = Vector3.up;
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }


    
}