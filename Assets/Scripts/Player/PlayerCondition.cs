using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;
    public event Action onTakeDamage;

    Condition health { get {  return uiCondition.health; } }
    private void Update()
    {
        if (health.curValue == 0)
            Die();
    }
    public void Heal(int a)
    {
        health.Add(a);
    }
    public void Die()
    {
        
    }

    public void TakeDamage(int a)
    {
        health.Subtract(a);
        onTakeDamage?.Invoke();
    }

   
}
