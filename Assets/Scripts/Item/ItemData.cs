using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}
public enum EquipSlotType
{
    Head,
    Hand
}
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public float coolTime;
    public Sprite icon;
    public GameObject dropPrefab;
   
    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    // �ν����Ϳ��� �巡�� �� ����� �� �ֵ��� ScriptableObject ����Ʈ�� ����
    public List<ScriptableObject> actionAssets;
    // ��Ÿ�ӿ��� IItemAction ����Ʈ�� ĳ����
    public IEnumerable<IItemAction> GetActions() =>
        actionAssets.OfType<IItemAction>();

    [Header("Equip")]
    public GameObject equipPrefab;
    public float addSpeed;
    public float addJumpPower;
    public EquipSlotType equipSlotType;



}
