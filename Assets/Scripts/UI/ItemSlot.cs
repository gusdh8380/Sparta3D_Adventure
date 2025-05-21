using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("UI ������Ʈ")]
    public Image iconImage;
    public TextMeshProUGUI countText;
    public Image cooldownOverlay;

    // ���� ������

    public ItemData itemData;
    public int stackCount;
    private bool isOnCooldown;
    public bool IsEmpty => itemData == null;

    // ���Կ� ������ �߰�
    public void AddItem(ItemData data)
    {
        if (isOnCooldown)
        {
            itemData = data;
            stackCount += 1;     // �̹� Clear �� stackCount�� 0�̹Ƿ� +1
            iconImage.enabled = true;
            countText.enabled = true;
            countText.text = stackCount.ToString();
            return;
        }

        itemData = data;
        stackCount = 1;
        iconImage.sprite = data.icon;
        iconImage.enabled = true;
        countText.enabled = true;
        countText.text = stackCount.ToString();

        // overlay, ��ٿ� �÷��� �ʱ�ȭ
        cooldownOverlay.enabled = false;
        isOnCooldown = false;
    }
    public void AddStack(int a)
    {
        stackCount += a;
        countText.text = stackCount.ToString();

        cooldownOverlay.enabled = false;
       
    }

    // ���� ��� ��
    public void Use()
    {
        if (isOnCooldown == false && itemData != null)
        {
            if (itemData == null) return;
            foreach (var action in itemData.GetActions())
                action.Execute(PlayerManager.Instance.Player);
           
            StartCoroutine(CooldownCoroutine(itemData.coolTime));

            // ���� ó��
            stackCount--;
            if (stackCount <= 0) Clear();
            else countText.text = stackCount.ToString();
          

        }
        else
        {
            Debug.Log("��Ÿ�� ��");
        }
    }

    private IEnumerator CooldownCoroutine(float cooldown)
    {
        isOnCooldown = true;
        cooldownOverlay.enabled = true;

        // Overlay ����: FillMethod = Vertical, Origin = Bottom
        cooldownOverlay.type = Image.Type.Filled;
        cooldownOverlay.fillMethod = Image.FillMethod.Radial360;
        cooldownOverlay.fillOrigin = (int)Image.Origin360.Bottom;

        float elapsed = 0f;
        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            cooldownOverlay.fillAmount = 1f - Mathf.Clamp01(elapsed / cooldown);
            yield return null;
        }

        // ��ٿ� ��
        cooldownOverlay.enabled = false;
        isOnCooldown = false;

        if(stackCount <=0)
            Clear();    
    }

    // ���� �ʱ�ȭ
    private void Clear()
    {
        itemData = null;
        iconImage.enabled = false;
        countText.enabled = false;
    }

}
