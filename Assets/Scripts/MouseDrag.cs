using UnityEngine;
using UnityEngine.InputSystem;

public class MouseDrag : MonoBehaviour
{
    private Camera _cam;
    private Rigidbody2D _rb;
    private InputActionReference _drag;
    private InputActionReference _delete;
    [SerializeField] private float maxDragSpeed = 15f;
    [SerializeField] private bool freezeRotation = true;

    private bool _isDragging;
    private Vector2 _dragOffset;
    private Vector2 _lastVelocity;
    
    private RigidbodyConstraints2D _savedConstraint;
    
    private void OnMouseDownC(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (_isDragging) return;
        var col = Physics2D.OverlapPoint(GetMousePosition());
        if (col == null || col.gameObject != gameObject) return;
        _drag.action.performed -= OnMouseDownC;
        _drag.action.canceled += OnMouseUpC;
        StartDragging();
    }

    private void DeleteDownC(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        var col = Physics2D.OverlapPoint(GetMousePosition());
        if (col == null || col.gameObject != gameObject) return;
        Destroy(gameObject);
    }

    private void OnMouseUpC(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled) return;
        if (!_isDragging) return;
        _drag.action.canceled -= OnMouseUpC;
        _drag.action.performed += OnMouseDownC;
        StopDragging();
    }

    private void StartDragging()
    {
        _isDragging = true;

        _savedConstraint = _rb.constraints;
        if (freezeRotation) _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _dragOffset = _rb.position - GetMousePosition();
        _lastVelocity = Vector2.zero;
    }

    private void StopDragging()
    {
        _isDragging = false;
        _rb.constraints = _savedConstraint;
        _rb.linearVelocity = _lastVelocity;
    }

    private void FixedUpdate()
    {
        if (!_isDragging) return;
        var target = GetMousePosition() + _dragOffset;
        var newVelocity = (target - _rb.position) / Time.fixedDeltaTime;
        if (newVelocity.magnitude > maxDragSpeed) newVelocity = newVelocity.normalized * maxDragSpeed;
        _lastVelocity = newVelocity;
        _rb.MovePosition(target);
    }

    private Vector2 GetMousePosition()
    {
        return _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void SetBinds(InputActionReference dragBox, InputActionReference deleteBox)
    {
        _drag = dragBox;
        _delete = deleteBox;
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _drag.action.performed += OnMouseDownC;
        _delete.action.performed += DeleteDownC;
    }

    private void OnDestroy()
    {
        _drag.action.performed -= OnMouseDownC;
        _drag.action.canceled -= OnMouseUpC;
        _delete.action.performed -= DeleteDownC;
    }
}