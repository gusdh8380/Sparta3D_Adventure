using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;

    private void Start()
    {
        PlayerManager.Instance.Player.condition.uiCondition = this;
    }

}
