using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHit : MonoBehaviour
{
    public Sprite idleSprite;     // 평상시 스프라이트
    public Sprite moveSprite1;    // 움직임 스프라이트 1
    public Sprite moveSprite2;    // 움직임 스프라이트 2
    public float hitDuration = 1f; // 스프라이트 변경 지속 시간

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idleSprite; // 초기 스프라이트 설정
    }

    void Update()
    {
        // Sprite 바꾸기 위한 타이머 체크
        if (spriteRenderer.sprite == moveSprite1 || spriteRenderer.sprite == moveSprite2)
        {
            hitDuration -= Time.deltaTime;
            if (hitDuration <= 0)
            {
                // 시간 초과 시 idle 스프라이트로 돌아감
                spriteRenderer.sprite = idleSprite;
                hitDuration = 1f; // 타이머 초기화
            }
        }
    }

    // 허수아비를 때릴 때 호출
    public void OnHit()
    {
        // 스프라이트 변경
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
            spriteRenderer.sprite = moveSprite1; // 기본 동작
        }

        // 타이머 초기화
        hitDuration = 1f; // 스프라이트 변경 유지 시간 재설정
    }
}
