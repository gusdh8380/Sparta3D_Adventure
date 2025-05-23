using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 상태 UI 관리
public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition stamina;

    private void Start()
    {
        PlayerManager.Instance.Player.condition.uiCondition = this;
    }
}
