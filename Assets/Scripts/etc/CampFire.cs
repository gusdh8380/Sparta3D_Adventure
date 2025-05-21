using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float damageRate;

    public CampFireHeal CampFireHeal;

    List<IDamageable> things = new List<IDamageable>();

    private void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);
        CampFireHeal.healEnabled = false;
    }
    void DealDamage()
    {
        for(int i = 0; i<things.Count; i++)
        {
            things[i].TakeDamage(damage);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageble))
        {
            things.Add(damageble);
            CampFireHeal.healEnabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageble))
        {
            things.Remove(damageble);
            CampFireHeal.healEnabled=true;
        }
    }

}
