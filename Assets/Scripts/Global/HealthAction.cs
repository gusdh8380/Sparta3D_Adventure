using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Actions/Health")]
public class HealthAction : ScriptableObject, IItemAction
{
    public int healAmount;
    public void Execute(Player player)
    {
        player.condition.Heal(healAmount);
    }
}
