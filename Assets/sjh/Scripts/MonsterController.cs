using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public bool isAttacking = false;
    public int monsterHP = 200 ;
    public int monsterDamage = 10;  // 몬스터가 플레이어에게 주는 데미지

    public GameObject player;
    private PlayerController playerController;
    public bool isInvincible = false;
    public bool isGuarding = false;
    public bool ishitted = false;
    public bool isHitting = false;
    public float knockBackForce = 3f;

    public void TakeDamage(int damage, Vector2 playerPosition)
    {
 
        ishitted = true;
        monsterHP -= damage;
        ishitted = false;

   
    }
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHitting) return;
        if (collision.gameObject.CompareTag("Player") && isAttacking)
        {
            isHitting = true;
            isAttacking = false;
            Debug.Log("플레이어와 공격 중 충돌");
            Vector2 hitPosition = transform.position;

            if (playerController != null)
            {
                playerController.TakeDamage(monsterDamage, hitPosition);
            }
            isHitting= false;
            StartCoroutine(TempCooldown());
        }
    }

    private IEnumerator TempCooldown()
    {
        yield return new WaitForSeconds(0.1f); // 설정한 시간 대기
        isAttacking = true; // 쿨다운 후 충돌 허용
    }
}
