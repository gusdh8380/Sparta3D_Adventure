using System;
using System.Collections.Generic;
using UnityEngine;


//플레이어 아이템 장착 관리

public class PlayerEquipment : MonoBehaviour
{
    public Transform headMount;
    public Transform handMount;

    // 슬롯 타입별 장착 아이템
    
    public Dictionary<EquipSlotType, ItemData> equipped =
        new Dictionary<EquipSlotType, ItemData>();

    // 마운트된 비주얼
    private Dictionary<EquipSlotType, GameObject> visuals =
        new Dictionary<EquipSlotType, GameObject>();

    public event Action<EquipSlotType, ItemData> OnEquip;
    public event Action<EquipSlotType, ItemData> OnUnequip;

    private PlayerController controller;
    private float baseSpeed, baseJump;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        baseSpeed = controller.movSpeed;
        baseJump = controller.JumpPower;
    }

    public bool Equip(EquipSlotType slotType, ItemData data)
    {
        if (equipped.ContainsKey(slotType))
            return false;  // 이미 차 있음

        equipped[slotType] = data;
        ApplyStats();
        SpawnVisual(slotType, data);
        OnEquip?.Invoke(slotType, data);
        return true;
    }

    public bool Unequip(EquipSlotType slotType)
    {
        if (!equipped.TryGetValue(slotType, out var data))
            return false;

        equipped.Remove(slotType);
        RemoveVisual(slotType);
        ApplyStats();
        OnUnequip?.Invoke(slotType, data);
        return true;
    }

    private void ApplyStats()
    {
        // 모든 슬롯 합산
        float bonusSpeed = 0, bonusJump = 0;
        foreach (var data in equipped.Values)
        {
            bonusSpeed += data.addSpeed;
            bonusJump += data.addJumpPower;
        }
        controller.movSpeed = baseSpeed + bonusSpeed;
        controller.JumpPower = baseJump + bonusJump;
    }

    private void SpawnVisual(EquipSlotType slot, ItemData data)
    {
        Transform mount = slot == EquipSlotType.Head ? headMount : handMount;
        var prefab = data.equipPrefab;
        if (prefab == null) return;

        var go = Instantiate(prefab, mount);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        var rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;   // 물리 시뮬레이션 비활성
            rb.detectCollisions = false;  // 충돌 감지 해제
        }

        var col = go.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;          // 콜라이더 끄기
        visuals[slot] = go;
    }

    private void RemoveVisual(EquipSlotType slot)
    {
        if (visuals.TryGetValue(slot, out var go))
        {
            Destroy(go);
            visuals.Remove(slot);
        }
    }
}
