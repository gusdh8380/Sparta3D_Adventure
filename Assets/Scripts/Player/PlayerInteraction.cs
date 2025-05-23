using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerInteraction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;
    public LayerMask layerMask1;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera cam;

    private PlayerInputHandler inputHandler;

    
   
    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>(); 
        cam = Camera.main;
    }

    void Update()
    {
            //아이템 상호작용 레이캐스트
            ItemInteractRay();

            //벽타기 상호작용 레이캐스트
            //ClimbingIntercatRay();
    }
    private void ItemInteractRay()
    {
        PlayerController c = PlayerManager.Instance.Player.controller;
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Vector3 originBottom = transform.position + Vector3.down * 0.5f;

            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            Ray ray1 = new Ray(originBottom,transform.forward);

            RaycastHit hit;
            RaycastHit hit1;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else if (Physics.Raycast(ray1, out hit1, 0.8f, layerMask1) && !c.isClimbing)
            {
                ClimbingPlatform climbTarget = hit1.collider.GetComponent<ClimbingPlatform>();
                if (climbTarget != null)
                {
                    curInteractGameObject = hit1.collider.gameObject;
                    curInteractable = hit1.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void TryInteract()
    {
        PlayerController c = PlayerManager.Instance.Player.controller;
        if (curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
        if (curInteractable != null && c.isClimbing)
        {
            c.ToggleClimbMode();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
            return;
        }
    }


}
