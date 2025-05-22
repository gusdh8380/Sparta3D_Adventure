using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public PlayerEquipment equipment;


    public Action<ItemData> addItem;
    public Action<ItemData> addEquipItem;

    public Transform dropPosition;

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equipment = GetComponent<PlayerEquipment>();
    }
    public void AddItem(ItemData data)
    {
        if (data.type == ItemType.Consumable)
        {
            addItem?.Invoke(data); // 기존 소비형 인벤토리
        }
        else if (data.type == ItemType.Equipable)
        {
            addEquipItem?.Invoke(data); // 새로 추가할 델리게이트
        }

    }

}
