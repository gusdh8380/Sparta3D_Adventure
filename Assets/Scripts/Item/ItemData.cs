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
    // 인스펙터에서 드래그 앤 드랍할 수 있도록 ScriptableObject 리스트로 선언
    public List<ScriptableObject> actionAssets;
    // 런타임에는 IItemAction 리스트로 캐스팅
    public IEnumerable<IItemAction> GetActions() =>
        actionAssets.OfType<IItemAction>();

    [Header("Equip")]
    public GameObject equipPrefab;
    public float addSpeed;
    public float addJumpPower;
    public EquipSlotType equipSlotType;



}
