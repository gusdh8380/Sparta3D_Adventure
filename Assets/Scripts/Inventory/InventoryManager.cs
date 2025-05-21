using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [Header("UI ���� 4���� �Ҵ�")]
    public List<ItemSlot> slots;

    void Start()
    {
        // �÷��̾ �������� ȹ������ �� ȣ��
        PlayerManager.Instance.Player.addItem += OnItemAdded;
    }

    void OnDestroy()
    {
        PlayerManager.Instance.Player.addItem -= OnItemAdded;
    }

    // ������ ȹ�� ��
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

        // 2) �ƴϸ� �� ������ ã�� ���� �߰�
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
        // 1~4�� Ű �Է� ó��
        for (int i = 0; i < slots.Count; i++)
        {
            if (Keyboard.current[(Key)((int)Key.Digit1 + i)].wasPressedThisFrame)
                slots[i].Use();
        }
    }
}


