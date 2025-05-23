using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ȿ�� - Ȱ�� ��
[CreateAssetMenu(menuName ="Items/Actions/Stamina")]
public class StaminaAction : ScriptableObject, IItemAction
{
    public int healStaminaAmount;
    public void Execute(Player player)
    {
        player.condition.AddStamona(healStaminaAmount);
    }
}
