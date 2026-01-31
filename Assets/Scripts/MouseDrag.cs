using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseDrag : MonoBehaviour
{
    private Camera _cam;
    private Rigidbody2D _rb;
    [SerializeField] private InputActionReference drag;
    [SerializeField] private float maxDragSpeed;
    [SerializeField] private bool freezeRotation;

    private bool _isDragging;
    private Vector2 _dragOffset;
    private Vector2 _lastVelocity;
    
    private RigidbodyConstraints2D _savedConstraint;

    private void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        drag.action.performed += _ => OnMouseDown();
    }

    private void OnMouseDown()
    {
        if (_isDragging) return;
        var col = Physics2D.OverlapPoint(GetMousePosition());
        if (col == null) return;
        drag.action.performed += _ => OnMouseUp();
        StartDragging();
    }

    private void OnMouseUp()
    {
        if(!_isDragging) return;
        drag.action.canceled += _ => OnMouseUp();
        StopDragging();
    }

    private void StartDragging()
    {
        _isDragging = true;

        _savedConstraint = _rb.constraints;
        if(freezeRotation) _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _dragOffset = _rb.position - GetMousePosition();
        _lastVelocity = Vector2.zero;
        drag.action.canceled += _ => OnMouseDown();
    }

    private void StopDragging()
    {
        _isDragging = false;
        _rb.constraints =  _savedConstraint;
        _rb.linearVelocity = _lastVelocity;
        drag.action.performed += _ => OnMouseDown();
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
}