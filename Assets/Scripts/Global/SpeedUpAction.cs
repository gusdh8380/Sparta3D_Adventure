using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Actions/SpeedUp")]
public class SpeedUpAction : ScriptableObject, IItemAction
{
    [Header("���� ����")]
    public float speedIncrease;
    [Header("���� �ð�(��)")]
    public float duration;

    public void Execute(Player player)
    {
        player.controller.ApplySpeedUp(speedIncrease, duration);
    }
}
