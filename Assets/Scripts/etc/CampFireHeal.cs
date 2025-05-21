using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CampFireHeal : MonoBehaviour
{
    public Condition condition;

    [SerializeField]
    private int heal;
    [SerializeField]
    private float healRate;

    public bool healEnabled;

    List<PlayerCondition> conditions = new List<PlayerCondition>();

    private void Update()
    {
        if (healEnabled)
        {
            Heal();
            Debug.Log("Ä¡À¯Áß");
        }
       
    }

    void Heal()
    {
        for(int i = 0; i < conditions.Count; i++)
        {
            conditions[i].Heal(heal);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCondition p))
        {
            conditions.Add(p);
        }
        healEnabled = true;
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCondition p))
        {
            conditions.Remove(p);
        }
        healEnabled=false;
    }



}
