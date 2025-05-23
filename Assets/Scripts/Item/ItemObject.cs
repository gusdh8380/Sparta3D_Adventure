using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���忡 ��ġ�� ������ �����տ� ����
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
