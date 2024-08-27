using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryEquipmentItem : InventoryItem
{
    [SerializeField] private TextMeshProUGUI _durabilityVisual;

    public override void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _rectTransform.localScale -= _scaleChange;
        _canvasGroup.blocksRaycasts = true;

        image.raycastTarget = true;

        //Drop item if it was dragged away from Inventory or use it to restore Fireplace if it was dragged on it
        RectTransform inventoryRect = InventoryManager.Instance.InventoryUI;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        bool isFireplaceHit = false;

        //Look for Fireplace
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == Fireplace.Instance.transform)
            {
                //Can't burn equipment so return it to inventory
                transform.SetParent(parentAfterDrag);

                isFireplaceHit = true;
                break;
            }
        }

        //Drop item if Fireplace was not hit
        if (!isFireplaceHit)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, Input.mousePosition))
            {
                InventoryManager.Instance.DropInventoryItem(ItemSO, this, amount, durability);
            } else
            {
                transform.SetParent(parentAfterDrag);

                if (_parentBeforeDrag.childCount == 0)
                {
                    SpecificInventorySlot specificInventorySlot = _parentBeforeDrag.GetComponent<SpecificInventorySlot>();
                    specificInventorySlot?.TriggerItemRemoved(this);
                }
            }
        }
    }

    public void InitializeEquipmentItem(ItemSO itemSO, int durability = 100)
    {
        ItemSO = itemSO;
        image.sprite = itemSO.sprite;
        this.durability = durability;
        RefreshDurability();
    }

    public void RefreshDurability()
    {
        _durabilityVisual.text = durability.ToString() + "%";
    }
}
