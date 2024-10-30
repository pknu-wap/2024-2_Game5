using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHit : MonoBehaviour
{
    public Sprite idleSprite;     // ���� ��������Ʈ
    public Sprite moveSprite1;    // ������ ��������Ʈ 1
    public Sprite moveSprite2;    // ������ ��������Ʈ 2
    public float hitDuration = 1f; // ��������Ʈ ���� ���� �ð�

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idleSprite; // �ʱ� ��������Ʈ ����
    }

    void Update()
    {
        // Sprite �ٲٱ� ���� Ÿ�̸� üũ
        if (spriteRenderer.sprite == moveSprite1 || spriteRenderer.sprite == moveSprite2)
        {
            hitDuration -= Time.deltaTime;
            if (hitDuration <= 0)
            {
                // �ð� �ʰ� �� idle ��������Ʈ�� ���ư�
                spriteRenderer.sprite = idleSprite;
                hitDuration = 1f; // Ÿ�̸� �ʱ�ȭ
            }
        }
    }

    // ����ƺ� ���� �� ȣ��
    public void OnHit()
    {
        // ��������Ʈ ����
        if (spriteRenderer.sprite == idleSprite)
        {
            spriteRenderer.sprite = moveSprite1;
        }
        else if (spriteRenderer.sprite == moveSprite1)
        {
            spriteRenderer.sprite = moveSprite2;
        }
        else
        {
            spriteRenderer.sprite = moveSprite1; // �⺻ ����
        }

        // Ÿ�̸� �ʱ�ȭ
        hitDuration = 1f; // ��������Ʈ ���� ���� �ð� �缳��
    }
}
