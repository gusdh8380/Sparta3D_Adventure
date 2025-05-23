using System;
using System.Collections.Generic;
using UnityEngine;


//�÷��̾� ������ ���� ����

public class PlayerEquipment : MonoBehaviour
{
    public Transform headMount;
    public Transform handMount;

    // ���� Ÿ�Ժ� ���� ������
    
    public Dictionary<EquipSlotType, ItemData> equipped =
        new Dictionary<EquipSlotType, ItemData>();

    // ����Ʈ�� ���־�
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
            return false;  // �̹� �� ����

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
        // ��� ���� �ջ�
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
            rb.isKinematic = true;   // ���� �ùķ��̼� ��Ȱ��
            rb.detectCollisions = false;  // �浹 ���� ����
        }

        var col = go.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;          // �ݶ��̴� ����
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
