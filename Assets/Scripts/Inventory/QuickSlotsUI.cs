using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickSlotsUI : MonoBehaviour
{
    [Header("UI 슬롯 4개를 할당")]
    public List<QuickSlot_Item> slots;

   // public InventoryManager inventoryManager;

    void OnEnable()
    {
        if (PlayerManager.Instance?.Player != null)
            PlayerManager.Instance.Player.addItem += OnItemAdded;
    }
    void OnDisable()
    {
        if (PlayerManager.Instance?.Player != null)
            PlayerManager.Instance.Player.addItem -= OnItemAdded;
    }

    // 아이템 획득 시
    private void OnItemAdded(ItemData data)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty
                && slot.itemData == data
                && data.canStack)
            {
                slot.AddStack(1);
                return;
            }
        }

        // 2) 아니면 빈 슬롯을 찾아 새로 추가
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(data);
                return;
            }
        }
    }

    void Update()
    {
        // 1~4번 키 입력 처리
        for (int i = 0; i < slots.Count; i++)
        {
            if (Keyboard.current[(Key)((int)Key.Digit1 + i)].wasPressedThisFrame)
                slots[i].Use();
        }
    }
}


