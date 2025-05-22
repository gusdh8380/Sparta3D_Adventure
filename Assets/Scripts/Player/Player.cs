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
            addItem?.Invoke(data); // ���� �Һ��� �κ��丮
        }
        else if (data.type == ItemType.Equipable)
        {
            addEquipItem?.Invoke(data); // ���� �߰��� ��������Ʈ
        }

    }

}
