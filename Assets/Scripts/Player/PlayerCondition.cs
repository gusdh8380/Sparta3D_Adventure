using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾��� ���� ��ġ ����
public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;
    public event Action onTakeDamage;

    Condition health { get {  return uiCondition.health; } }
    public Condition stamina { get { return uiCondition.stamina; } }
    private void Update()
    {
        if (health.curValue == 0)
            Die();

        stamina.Add(stamina.passiveValue * Time.deltaTime);
    }
    public void Heal(int a)
    {
        health.Add(a);
    }
    public void AddStamona(int a)
    {
        stamina.Add(a);
    }
    public void Die()
    {
        
    }

    public void TakeDamage(int a)
    {
        health.Subtract(a);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float a)
    {
        if (stamina.curValue - a < 0f)
        {
            return false;
        }
        stamina.Subtract(a);
        return true;
    }
}
