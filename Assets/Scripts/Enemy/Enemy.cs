using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Chase,
        Attack,
    }
    private State _state;
    private int _moveSpeed = 3;
    private List<Vector3> _pathVectorList;
    private int _currentPathIndex;
    private float _attackRange = 3f;
    private float _chaseRange = 15f;
    private float _patrolTimer;
    private float _patrolTargetChangeDelay = 3f;
    private float _attackTimer;
    private float _attackDelay = 2f;
    private int _damage = 10;

    private void Start()
    {
        _state = State.Patrol;

        _patrolTimer = _patrolTargetChangeDelay;
        _attackTimer = _attackDelay;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Patrol:
                _patrolTimer += Time.deltaTime;

                if (_patrolTimer >= _patrolTargetChangeDelay)
                {
                    _patrolTimer = 0f;
                    SetRandomPatrolTargetInRange(10);
                }

                if (Vector3.Distance(transform.position, Player.Instance.transform.position) < _chaseRange)
                {
                    _state = State.Chase;
                }
                break;
            case State.Chase:
                if (Vector3.Distance(transform.position, Player.Instance.transform.position) < _attackRange)
                {
                    _state = State.Attack;
                } else if (Vector3.Distance(transform.position, Player.Instance.transform.position) > _chaseRange)
                {
                    _state = State.Patrol;
                } else
                {
                    SetTargetPosition(Player.Instance.transform.position);
                }
                break;
            case State.Attack:
                _attackTimer += Time.deltaTime;

                if (_attackTimer >= _attackDelay)
                {
                    _attackTimer = 0f;
                    PlayerHealthManager.Instance.PlayerHealth -= _damage;
                }

                if (Vector3.Distance(transform.position, Player.Instance.transform.position) > _attackRange)
                {
                    StopMoving();
                    _attackTimer = _attackDelay;
                    _state = State.Chase;
                }
                break;
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mouseWorldPosition = MyGridUtils.GetMouse3DWorldPosition();
        //    SetTargetPosition(mouseWorldPosition);
        //    _currentPathIndex = 0;
        //}

        //SetTargetPosition(Player.Instance.transform.position);

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_pathVectorList != null && _currentPathIndex < _pathVectorList.Count)
        {
            Vector3 targetPosition = _pathVectorList[_currentPathIndex];

            if (Vector3.Distance(transform.position, targetPosition) > .1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * _moveSpeed * Time.deltaTime;
            } else
            {
                _currentPathIndex++;

                if (_currentPathIndex >= _pathVectorList.Count)
                {
                    StopMoving();
                }
            }
        }
    }

    private void StopMoving()
    {
        _pathVectorList = null;
    }

    private void SetTargetPosition(Vector3 targetPosition)
    {
        _currentPathIndex = 0;

        _pathVectorList = Pathfinding.Instance.FindPath(transform.position, targetPosition);

        if (_pathVectorList != null && _pathVectorList.Count > 1)
        {
            _pathVectorList.RemoveAt(0);

            //for (int i = 0; i < _pathVectorList.Count - 1; i++)
            //{
            //    Vector3 startPosition = _pathVectorList[i];
            //    Vector3 endPosition = _pathVectorList[i + 1];
            //    Debug.DrawLine(startPosition, endPosition, Color.green, 20f);
            //}
        }
    }

    private void SetRandomPatrolTargetInRange(int range)
    {
        Vector3 targetPosition = transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        SetTargetPosition(targetPosition);
    }
}
