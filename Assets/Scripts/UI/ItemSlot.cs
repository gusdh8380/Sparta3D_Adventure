using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("UI 컴포넌트")]
    public Image iconImage;
    public TextMeshProUGUI countText;
    public Image cooldownOverlay;

    // 내부 보관용

    public ItemData itemData;
    public int stackCount;
    private bool isOnCooldown;
    public bool IsEmpty => itemData == null;

    // 슬롯에 아이템 추가
    public void AddItem(ItemData data)
    {
        if (isOnCooldown)
        {
            itemData = data;
            stackCount += 1;     // 이미 Clear 후 stackCount는 0이므로 +1
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

        // overlay, 쿨다운 플래그 초기화
        cooldownOverlay.enabled = false;
        isOnCooldown = false;
    }
    public void AddStack(int a)
    {
        stackCount += a;
        countText.text = stackCount.ToString();

        cooldownOverlay.enabled = false;
       
    }

    // 슬롯 사용 시
    public void Use()
    {
        if (isOnCooldown == false && itemData != null)
        {
            if (itemData == null) return;
            foreach (var action in itemData.GetActions())
                action.Execute(PlayerManager.Instance.Player);
           
            StartCoroutine(CooldownCoroutine(itemData.coolTime));

            // 스택 처리
            stackCount--;
            if (stackCount <= 0) Clear();
            else countText.text = stackCount.ToString();
          

        }
        else
        {
            Debug.Log("쿨타임 중");
        }
    }

    private IEnumerator CooldownCoroutine(float cooldown)
    {
        isOnCooldown = true;
        cooldownOverlay.enabled = true;

        // Overlay 세팅: FillMethod = Vertical, Origin = Bottom
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

        // 쿨다운 끝
        cooldownOverlay.enabled = false;
        isOnCooldown = false;

        if(stackCount <=0)
            Clear();    
    }

    // 슬롯 초기화
    private void Clear()
    {
        itemData = null;
        iconImage.enabled = false;
        countText.enabled = false;
    }

}
