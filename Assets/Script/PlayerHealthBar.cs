using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealthBar : MonoBehaviour
{
    protected float curHealth; //* 현재 체력
    public float maxHealth; //* 최대 체력
    private Slider HpBarSlider;

    // Start is called before the first frame update
    void Start()
    {
        HpBarSlider = GetComponent<Slider>();
        SetHp(100);
    }

    // Update is called once per frame
    void Update()
    {
        Damage(1);
        //소유자 히트박스 충돌 시 Damage(float damage) 작동
    }

    public void SetHp(float amount) //*Hp설정
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }


    public void CheckHp() //*HP 갱신
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }

    public void Damage(float damage) //* 데미지 받는 함수
    {
        if (maxHealth == 0 || curHealth <= 0) //* 이미 체력 0이하면 패스
            return;
        curHealth -= damage;
        CheckHp(); //* 체력 갱신
        if (curHealth <= 0)
        {
            print("dead");
            //* 체력이 0 이하라 죽음
        }
    }
}
