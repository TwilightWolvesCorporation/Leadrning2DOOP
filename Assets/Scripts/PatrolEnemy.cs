using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTime = 3f;

    private int _currentWaypointIndex = 0;
    private float _waitTimer = 0f;
    private bool _waiting;

    private bool _isPatrol = true;
    private Rigidbody2D _rb;
    private Transform _player;
    private bool _playerDetected = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.position = waypoints[_currentWaypointIndex].position;
    }

    private void Update()
    {
        if (_player)
        {
            var directionToPlayer = (_player.position - transform.position).normalized;
            var distance = Vector2.Distance(transform.position, _player.position);
        }
    }

    private void FixedUpdate()
    {
        if (_playerDetected) Hunt();
        else Patrol();
    }

    private void Patrol()
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

        if (direction.x != 0) transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    private void Hunt()
    {
        if (!_player) return;
        var direction = (_player.position - transform.position).normalized;
        _rb.linearVelocity = direction * speed;

        if (direction.x != 0) transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    public void PlayerDetect(Transform player, bool isDetected)
    {
        if(player) player.gameObject.GetComponent<PlayerController>().Hurt(2);
        _player = isDetected ? player : null;
        _playerDetected = isDetected;
    }
}