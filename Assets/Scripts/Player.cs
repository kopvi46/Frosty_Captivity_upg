using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IHasHealth
{
    public static Player Instance;

    public event EventHandler<IHasHealth.OnHealthChangedEventArgs> OnHealthChanged;
    public event EventHandler<OnSelectedObjectChangedEventArgs> OnSelectedObjectChanged;
    public class OnSelectedObjectChangedEventArgs : EventArgs
    {
        public IInteractable selectedObject;
    }

    [SerializeField] private float moveSpeed;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask interactableLayerMask;

    private int playerHealth = 100;
    private float playerHealthChangeDelay = 3f;
    private float playerHealtChangeTimer = 0f;
    private Vector3 lastInteractDirection;
    private IInteractable selectedObject;

    public int PlayerHealth 
    {  
        get 
        { 
            return playerHealth; 
        } 
        set
        {
            int playerMinHealth = 0;
            int playerMaxHealth = 100;
            if (value < playerMinHealth)
            {
                playerHealth = playerMinHealth;
            } else if (value > playerMaxHealth)
            {
                playerHealth = playerMaxHealth;
            } else 
            { 
                playerHealth = value;
            }

            OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs
            {
                healthNormalized = (float)PlayerHealth / playerMaxHealth
            });
        } 
    }


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();

        playerHealtChangeTimer -= Time.deltaTime;

        if (FireplaceHeatZone.Instance.GetIsPlayerTriggered())
        {
            if (playerHealtChangeTimer < 0)
            {
                playerHealtChangeTimer = playerHealthChangeDelay;
                PlayerHealth += 10;
            }
        } else
        {
            if (playerHealtChangeTimer < 0)
            {
                playerHealtChangeTimer = playerHealthChangeDelay;
                PlayerHealth -= 10;
            }
        }
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 directionVector = GetDirectionVector(inputVector);

        if (directionVector != Vector3.zero)
        {
            lastInteractDirection = directionVector;
        }

        float intreractDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, intreractDistance, interactableLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out IInteractable interactable))
            {
                if (interactable != selectedObject)
                {
                    SetSelectedObject(interactable);
                }
            } else
            {
                SetSelectedObject(null);
            }
        } else
        {
            SetSelectedObject(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 directionVector = GetDirectionVector(inputVector);

        float moveDistance = moveSpeed * Time.deltaTime;

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
        this.selectedObject = selectedObject;

        OnSelectedObjectChanged?.Invoke(this, new OnSelectedObjectChangedEventArgs
        {
            selectedObject = selectedObject
        });
    }
}
