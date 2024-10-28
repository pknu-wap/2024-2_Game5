using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealthBar : MonoBehaviour
{
    protected float curHealth; //* ���� ü��
    public float maxHealth; //* �ִ� ü��
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
        //������ ��Ʈ�ڽ� �浹 �� Damage(float damage) �۵�
    }

    public void SetHp(float amount) //*Hp����
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }


    public void CheckHp() //*HP ����
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }

    public void Damage(float damage) //* ������ �޴� �Լ�
    {
        if (maxHealth == 0 || curHealth <= 0) //* �̹� ü�� 0���ϸ� �н�
            return;
        curHealth -= damage;
        CheckHp(); //* ü�� ����
        if (curHealth <= 0)
        {
            print("dead");
            //* ü���� 0 ���϶� ����
        }
    }
}
