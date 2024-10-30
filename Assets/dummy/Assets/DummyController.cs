using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    public Sprite idleSprite;     // ��� ��������Ʈ
    public Sprite moveSprite1;    // ������ ��������Ʈ 1
    public Sprite moveSprite2;    // ������ ��������Ʈ 2
    public float detectionRadius = 5f; // �� ���� �ݰ�
    public Transform enemy;        // ���� Transform

    private SpriteRenderer spriteRenderer;
    private bool isEnemyInRange = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idleSprite; // �ʱ� ��������Ʈ ����
    }

    void Update()
    {
        // ���� ������ �ݰ� �ȿ� �ִ��� Ȯ��
        isEnemyInRange = Vector2.Distance(transform.position, enemy.position) <= detectionRadius;

        if (isEnemyInRange)
        {
            // ���� ���� �ȿ� ������ ��������Ʈ ����
            ChangeSprite();
        }
        else
        {
            // ���� ������ ��� ��������Ʈ�� ���ư���
            spriteRenderer.sprite = idleSprite;
        }
    }

    private float spriteChangeTime = 0.5f; // ��������Ʈ ���� �ֱ�
    private float timer = 0f;

    private void ChangeSprite()
    {
        timer += Time.deltaTime;

        if (timer >= spriteChangeTime)
        {
            // ���� ��������Ʈ�� ���� ����
            if (spriteRenderer.sprite == moveSprite1)
            {
                spriteRenderer.sprite = moveSprite2;
            }
            else
            {
                spriteRenderer.sprite = moveSprite1;
            }

            timer = 0f; // Ÿ�̸� �ʱ�ȭ
        }
    }
}
