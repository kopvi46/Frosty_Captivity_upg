using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public event EventHandler<OnSelectedObjectChangedEventArgs> OnSelectedObjectChanged;

    public class OnSelectedObjectChangedEventArgs : EventArgs
    {
        public IInteractable selectedObject;
    }

    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _interactableLayerMask;

    private Vector3 _lastInteractDirection;
    private IInteractable _selectedObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;

    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_selectedObject != null)
        {
            _selectedObject.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        float interactionRadius = 1f;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRadius, _interactableLayerMask);

        IInteractable closestInteractable = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                float distanceSqr = (collider.transform.position - transform.position).sqrMagnitude;

                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestInteractable = interactable;
                }
            }
        }

        if (closestInteractable != null)
        {
            if (closestInteractable != _selectedObject)
            {
                SetSelectedObject(closestInteractable);
            }
        } else
        {
            SetSelectedObject(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 directionVector = GetDirectionVector(inputVector);

        float moveDistance = _moveSpeed * Time.deltaTime;

        float playerRadius = .6f;
        float playerHeight = .6f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVector, out RaycastHit hit, moveDistance);
        if (!canMove && hit.collider.isTrigger)
        {
            //Player hit a trigger, so he can move towards directionVector
            canMove = true;
        }

        if (!canMove)
        {
            //Cannot move towards directionVector

            //Atempt only x movement
            Vector3 directionVectorX = new Vector3(directionVector.x, 0, 0).normalized;
            canMove = (directionVector.x < -.5f || directionVector.x > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVectorX, moveDistance);

            if (canMove)
            {
                //Can move only on the X
                directionVector = directionVectorX;
            } else
            {
                //Cannot move only on the X

                //Attempt only Z movement
                Vector3 directionVectorZ = new Vector3(0, 0, directionVector.z).normalized;
                canMove = (directionVector.z < -.5f || directionVector.z > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVectorZ, moveDistance);

                if (canMove)
                {
                    //Can move only on the Z
                    directionVector = directionVectorZ;
                } else
                {
                    //Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += directionVector * moveDistance;
        }

        if (directionVector != Vector3.zero)
        {
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, directionVector, Time.deltaTime * rotateSpeed);
        }
    }

    private Vector3 GetDirectionVector(Vector2 inputVector)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 directionVector = cameraForward * inputVector.y + cameraRight * inputVector.x;

        return directionVector;
    }

    private void SetSelectedObject(IInteractable selectedObject)
    {
        this._selectedObject = selectedObject;

        OnSelectedObjectChanged?.Invoke(this, new OnSelectedObjectChangedEventArgs
        {
            selectedObject = selectedObject
        });
    }

    public Vector3 GetLastInteractDirection()
    {
        return _lastInteractDirection;
    }
}
