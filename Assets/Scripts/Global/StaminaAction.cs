using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 효과 - 활력 업
[CreateAssetMenu(menuName ="Items/Actions/Stamina")]
public class StaminaAction : ScriptableObject, IItemAction
{
    public int healStaminaAmount;
    public void Execute(Player player)
    {
        player.condition.AddStamona(healStaminaAmount);
    }
}
