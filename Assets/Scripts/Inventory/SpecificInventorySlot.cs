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
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        EquipmentSO droppedEquipmentSO = droppedItem.ItemSO as EquipmentSO;

        if (transform.childCount == 0 && droppedEquipmentSO != null && droppedEquipmentSO.handType == _storedItemType.handType)
        {
            //Slot is empty
            droppedItem.parentAfterDrag = transform;

            OnItemAdded?.Invoke(this, new OnItemAddedEventArgs { inventoryItem = droppedItem, specificInventorySlot = this });

        } else if (transform.childCount != 0 && droppedEquipmentSO != null && droppedEquipmentSO.handType == _storedItemType.handType)
        {
            //Slot is not empty
            InventoryItem storedItem = GetComponentInChildren<InventoryItem>();

            if (storedItem != null)
            {
                storedItem.transform.SetParent(droppedItem.parentAfterDrag);

                OnItemRemoved?.Invoke(this, new OnItemRemovedEventArgs { inventoryItem = storedItem, specificInventorySlot = this });

                droppedItem.parentAfterDrag = transform;
                OnItemAdded?.Invoke(this, new OnItemAddedEventArgs { inventoryItem = droppedItem, specificInventorySlot = this });
            }
        }
    }

    public void TriggerItemRemoved(InventoryItem inventoryItem)
    {
        OnItemRemoved?.Invoke(this, new OnItemRemovedEventArgs { inventoryItem = inventoryItem, specificInventorySlot = this });
    }
}
