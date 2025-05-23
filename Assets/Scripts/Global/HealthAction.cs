using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 효과 - 체력 회복 데이터
[CreateAssetMenu(menuName = "Items/Actions/Health")]
public class HealthAction : ScriptableObject, IItemAction
{
    public int healAmount;
    public void Execute(Player player)
    {
        player.condition.Heal(healAmount);
    }
}
