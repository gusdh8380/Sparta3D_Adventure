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

    private float baseSpeed;
    private float baseJump;

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

    [Header("장비 인벤토리 UI")]
    public EInventoryUI equipUI;        
    private bool isEquipUIOpen = false;

 

    // PlayerInput 컴포넌트 참조
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        baseSpeed = movSpeed;
        baseJump = JumpPower;
    }



    private void FixedUpdate()
    {
        if (isClimbing)
            ClimbMove(inputHandler.MoveInput);
        else
            Move(inputHandler.MoveInput);
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook(inputHandler.LookInput);
    }

    // Look  처리
    private void OnLook(Vector2 input)
    {
        mouseDelta = input;
    }

    // Jump 처리
    public void TryJump()
    {
        if (IsGrounded() && PlayerManager.Instance.player.condition.stamina.curValue > 10 )
        {
            PlayerManager.Instance.player.condition.UseStamina(10);
            rb.AddForce(Vector2.up * JumpPower, ForceMode.Impulse);
        }
    }

    // Inventory 처리
    public void ToggleInventory()
    {
       isEquipUIOpen = !isEquipUIOpen;
        equipUI.Toggle(isEquipUIOpen);

        // 마우스 회전 제어
        canLook = !isEquipUIOpen;

        // 커서 보이기/숨기기
        Cursor.visible = isEquipUIOpen;
        Cursor.lockState = isEquipUIOpen
                              ? CursorLockMode.None
                              : CursorLockMode.Locked;
    }
    public void Zoom(float scrollY)
    {
        float newFOV = playerCamera.fieldOfView - scrollY * zoomSensitivity;
        playerCamera.fieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
    }
 
    private void Move(Vector2 input)
    {
        Vector3 dir = (transform.forward * input.y + transform.right * input.x) * movSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }
    private void ClimbMove(Vector2 input)
    {
        Vector3 dir = new Vector3(input.x, input.y, 0);
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

    private void CameraLook(Vector2 lookInput)
    {
        camCurXRot += lookInput.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += Vector3.up * (lookInput.x * lookSensitivity);
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
    public void SwitchView()
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

    //public void ApplyEquipEffect(ItemData data)
    //{
    //    movSpeed = baseSpeed + data.addSpeed;
    //    JumpPower = baseJump + data.addJumpPower;
    //}
    //public void ClearEquipEffect()
    //{
    //    movSpeed = baseSpeed;
    //    JumpPower = baseJump;
    //}
}
