using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, PlayerAction.IPlayerActions
{
    private PlayerAction inputActions;
    private PlayerController playerController;
    private PlayerInteraction playerInteraction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    private void Awake()
    {
        inputActions = new PlayerAction();
        inputActions.Player.SetCallbacks(this);
        playerController = GetComponent<PlayerController>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }
    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();
    void PlayerAction.IPlayerActions.OnInteraction(InputAction.CallbackContext context)
    {
        if (context.started)
            playerInteraction?.TryInteract();
    }

    void PlayerAction.IPlayerActions.OnInventory(InputAction.CallbackContext context)
    {
        if (context.started)
            playerController?.ToggleInventory();
    }

    void PlayerAction.IPlayerActions.OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            playerController?.TryJump();
    }

    void PlayerAction.IPlayerActions.OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    void PlayerAction.IPlayerActions.OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    void PlayerAction.IPlayerActions.OnViewToggle(InputAction.CallbackContext context)
    {
        if (context.started)
            playerController?.SwitchView();
    }

    void PlayerAction.IPlayerActions.OnZoom(InputAction.CallbackContext context)
    {
        Vector2 zoomDelta = context.ReadValue<Vector2>();
        playerController?.Zoom(zoomDelta.y);
    }
}
