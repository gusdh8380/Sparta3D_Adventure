using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾� ���� UI ����
public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition stamina;

    private void Start()
    {
        PlayerManager.Instance.Player.condition.uiCondition = this;
    }
}
