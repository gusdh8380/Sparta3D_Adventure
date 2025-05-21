using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [Header("UI 슬롯 4개를 할당")]
    public List<ItemSlot> slots;

    void Start()
    {
        // 플레이어가 아이템을 획득했을 때 호출
        PlayerManager.Instance.Player.addItem += OnItemAdded;
    }

    void OnDestroy()
    {
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


