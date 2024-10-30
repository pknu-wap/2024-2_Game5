using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class AllGroundsScroll : MonoBehaviour
{
    //���⿡�� �ּ�ó���� �κ��� ��� �������� �� �� �ּ� ����� ��
    //public int NumberOfThis;    //���ʺ��� ���° �������(0���� ����)

    //Transform Player_tr;      ����� �ּ�Ǯ �� ��ŸƮ���� ��������Ʈ�� �ּ�Ǯ��
    //Rigidbody2D Player_rigid;
    //Transform Camera_tr;



    [SerializeField]
    private Transform Target;   //���� ���� �̾����� ���
    [SerializeField]
    private float ScrollAmount; //�̾����� ��� ������ �Ÿ�
    [SerializeField]
    private float MoveSpeed;    //����� �̵� �ӵ� - �÷��̾� �߽����� �ϸ� �ʿ�x
    [SerializeField]
    private Vector3 MoveDirection;//�̵� ����

    // Start is called before the first frame update
    void Start()
    {        
        Player_tr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Player_rigid = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        Camera_tr = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        //ȭ�������ŭ �⺻ ũ�⿡ ���ϱ� - �̹��� ũ�� ����(�پ��� ȭ�� ũ�⿡�� �Ϻ����� ������, ��ü������ ����ϰ� ����)
        transform.localScale = new Vector2(transform.localScale.x * (Screen_w / Screen_h), transform.localScale.y * (Screen_w / Screen_h));
        //�̾����� ��� ������ �Ÿ� ���ϱ�
        ScrollAmount = 4 * transform.localScale.x;    //�����ʿ�
        //��� ��ġ ���ϱ� - ó���� ȭ�� �� ����� x�� -1��ŭ ���ϱ�
        transform.position = new Vector2(ScrollAmount*(NumberOfThis-1), transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate() 
    {
        //�÷��̾� �̵� ���⿡ ���� ������ ���� ���ϱ�/ ���� �÷��̾� �����̴� �˰����� ������ ��ġ ��ȭ�� ��. �ȵ�
        //if (Player_rigid.velocity.x > 0)  MoveDirection = new (1,0,0);
        //if (Player_rigid.velocity.x < 0) MoveDirection = new (-1,0,0);

        // ����� MoveDirection�� �������� MoveSpeed�� �ӵ��� �̵�
        transform.position += MoveDirection * MoveSpeed * Time.deltaTime;

        /
        //����� �������� ���ٰ� ������ ����� ��ġ �缳�� - �� ó���� �� ���� �� ����� �����ϰ�1.2����
        if (transform.position.x <= -1.2*ScrollAmount)
        {
            transform.position = Target.position - MoveDirection * ScrollAmount; //�����ʿ�-�� ���� �Դٰ����ϸ� 2���� ��ȯ��
        }
        //����� ���������� ���ٰ� ������ ����� ��ġ �缳��
        if (transform.position.x >= -1.2 * ScrollAmount)
        {
            transform.position = Target.position + MoveDirection * ScrollAmount; //�����ʿ�-�� ���� �Դٰ����ϸ� 2���� ��ȯ��
        }
        
    }


}
*/
