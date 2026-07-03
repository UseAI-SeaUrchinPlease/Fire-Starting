using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject avatar;
    public float moveSpeed;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.15f;
    public float mouseSensitivity = 2f;

    const float MAX_LOOK_ANGLE = 89f;

    private CharacterController characterController;
    private float currentXRotation = 0f;
    private float currentYRotation = 0f;
    private float verticalVelocity = 0f;
    private bool isGrounded = false;

    void movePlayerByKeyboardInput()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            moveDirection += playerCamera.transform.forward;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            moveDirection -= playerCamera.transform.forward;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            moveDirection += playerCamera.transform.right;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            moveDirection -= playerCamera.transform.right;
        }

        // Y軸（上下）を無視して、カメラ向き基準で水平移動
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized;

        isGrounded = characterController.isGrounded || IsGroundedByRaycast();

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 moveVelocity = moveDirection * moveSpeed;
        moveVelocity.y = verticalVelocity;
        characterController.Move(moveVelocity * Time.deltaTime);
    }

    bool IsGroundedByRaycast()
    {
        Vector3 rayOrigin = transform.position + characterController.center;
        float rayDistance = (characterController.height * 0.5f) - characterController.radius + groundCheckDistance;
        return Physics.Raycast(rayOrigin, Vector3.down, rayDistance);
    }

    void rotateByMouse()
    {
        // アプリケーションがフォーカスを持っていないときは視点操作を受け付けない
        if (!Application.isFocused)
        {
            return;
        }

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float rotationX = mouseDelta.y * mouseSensitivity * Time.deltaTime;
        float rotationY = mouseDelta.x * mouseSensitivity * Time.deltaTime;

        // X軸回転（上下視点）を制限
        currentXRotation -= rotationX;
        currentXRotation = Mathf.Clamp(currentXRotation, -MAX_LOOK_ANGLE, MAX_LOOK_ANGLE);

        // Y軸回転（左右視点）を更新
        currentYRotation += rotationY;

        // 回転を適用
        playerCamera.transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);

        // アバターの向き（ヨ方向）のみ追従
        if (avatar != null)
        {
            Vector3 avatarEuler = avatar.transform.localEulerAngles;
            avatarEuler.y = currentYRotation;
            avatar.transform.localEulerAngles = avatarEuler;
        }

        // マウスカーソルを非表示に
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // カメラの初期向きを保存
        currentXRotation = playerCamera.transform.localRotation.eulerAngles.x;
        currentYRotation = playerCamera.transform.localRotation.eulerAngles.y;

        // currentXRotationが180以上の場合は負の値に変換（-180～0の範囲）
        if (currentXRotation > 180)
        {
            currentXRotation -= 360;
        }
    }

    // Update is called once per frame
    void Update()
    {
        movePlayerByKeyboardInput();
        rotateByMouse();
    }
}
