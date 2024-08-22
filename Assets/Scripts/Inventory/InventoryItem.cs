using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector3 _scaleChange = new Vector3(.3f, .3f, .3f);

    [SerializeField] private TextMeshProUGUI _amountVisual;

    [HideInInspector] public int amount = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Image image;

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

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
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
        //transform.SetParent(parentAfterDrag);

        RectTransform inventoryRect = parentAfterDrag.parent.GetComponent<RectTransform>();
        if (!RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, Input.mousePosition))
        {
            InventoryManager.Instance.DropInventoryItem(ItemSO, amount);
            Destroy(gameObject);
        } else
        {
            transform.SetParent(parentAfterDrag);
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
