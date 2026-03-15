using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 18f;
    public float maxFallSpeed = 24f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.35f, 0.08f);

    public bool IsGrounded => isGrounded;
    public bool IsMoving => Mathf.Abs(moveInput) > 0.01f;
    public bool FacingRight => facingRight;

    private Rigidbody2D rb;
    private float moveInput;
    private float actionMoveInput;
    private bool jumpQueued;
    private bool isGrounded;
    private bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputAction.CallbackContext context)
    {
        actionMoveInput = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpQueued = true;
        }
    }
#endif

    void Update()
    {
        moveInput = ReadMoveInput();

        if (ReadJumpPressed())
        {
            jumpQueued = true;
        }

        if (moveInput > 0.01f)
        {
            facingRight = true;
        }
        else if (moveInput < -0.01f)
        {
            facingRight = false;
        }
    }

    void FixedUpdate()
    {
        UpdateGroundedState();

        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;
        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);

        if (jumpQueued && isGrounded)
        {
            velocity.y = jumpForce;
            isGrounded = false;
        }

        rb.linearVelocity = velocity;
        jumpQueued = false;
    }

    private float ReadMoveInput()
    {
        float input = Mathf.Abs(actionMoveInput) > 0.01f ? actionMoveInput : 0f;

#if ENABLE_INPUT_SYSTEM
        if (Mathf.Abs(input) < 0.01f && Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            {
                input -= 1f;
            }

            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            {
                input += 1f;
            }
        }
#else
        if (Mathf.Abs(input) < 0.01f)
        {
            input = Input.GetAxisRaw("Horizontal");
        }
#endif

        return Mathf.Clamp(input, -1f, 1f);
    }

    private bool ReadJumpPressed()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null)
        {
            return false;
        }

        return Keyboard.current.spaceKey.wasPressedThisFrame
            || Keyboard.current.wKey.wasPressedThisFrame
            || Keyboard.current.upArrowKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.W)
            || Input.GetKeyDown(KeyCode.UpArrow);
#endif
    }

    private void UpdateGroundedState()
    {
        Vector2 checkCenter;

        if (groundCheck != null)
        {
            checkCenter = groundCheck.position;
        }
        else if (TryGetComponent(out Collider2D collider2D))
        {
            Bounds bounds = collider2D.bounds;
            checkCenter = new Vector2(bounds.center.x, bounds.min.y - (groundCheckSize.y * 0.5f));
        }
        else
        {
            checkCenter = (Vector2)transform.position + Vector2.down * 0.5f;
        }

        int mask = groundLayer.value == 0 ? Physics2D.AllLayers : groundLayer.value;
        isGrounded = Physics2D.OverlapBox(checkCenter, groundCheckSize, 0f, mask);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 checkCenter;
        if (groundCheck != null)
        {
            checkCenter = groundCheck.position;
        }
        else
        {
            checkCenter = transform.position + Vector3.down * 0.5f;
        }

        Gizmos.DrawWireCube(checkCenter, groundCheckSize);
    }
}
