using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    public Sprite idleSprite;     // 대기 스프라이트
    public Sprite moveSprite1;    // 움직임 스프라이트 1
    public Sprite moveSprite2;    // 움직임 스프라이트 2
    public float detectionRadius = 5f; // 적 감지 반경
    public Transform enemy;        // 적의 Transform

    private SpriteRenderer spriteRenderer;
    private bool isEnemyInRange = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idleSprite; // 초기 스프라이트 설정
    }

    void Update()
    {
        // 적이 지정된 반경 안에 있는지 확인
        isEnemyInRange = Vector2.Distance(transform.position, enemy.position) <= detectionRadius;

        if (isEnemyInRange)
        {
            // 적이 범위 안에 들어오면 스프라이트 변경
            ChangeSprite();
        }
        else
        {
            // 적이 없으면 대기 스프라이트로 돌아가기
            spriteRenderer.sprite = idleSprite;
        }
    }

    private float spriteChangeTime = 0.5f; // 스프라이트 변경 주기
    private float timer = 0f;

    private void ChangeSprite()
    {
        timer += Time.deltaTime;

        if (timer >= spriteChangeTime)
        {
            // 현재 스프라이트에 따라 변경
            if (spriteRenderer.sprite == moveSprite1)
            {
                spriteRenderer.sprite = moveSprite2;
            }
            else
            {
                spriteRenderer.sprite = moveSprite1;
            }

            timer = 0f; // 타이머 초기화
        }
    }
}
