using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryMaterialItem : InventoryItem
{
    //[SerializeField] private TextMeshProUGUI _amountVisual;

    //[HideInInspector] public int amount = 1;

    public override void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _rectTransform.localScale -= _scaleChange;
        _canvasGroup.blocksRaycasts = true;

        image.raycastTarget = true;

        //Drop item if it was dragged away from Inventory
        RectTransform inventoryRect = InventoryManager.Instance.InventoryUI;

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

    public void InitializeMaterialItem(ItemSO itemSO, int amount = 1)
    {
        ItemSO = itemSO;
        image.sprite = itemSO.sprite;
        this.amount = amount;
        RefreshAmount();
    }

    //public void RefreshAmount()
    //{
    //    _amountVisual.text = amount.ToString();
    //    bool isTextActive = amount > 1;
    //    _amountVisual.gameObject.SetActive(isTextActive);
    //}
}
