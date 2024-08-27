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

    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _interactableLayerMask;
    [SerializeField] private LayerMask _defaultLayerMask;
    [SerializeField] private Transform _playerLeftHandPoint;
    [SerializeField] private Transform _playerRightHandPoint;

    private int _playerHealth = 100;
    private float _playerHealthChangeDelay = 3f;
    private float _playerHealtChangeTimer = 0f;
    private Vector3 _lastInteractDirection;
    private IInteractable _selectedObject;
    //private Item _playerLeftHandHold;
    //private Item _playerRightHandHold;

    public int PlayerHealth 
    {  
        get 
        { 
            return _playerHealth; 
        } 
        set
        {
            int playerMinHealth = 0;
            int playerMaxHealth = 100;
            if (value < playerMinHealth)
            {
                _playerHealth = playerMinHealth;
            } else if (value > playerMaxHealth)
            {
                _playerHealth = playerMaxHealth;
            } else 
            { 
                _playerHealth = value;
            }

            OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs
            {
                healthNormalized = (float)PlayerHealth / playerMaxHealth
            });
        } 
    }
    public Transform PlayerLeftHandPoint 
    { 
        get { return _playerLeftHandPoint;} 
        private set { _playerLeftHandPoint = value; } 
    }
    public Transform PlayerRightHandPoint
    {
        get { return _playerRightHandPoint; }
        private set { _playerRightHandPoint = value; }
    }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
        InventoryManager.Instance.LeftHandSlot.OnItemAdded += LeftHandSlot_OnItemAdded;
        InventoryManager.Instance.RightHandSlot.OnItemAdded += RightHandSlot_OnItemAdded;
        InventoryManager.Instance.LeftHandSlot.OnItemRemoved += LeftHandSlot_OnItemRemoved;
        InventoryManager.Instance.RightHandSlot.OnItemRemoved += RightHandSlot_OnItemRemoved;

    }

    private void RightHandSlot_OnItemRemoved(object sender, SpecificInventorySlot.OnItemRemovedEventArgs e)
    {
        foreach (Transform child in PlayerRightHandPoint.transform)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Hide item");
    }

    private void LeftHandSlot_OnItemRemoved(object sender, SpecificInventorySlot.OnItemRemovedEventArgs e)
    {
        foreach (Transform child in PlayerLeftHandPoint.transform)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Hide item");
    }

    private void RightHandSlot_OnItemAdded(object sender, SpecificInventorySlot.OnItemAddedEventArgs e)
    {
        Transform itemTransform = Instantiate(e.inventoryItem.ItemSO.prefab, PlayerRightHandPoint.transform.position, PlayerLeftHandPoint.transform.rotation);

        itemTransform.SetParent(PlayerRightHandPoint.transform);

        itemTransform.gameObject.layer = _defaultLayerMask;

        InventoryEquipmentItem inventoryEquipmentItem = e.inventoryItem as InventoryEquipmentItem;

        itemTransform.GetComponent<Item>().durability = inventoryEquipmentItem.durability;

        Debug.Log("Show item");
    }

    private void LeftHandSlot_OnItemAdded(object sender, SpecificInventorySlot.OnItemAddedEventArgs e)
    {
        Transform itemTransform = Instantiate(e.inventoryItem.ItemSO.prefab, PlayerLeftHandPoint.transform.position, PlayerLeftHandPoint.transform.rotation);

        itemTransform.SetParent(PlayerLeftHandPoint.transform);

        itemTransform.gameObject.layer = _defaultLayerMask;

        InventoryEquipmentItem inventoryEquipmentItem = e.inventoryItem as InventoryEquipmentItem;

        itemTransform.GetComponent<Item>().durability = inventoryEquipmentItem.durability;

        Debug.Log("Show item");
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

        _playerHealtChangeTimer -= Time.deltaTime;

        if (FireplaceHeatZone.Instance.IsPlayerTriggered())
        {
            if (_playerHealtChangeTimer < 0)
            {
                _playerHealtChangeTimer = _playerHealthChangeDelay;
                PlayerHealth += 10;
            }
        } else
        {
            if (PlayerLeftHandPoint.childCount != 0 && PlayerLeftHandPoint.GetComponentInChildren<Item>().ItemSO is EquipmentSO equipmentSO 
                && equipmentSO.equipmentType == EquipmentSO.EquipmentType.Torch)
            {
                if (_playerHealtChangeTimer < 0)
                {
                    //Debug.Log("Player under torch protection!");
                }
            } else
            {
                if (_playerHealtChangeTimer < 0)
                {
                    _playerHealtChangeTimer = _playerHealthChangeDelay;
                    PlayerHealth -= 10;
                }
            }
        }
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
