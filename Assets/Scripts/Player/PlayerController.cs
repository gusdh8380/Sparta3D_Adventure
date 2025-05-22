using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    public float movSpeed;
    public float JumpPower;
    private Vector2 curMovementInput;
    private Rigidbody rb;
    public LayerMask groundLayerMask;

    [Header("방향")]
    public Transform cameraContainer;
    [SerializeField] private Camera playerCamera;
    public float minXLook;
    public float maxXLook;
    public float minFOV = 30f;
    public float maxFOV = 90f;
    private float camCurXRot;
    public float lookSensitivity;
    public float zoomSensitivity;
    private Vector2 mouseDelta;
    private bool canLook = true;

    [Header("카메라 전환")]
    public bool isFirstPerson = false;
    // 시점 위치
    [SerializeField] private Vector3 thirdPersonOffset = new Vector3(0, 2, -4);
    [SerializeField] private Vector3 firstPersonOffset = new Vector3(0, 1.6f, 0.2f);


    [Header("벽타기")]
    public bool isClimbing = false;
    [SerializeField] private float climbingSpeed;
    [SerializeField] private LayerMask climbable;

    public event Action onInventoryToggle;

    // PlayerInput 컴포넌트 참조
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        // Move 액션
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        // Look 액션
        playerInput.actions["Look"].performed += OnLook;
        // Jump 액션
        playerInput.actions["Jump"].started += OnJump;
        // Inventory 액션
        playerInput.actions["Inventory"].started += OnInventory;
        playerInput.actions["Zoom"].performed += OnZoom;
        //시점 전환 액션
        playerInput.actions["ViewToggle"].started += OnViewToggle;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
        playerInput.actions["Look"].performed -= OnLook;
        playerInput.actions["Jump"].started -= OnJump;
        playerInput.actions["Inventory"].started -= OnInventory;
        playerInput.actions["Zoom"].performed -= OnZoom;
    }

    private void FixedUpdate()
    {
        if (isClimbing)
            ClimbMove();
        else
            Move();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    // Move 입력 처리
    private void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
            curMovementInput = ctx.ReadValue<Vector2>();
        else if (ctx.phase == InputActionPhase.Canceled)
            curMovementInput = Vector2.zero;
    }

    // Look 입력 처리
    private void OnLook(InputAction.CallbackContext ctx)
    {
        mouseDelta = ctx.ReadValue<Vector2>();
    }

    // Jump 입력 처리
    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (IsGrounded() && PlayerManager.Instance.player.condition.stamina.curValue > 10 )
        {
            PlayerManager.Instance.player.condition.UseStamina(10);
            rb.AddForce(Vector2.up * JumpPower, ForceMode.Impulse);
        }
    }

    // Inventory 입력 처리
    private void OnInventory(InputAction.CallbackContext ctx)
    {
        onInventoryToggle?.Invoke();
        ToggleCursor();
    }
    private void OnZoom(InputAction.CallbackContext ctx)
    {
        // Vector2일 때
        float scrollY = ctx.ReadValue<Vector2>().y;

        float newFOV = playerCamera.fieldOfView - scrollY * zoomSensitivity;
        playerCamera.fieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
    }
    private void OnViewToggle(InputAction.CallbackContext ctx)
    {
        SwitchView();
    }

    private void Move()
    {
        Vector3 dir = (transform.forward * curMovementInput.y + transform.right * curMovementInput.x) * movSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }
    private void ClimbMove()
    {
        Vector3 dir = new Vector3(curMovementInput.x, curMovementInput.y, 0);
        rb.velocity = dir * climbingSpeed;

        // 바닥 근처에서 벽 감지 안 되면 자동 해제
        Vector3 bottom = transform.position + Vector3.down * 0.9f;
        if (!Physics.Raycast(bottom, transform.forward, 1f, climbable))
        {
            ExitClimbMode();
        }
    }
    public void EnterClimbMode()
    {
        isClimbing = true;
        rb.useGravity = false;
    }

    public void ExitClimbMode()
    {
        isClimbing = false;
        rb.useGravity = true;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += Vector3.up * (mouseDelta.x * lookSensitivity);
        mouseDelta = Vector2.zero;
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + transform.forward * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position - transform.forward * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + transform.right   * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position - transform.right   * 0.2f + transform.up * 0.01f, Vector3.down)
        };

        foreach (var ray in rays)
            if (Physics.Raycast(ray, 1.5f, groundLayerMask))
                return true;

        return false;
    }

    private void ToggleCursor()
    {
        bool unlocked = (Cursor.lockState == CursorLockMode.None);
        Cursor.lockState = unlocked ? CursorLockMode.Locked : CursorLockMode.None;
        canLook = unlocked;
    }

    public void ApplySpeedUp(float a,float d)
    {
        StartCoroutine(SpeedUpCoroutine(a,d));
    }
    private IEnumerator SpeedUpCoroutine(float a,float d)
    {
        movSpeed += a;
        yield return  new WaitForSeconds(d);
        movSpeed -= a;
    }
    private void SwitchView()
    {
        isFirstPerson = !isFirstPerson;

        Vector3 targetOffset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
        playerCamera.transform.localPosition = targetOffset;
    }
    public void ToggleClimbMode()
    {
        if (isClimbing)
            ExitClimbMode();
        else
            EnterClimbMode();
    }
}
