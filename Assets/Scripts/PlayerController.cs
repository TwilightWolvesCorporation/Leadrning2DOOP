using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly int State = Animator.StringToHash("State");
    private Rigidbody2D _rb;
    private Animator _animator;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeedModify;
    [SerializeField] private float jumpForce;

    [Header("Controls")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference sprint;
    [SerializeField] private bool isGrounded;

    [SerializeField] private bool isPause;

    private float _velocityX = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var isRun = sprint.action.ReadValue<float>() != 0;
        _velocityX = move.action.ReadValue<float>() * (isRun ? sprintSpeedModify : 1);

        if (!isGrounded) _animator.SetInteger(State, 3);
        else if (_velocityX == 0) _animator.SetInteger(State, 0);
        else if (_velocityX != 0 && !isRun) _animator.SetInteger(State, 1);
        else if (_velocityX != 0 && isRun) _animator.SetInteger(State, 2);
    }

    private void FixedUpdate()
    {
        if (!isPause)
        {
            FlipPlayer();
            _rb.linearVelocity = new Vector2(_velocityX * speed, _rb.linearVelocity.y);
        }
    }

    private void FlipPlayer()
    {
        transform.localRotation = _velocityX switch
        {
            > 0 => Quaternion.Euler(0f, 0f, 0f),
            < 0 => Quaternion.Euler(0f, 180f, 0f),
            _ => transform.localRotation
        };
    }

    public void PlayerIsGrounded(bool isGroundedCheck)
    {
        isGrounded = isGroundedCheck;
        if (isGroundedCheck) jump.action.performed += OnJumpPerformed;
        else jump.action.performed -= OnJumpPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}