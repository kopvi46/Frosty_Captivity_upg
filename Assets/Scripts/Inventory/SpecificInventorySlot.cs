using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpecificInventorySlot : MonoBehaviour, IDropHandler
{
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
        } else if (transform.childCount != 0 && droppedEquipmentSO != null && droppedEquipmentSO.handType == _storedItemType.handType)
        {
            //Slot is not empty
            InventoryItem storedItem = GetComponentInChildren<InventoryItem>();

            if (storedItem != null)
            {
                storedItem.transform.SetParent(droppedItem.parentAfterDrag);

                droppedItem.parentAfterDrag = transform;
            }
        } 
    }
}
