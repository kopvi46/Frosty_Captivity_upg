using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SpecificInventorySlot;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector3 _scaleChange = new Vector3(.3f, .3f, .3f);
    private Transform _parentBeforeDrag;

    [SerializeField] private TextMeshProUGUI _amountVisual;

    [HideInInspector] public int amount = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Image image;

    public bool IsNeedToDestroyVisual = false;

    public ItemSO ItemSO { get; private set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = .6f;
        _rectTransform.localScale += _scaleChange;
        _canvasGroup.blocksRaycasts = false;

        _parentBeforeDrag = transform.parent;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        //if (parentAfterDrag.GetComponent<SpecificInventorySlot>() != null && parentAfterDrag.transform.childCount == 0)
        //{
        //    IsNeedToDestroyVisual = true;
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _rectTransform.localScale -= _scaleChange;
        _canvasGroup.blocksRaycasts = true;

        image.raycastTarget = true;

        //Drop item if it was dragged away from Inventory
        RectTransform inventoryRect = InventoryManager.Instance.InventoryUI;

        if (!RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, Input.mousePosition))
        {
            InventoryManager.Instance.DropInventoryItem(ItemSO, this, amount);
        } else
        {
            transform.SetParent(parentAfterDrag);

            if (_parentBeforeDrag.childCount == 0)
            {
                SpecificInventorySlot specificInventorySlot = _parentBeforeDrag.GetComponent<SpecificInventorySlot>();
                specificInventorySlot?.TriggerItemRemoved(this);
                IsNeedToDestroyVisual = false;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

        } else if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventoryManager.Instance.SplitInventoryItem(this);
        }
    }

    public void InitializeItem(ItemSO itemSO, int amount = 1)
    {
        ItemSO = itemSO;
        image.sprite = itemSO.sprite;
        this.amount = amount;
        RefreshAmount();
    }

    public void RefreshAmount()
    {
        _amountVisual.text = amount.ToString();
        bool isTextActive = amount > 1;
        _amountVisual.gameObject.SetActive(isTextActive);
    }
}
