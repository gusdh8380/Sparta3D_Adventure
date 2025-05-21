using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData ItemData;

    public string GetInteractPrompt()
    {
        string str = $"{ItemData.displayName}\n{ItemData.description}";
        return str;
    }

    public void OnInteract()
    {
        PlayerManager.Instance.Player.itemData = ItemData;
        PlayerManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }

}
