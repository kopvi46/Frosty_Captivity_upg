using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        
        if (transform.childCount == 0)
        {
            //Slot is empty
            droppedItem.parentAfterDrag = transform;
        } else
        {
            //Slot is not empty
            InventoryItem storedItem = GetComponentInChildren<InventoryItem>();

            if (storedItem.ItemSO.isStackable && storedItem.ItemSO.IsSameItemType(droppedItem.ItemSO))
            {
                //Slot have the same stackable item
                if (storedItem.amount + droppedItem.amount <= storedItem.ItemSO.maxStackAmount)
                {
                    //Item amount can be hold in one stack
                    storedItem.amount += droppedItem.amount;
                    storedItem.RefreshAmount();
                    Destroy(droppedItem.gameObject);
                } else
                {
                    //Item amount can not be hold in one stack
                    droppedItem.amount -= storedItem.ItemSO.maxStackAmount - storedItem.amount;
                    droppedItem.RefreshAmount();                    
                    storedItem.amount = storedItem.ItemSO.maxStackAmount;
                    storedItem.RefreshAmount();
                }
            } else
            {
                
                //Slot have different item
                if (storedItem != null && !droppedItem.parentAfterDrag.GetComponent<SpecificInventorySlot>())
                {
                    storedItem.transform.SetParent(droppedItem.parentAfterDrag);

                    droppedItem.parentAfterDrag = transform;
                }
            }
        }
    }
}