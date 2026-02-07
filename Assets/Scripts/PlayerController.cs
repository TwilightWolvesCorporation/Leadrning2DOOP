using System.Collections;
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

    [SerializeField] private InputActionReference dragBox;
    [SerializeField] private InputActionReference spawnBox;
    [SerializeField] private InputActionReference deleteBox;
    [SerializeField] private GameObject box;


    [SerializeField] private bool isPause;

    private float _velocityX = 0;

    private const float TimeSpawn = 5;
    private bool _isCanSpawn = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        spawnBox.action.performed += _ => SpawnBox();
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

    private void SpawnBox()
    {
        if (!_isCanSpawn)
        {
            Debug.Log("I not can spawns");
            return;
        }
        _isCanSpawn = false;
        var newBox = Instantiate(box, _rb.position + new Vector2(transform.localRotation.y == 0 ? 1 : -1, 0),
            box.GetComponent<BoxCollider2D>().transform.rotation);
        newBox.GetComponent<MouseDrag>().SetBinds(dragBox, deleteBox);
        StartCoroutine(SpawnBoxTime());
    }

    private IEnumerator SpawnBoxTime()
    {
        _isCanSpawn = false;
        yield return new WaitForSeconds(TimeSpawn);
        _isCanSpawn = true;
    }
}