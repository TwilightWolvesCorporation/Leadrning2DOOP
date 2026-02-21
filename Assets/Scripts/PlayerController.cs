using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly int State = Animator.StringToHash("State");
    private Rigidbody2D _rb;
    private Animator _animator;
    private Camera _camera;
    private InputActionMap _inputActionMap;

    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeedModify;
    [SerializeField] private float jumpForce;

    [SerializeField] private bool isGrounded;

    [SerializeField] private GameObject box;

    [SerializeField] private TMP_Text hpText;

    [SerializeField] private bool isPause;

    [SerializeField] private AudioSource audioSource;

    private float _velocityX = 0;

    private const float TimeSpawn = 5;
    private bool _isCanSpawn = true;

    private float _moveInput;
    private bool _isRun;
    private bool _boxDragged;
    private DraggableBox _draggableBox;

    private int _hp;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        _inputActionMap = GetComponent<PlayerInput>().currentActionMap;
    }

    private void Update()
    {
        _velocityX = _moveInput * (_isRun ? sprintSpeedModify : 1);

        if (!isGrounded) _animator.SetInteger(State, 3);
        else if (_velocityX == 0) _animator.SetInteger(State, 0);
        else if (_velocityX != 0 && !_isRun) _animator.SetInteger(State, 1);
        else if (_velocityX != 0 && _isRun) _animator.SetInteger(State, 2);
    }

    private void FixedUpdate()
    {
        if (!isPause)
        {
            FlipPlayer();
            _rb.linearVelocity = new Vector2(_velocityX * speed, _rb.linearVelocity.y);
        }
    }

    public void SetHp(int hp)
    {
        _hp = hp;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed || ctx.canceled)
        {
            _moveInput = ctx.ReadValue<float>();
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !isGrounded) return;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _isRun = true;
        }
        else if (ctx.canceled)
        {
            _isRun = false;
        }
    }

    public void OnDragBox(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_boxDragged)
        {
            var ray = _camera.ScreenPointToRay(_inputActionMap["MousePosition"].ReadValue<Vector2>());
            var hit = Physics2D.GetRayIntersection(ray, 100f);
            if (!hit) return;
            if (hit.collider.gameObject.name != "DraggableBox") return;
            _draggableBox = hit.collider.gameObject.GetComponent<DraggableBox>();
            _boxDragged = true;
            _draggableBox.Dragging(true);
        }
        else if (_draggableBox && ctx.canceled && _boxDragged)
        {
            _boxDragged = false;
            _draggableBox.Dragging(false);
            _draggableBox = null;
        }
    }

    public void OnDeleteBox(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        var ray = _camera.ScreenPointToRay(_inputActionMap["MousePosition"].ReadValue<Vector2>());
        var hit = Physics2D.GetRayIntersection(ray, 100f);
        if (!hit) return;
        if (hit.collider.gameObject.name == "DraggableBox")
        {
            Destroy(hit.collider.gameObject);
        }
    }

    public void OnSpawnBox(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        SpawnBox();
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
    }

    private void SpawnBox()
    {
        if (!_isCanSpawn)
        {
            Debug.Log("I not can spawns");
            return;
        }

        audioSource.Play();

        _isCanSpawn = false;
        Instantiate(box, _rb.position + new Vector2(transform.localRotation.y == 0 ? 1 : -1, 0),
            box.GetComponent<BoxCollider2D>().transform.rotation).name = "DraggableBox";
        StartCoroutine(SpawnBoxTime());
    }

    private IEnumerator SpawnBoxTime()
    {
        _isCanSpawn = false;
        yield return new WaitForSeconds(TimeSpawn);
        _isCanSpawn = true;
    }
}