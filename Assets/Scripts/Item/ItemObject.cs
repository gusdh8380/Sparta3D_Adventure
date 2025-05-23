using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//월드에 배치된 아이템 프리팹에 부착
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
        var player = PlayerManager.Instance.Player;
        player.AddItem(ItemData);
        Destroy(gameObject);
    }

}
