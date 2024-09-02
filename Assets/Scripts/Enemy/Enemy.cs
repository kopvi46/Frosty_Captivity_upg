using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _moveSpeed = 3;
    private List<Vector3> _pathVectorList;
    private int _currentPathIndex;

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mouseWorldPosition = MyGridUtils.GetMouse3DWorldPosition();
        //    SetTargetPosition(mouseWorldPosition);
        //    _currentPathIndex = 0;
        //}

        SetTargetPosition(Player.Instance.transform.position);

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

    public void SetTargetPosition(Vector3 targetPosition)
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
}
