using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Normal,
    Jumping,
    Climbing
}

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

    [Header("상태")]
    public PlayerState state = PlayerState.Normal;

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
    [SerializeField] private float climbingSpeed;
    [SerializeField] private LayerMask climbable;

    [Header("장비 인벤토리 UI")]
    public EInventoryUI equipUI;        
    private bool isEquipUIOpen = false;

    private bool isLaunched = false;

    // PlayerInput 컴포넌트 참조
    private PlayerInputHandler inputHandler;

    public Transform bodyBottom;

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
        // 클라이밍 우선 처리
        if (state == PlayerState.Climbing)
        {
            ClimbMove(inputHandler.MoveInput);
            return;
        }
        // 지면 감지: 바닥이면 Normal, 아니면 Jumping
        if (IsGrounded())
        {
            state = PlayerState.Normal;
            Move(inputHandler.MoveInput);
        }
        else
        {
            state = PlayerState.Jumping;
            // 공중에서는 이동 입력 무시
        }
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
        if (state != PlayerState.Normal) return;
        if (IsGrounded() && PlayerManager.Instance.player.condition.stamina.curValue > 10 )
        {
            PlayerManager.Instance.player.condition.UseStamina(10);

            state = PlayerState.Jumping;
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
        Vector3 dir = new Vector3(input.x, input.y, 0f) * climbingSpeed;
        rb.velocity = dir;

        // 벽 꼭대기 및 바닥 감지
        Vector3 bottom = bodyBottom.transform.position + Vector3.down * 1f;
        
        bool hitBelow = Physics.Raycast(bottom, transform.forward, 1f, climbable);
        Debug.DrawRay(bottom, transform.forward, Color.yellow);
        
        if (!hitBelow)
        {
            ExitClimbMode();
        }

    }
    public void EnterClimbMode()
    {
        state = PlayerState.Climbing;
        rb.useGravity = false;
    }

    public void ExitClimbMode()
    {
        rb.useGravity = true;
        // ledge 위로 올라서기 위한 스냅
        rb.AddForce(transform.forward * 3f, ForceMode.VelocityChange);
        // 항상 Normal 상태로 전환 (점프 방지)
        state = PlayerState.Normal;
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
        Vector3 origin = bodyBottom != null ? bodyBottom.position : (transform.position + Vector3.up * 0.1f);
        bool r = Physics.Raycast(origin, Vector3.down, 1.1f, groundLayerMask);
        Debug.DrawRay(origin, Vector3.down, Color.yellow);
        return r;
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
        if (PlayerState.Climbing == state)
            ExitClimbMode();
        else
            EnterClimbMode();
    }

    public void BeginBallistic()
    {
        state = PlayerState.Jumping;

    }
    public void EndBallistic()
    {
        state = PlayerState.Normal;
    }
}
