using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class AllGroundsScroll : MonoBehaviour
{
    //여기에서 주석처리한 부분은 배경 여러개로 쓸 때 주석 지우면 됨
    //public int NumberOfThis;    //왼쪽부터 몇번째 배경인지(0부터 시작)

    //Transform Player_tr;      여기는 주석풀 때 스타트에서 겟컴포넌트도 주석풀고
    //Rigidbody2D Player_rigid;
    //Transform Camera_tr;



    [SerializeField]
    private Transform Target;   //현재 배경과 이어지는 배경
    [SerializeField]
    private float ScrollAmount; //이어지는 배경 사이의 거리
    [SerializeField]
    private float MoveSpeed;    //배경의 이동 속도 - 플레이어 중심으로 하면 필요x
    [SerializeField]
    private Vector3 MoveDirection;//이동 방향

    // Start is called before the first frame update
    void Start()
    {        
        Player_tr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Player_rigid = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        Camera_tr = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        //화면비율만큼 기본 크기에 곱하기 - 이미지 크기 조절(다양한 화면 크기에서 완벽하진 않지만, 대체적으로 비슷하게 보임)
        transform.localScale = new Vector2(transform.localScale.x * (Screen_w / Screen_h), transform.localScale.y * (Screen_w / Screen_h));
        //이어지는 배경 사이의 거리 정하기
        ScrollAmount = 4 * transform.localScale.x;    //개선필요
        //배경 위치 정하기 - 처음에 화면 안 벗어나게 x는 -1만큼 곱하기
        transform.position = new Vector2(ScrollAmount*(NumberOfThis-1), transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate() 
    {
        //플레이어 이동 방향에 따라 움직일 방향 정하기/ 지금 플레이어 움직이는 알고리즘이 포지션 수치 변화인 듯. 안됨
        //if (Player_rigid.velocity.x > 0)  MoveDirection = new (1,0,0);
        //if (Player_rigid.velocity.x < 0) MoveDirection = new (-1,0,0);

        // 배경이 MoveDirection의 방향으로 MoveSpeed의 속도로 이동
        transform.position += MoveDirection * MoveSpeed * Time.deltaTime;

        /
        //배경이 왼쪽으로 가다가 범위를 벗어나면 위치 재설정 - 맨 처음에 빈 공간 안 생기게 적당하게1.2곱합
        if (transform.position.x <= -1.2*ScrollAmount)
        {
            transform.position = Target.position - MoveDirection * ScrollAmount; //개선필요-양 방향 왔다갔다하면 2개만 순환함
        }
        //배경이 오른쪽으로 가다가 범위를 벗어나면 위치 재설정
        if (transform.position.x >= -1.2 * ScrollAmount)
        {
            transform.position = Target.position + MoveDirection * ScrollAmount; //개선필요-양 방향 왔다갔다하면 2개만 순환함
        }
        
    }


}
*/
