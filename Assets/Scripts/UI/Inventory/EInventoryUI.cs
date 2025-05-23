using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//장착 가능한 아이템(장비)의 인벤토리UI 관리
public class EInventoryUI : MonoBehaviour
{
    [Header("Root Panel")]
    public GameObject rootPanel;

    [Header("인벤 슬롯")]
    public List<EItemSlot> inventorySlots;

    [Header("장착 중 슬롯 2개")]
    public List<EItemSlot> equippedSlots;

    [Header("버튼")]
    public Button equipButton;
    public Button unequipButton;

    [Header("InfoText")]
    public TextMeshProUGUI EitemName;
    public TextMeshProUGUI EitemDescription;

    private EItemSlot selectedSlot;
    private EItemSlot selectedEquippedSlot;

    private void Awake()
    {
        rootPanel.SetActive(false);
    }

    // Start에서 구독 → Awake/OnEnable 타이밍 이슈 회피
    private void Start()
    {
        // Player가 세팅된 이후에만 구독
        var player = PlayerManager.Instance?.Player;
        if (player != null)
            player.addEquipItem += OnEquipItemAdded;

        equipButton.onClick.AddListener(OnEquipPressed);
        unequipButton.onClick.AddListener(OnUnequipPressed);
    }

    private void OnDestroy()
    {
        // 구독 해제
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
        if (selectedSlot != null && selectedSlot != slot)
        {
            selectedSlot.outline.enabled = false;
        }
        if(selectedEquippedSlot != null && selectedEquippedSlot != slot)
        {
            selectedEquippedSlot.outline.enabled = false;
        }

        if (inventorySlots.Contains(slot))
        {
            selectedSlot = slot;
            selectedEquippedSlot = null;
            EitemName.text = slot.itemData.name;
            EitemDescription.text  = slot.itemData.description;
            equipButton.gameObject.SetActive(slot.HasItem && equippedSlots.Exists(s => s.IsEmpty));
            unequipButton.gameObject.SetActive(false);
        }
        else if(equippedSlots.Contains(slot))
        {
            selectedEquippedSlot = slot;
            selectedSlot = null;
            EitemName.text = slot.itemData.name;
            EitemDescription.text = slot.itemData.description;
            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(slot.HasItem);
        }
        else
        {
            selectedEquippedSlot = slot;
            selectedSlot = null;
            EitemName.text = null;
            EitemDescription.text = null;
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
