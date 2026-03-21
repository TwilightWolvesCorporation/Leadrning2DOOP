using System.Collections.Generic;
using UnityEngine;

public class WaypointEnemy : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTime = 3f;

    private int _currentWaypointIndex = 0;
    private float _waitTimer = 0f;
    private bool _waiting;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.position = waypoints[_currentWaypointIndex].position;
    }

    private void FixedUpdate()
    {
        if (waypoints.Count == 0) return;

        if (_waiting)
        {
            _waitTimer -= Time.fixedDeltaTime;
            if (_waitTimer <= 0f)
            {
                _waiting = false;
                _currentWaypointIndex = _currentWaypointIndex == waypoints.Count - 1 ? 0 : _currentWaypointIndex + 1;
            }
            else
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        var targetPosition = waypoints[_currentWaypointIndex].position;
        var direction = (targetPosition - transform.position).normalized;

        _rb.linearVelocity = direction * speed;


        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            _rb.linearVelocity = Vector2.zero;
            _waiting = true;
            _waitTimer = waitTime;
        }
    }
}