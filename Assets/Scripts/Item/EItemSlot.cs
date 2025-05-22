using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outline))]
public class EItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Icon Image")]
    public Image iconImage;

    [Header("Selection Outline")]
    public Outline outline;


    public ItemData itemData;

    public bool IsEmpty => itemData == null;
    public bool HasItem => !IsEmpty;

    private void Awake()
    {
        // Ensure outline component
        outline = outline != null ? outline : GetComponent<Outline>();
        outline.enabled = false;

        // Enable raycast on icon so clicks register
        if (iconImage != null)
            iconImage.raycastTarget = true;
    }

    public void AddItem(ItemData data)
    {
        itemData = data;
        iconImage.sprite = data.icon;
        iconImage.enabled = true;
        iconImage.raycastTarget = true;
    }

    public void Clear()
    {
        itemData = null;
        iconImage.enabled = false;
        iconImage.raycastTarget = false;
        outline.enabled = false;
    }

    // IPointerClickHandler implementation
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData == null) return;

        // Highlight selected slot
        outline.enabled = true;

        // Notify the inventory UI
        var ui = FindObjectOfType<EInventoryUI>();
        if (ui != null)
            ui.SelectSlot(this);
    }
}
