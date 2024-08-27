using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [SerializeField] private Transform _playerLeftHandPoint;
    [SerializeField] private Transform _playerRightHandPoint;
    [SerializeField] private LayerMask _defaultLayerMask;

    public Transform PlayerLeftHandPoint
    {
        get { return _playerLeftHandPoint; }
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
        InventoryManager.Instance.LeftHandSlot.OnItemAdded += LeftHandSlot_OnItemAdded;
        InventoryManager.Instance.RightHandSlot.OnItemAdded += RightHandSlot_OnItemAdded;
        InventoryManager.Instance.LeftHandSlot.OnItemRemoved += LeftHandSlot_OnItemRemoved;
        InventoryManager.Instance.RightHandSlot.OnItemRemoved += RightHandSlot_OnItemRemoved;

    }

    private void RightHandSlot_OnItemRemoved(object sender, SpecificInventorySlot.OnItemRemovedEventArgs e)
    {
        DestroySlotVisual(PlayerRightHandPoint);
    }

    private void LeftHandSlot_OnItemRemoved(object sender, SpecificInventorySlot.OnItemRemovedEventArgs e)
    {
        DestroySlotVisual(PlayerLeftHandPoint);
    }

    private void RightHandSlot_OnItemAdded(object sender, SpecificInventorySlot.OnItemAddedEventArgs e)
    {
        DisplaySlotVisual(PlayerRightHandPoint, e);
    }

    private void LeftHandSlot_OnItemAdded(object sender, SpecificInventorySlot.OnItemAddedEventArgs e)
    {
        DisplaySlotVisual(PlayerLeftHandPoint, e);
    }

    private void DisplaySlotVisual(Transform playerHandPoint, SpecificInventorySlot.OnItemAddedEventArgs e)
    {
        Transform itemTransform = Instantiate(e.inventoryItem.ItemSO.prefab, playerHandPoint.transform.position, playerHandPoint.transform.rotation);

        itemTransform.SetParent(playerHandPoint.transform);

        itemTransform.gameObject.layer = _defaultLayerMask;

        InventoryEquipmentItem inventoryEquipmentItem = e.inventoryItem as InventoryEquipmentItem;

        itemTransform.GetComponent<Item>().durability = inventoryEquipmentItem.durability;
    }

    private void DestroySlotVisual(Transform playerHandPoint)
    {
        foreach (Transform child in playerHandPoint.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
