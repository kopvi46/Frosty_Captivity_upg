using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class SpecificInventorySlot : MonoBehaviour, IDropHandler
{
    public event EventHandler<OnItemAddedEventArgs> OnItemAdded;
    public event EventHandler<OnItemRemovedEventArgs> OnItemRemoved;

    public class OnItemAddedEventArgs : EventArgs
    {
        public InventoryItem inventoryItem;
        public SpecificInventorySlot specificInventorySlot;
    }
    public class OnItemRemovedEventArgs : EventArgs
    {
        public InventoryItem inventoryItem;
        public SpecificInventorySlot specificInventorySlot;
    }

    [SerializeField] private EquipmentSO _storedItemType;

    public EquipmentSO StoredItemType
    {
        get { return _storedItemType; }
        private set { _storedItemType = value; }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedEquipmentItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        EquipmentSO droppedEquipmentItemSO = droppedEquipmentItem.ItemSO as EquipmentSO;

        if (transform.childCount == 0 && droppedEquipmentItemSO != null && droppedEquipmentItemSO.handType == _storedItemType.handType)
        {
            //Slot is empty
            droppedEquipmentItem.parentAfterDrag = transform;

            OnItemAdded?.Invoke(this, new OnItemAddedEventArgs { inventoryItem = droppedEquipmentItem, specificInventorySlot = this });

        } else if (transform.childCount != 0 && droppedEquipmentItemSO != null && droppedEquipmentItemSO.handType == _storedItemType.handType)
        {
            //Slot is not empty
            InventoryItem storedEquipmentItem = GetComponentInChildren<InventoryItem>();
            EquipmentSO storedEquipmentItemSO = storedEquipmentItem.ItemSO as EquipmentSO;

            if (storedEquipmentItem != null && droppedEquipmentItemSO != null && droppedEquipmentItemSO.handType == storedEquipmentItemSO.handType)
            {
                storedEquipmentItem.transform.SetParent(droppedEquipmentItem.parentAfterDrag);

                OnItemRemoved?.Invoke(this, new OnItemRemovedEventArgs { inventoryItem = storedEquipmentItem, specificInventorySlot = this });

                droppedEquipmentItem.parentAfterDrag = transform;
                OnItemAdded?.Invoke(this, new OnItemAddedEventArgs { inventoryItem = droppedEquipmentItem, specificInventorySlot = this });
            }
        }
    }

    public void TriggerItemRemoved(InventoryItem inventoryItem)
    {
        OnItemRemoved?.Invoke(this, new OnItemRemovedEventArgs { inventoryItem = inventoryItem, specificInventorySlot = this });
    }
}
