using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EInventoryUI : MonoBehaviour
{
    [Header("Root Panel")]
    public GameObject rootPanel;

    [Header("�κ� ����")]
    public List<EItemSlot> inventorySlots;

    [Header("���� �� ���� 2��")]
    public List<EItemSlot> equippedSlots;

    [Header("��ư")]
    public Button equipButton;
    public Button unequipButton;

    private EItemSlot selectedSlot;
    private EItemSlot selectedEquippedSlot;

    private void Awake()
    {
        rootPanel.SetActive(false);
    }

    // Start���� ���� �� Awake/OnEnable Ÿ�̹� �̽� ȸ��
    private void Start()
    {
        // Player�� ���õ� ���Ŀ��� ����
        var player = PlayerManager.Instance?.Player;
        if (player != null)
            player.addEquipItem += OnEquipItemAdded;

        equipButton.onClick.AddListener(OnEquipPressed);
        unequipButton.onClick.AddListener(OnUnequipPressed);
    }

    private void OnDestroy()
    {
        // ���� ����
        var player = PlayerManager.Instance?.Player;
        if (player != null)
            player.addEquipItem -= OnEquipItemAdded;

        equipButton.onClick.RemoveListener(OnEquipPressed);
        unequipButton.onClick.RemoveListener(OnUnequipPressed);
    }

    public void Toggle(bool open)
    {
        rootPanel.SetActive(open);
        selectedSlot = null;
        selectedEquippedSlot = null;
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
    }

    private void OnEquipItemAdded(ItemData data)
    {
        foreach (var slot in inventorySlots)
            if (slot.IsEmpty)
            {
                slot.AddItem(data);
                return;
            }
    }

    public void SelectSlot(EItemSlot slot)
    {
        if (inventorySlots.Contains(slot))
        {
            selectedSlot = slot;
            selectedEquippedSlot = null;
            equipButton.gameObject.SetActive(slot.HasItem && equippedSlots.Exists(s => s.IsEmpty));
            unequipButton.gameObject.SetActive(false);
        }
        else
        {
            selectedEquippedSlot = slot;
            selectedSlot = null;
            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(slot.HasItem);
        }
    }

    private void OnEquipPressed()
    {
        if (selectedSlot == null || selectedSlot.IsEmpty) return;

        var free = equippedSlots.Find(s => s.IsEmpty);
        if (free == null) return;

        var data = selectedSlot.itemData;

        free.AddItem(selectedSlot.itemData);
        PlayerManager.Instance.Player.equipment.Equip(data.equipSlotType, data);
        selectedSlot.Clear();
        equipButton.gameObject.SetActive(false);
    }

    private void OnUnequipPressed()
    {
        if (selectedEquippedSlot == null || selectedEquippedSlot.IsEmpty) return;

        EquipSlotType slotType = selectedEquippedSlot.itemData.equipSlotType;

        PlayerManager.Instance.Player.equipment.Unequip(slotType);

        foreach (var slot in inventorySlots)
            if (slot.IsEmpty)
            {
                slot.AddItem(selectedEquippedSlot.itemData);
                break;
            }

        selectedEquippedSlot.Clear();
        unequipButton.gameObject.SetActive(false);
    }
}
