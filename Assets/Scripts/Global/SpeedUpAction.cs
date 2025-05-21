using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Actions/SpeedUp")]
public class SpeedUpAction : ScriptableObject, IItemAction
{
    [Header("버프 강도")]
    public float speedIncrease;
    [Header("지속 시간(초)")]
    public float duration;

    public void Execute(Player player)
    {
        player.controller.ApplySpeedUp(speedIncrease, duration);
    }
}
