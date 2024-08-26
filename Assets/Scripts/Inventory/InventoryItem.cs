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
    protected RectTransform _rectTransform;
    protected CanvasGroup _canvasGroup;
    protected Vector3 _scaleChange = new Vector3(.3f, .3f, .3f);
    protected Transform _parentBeforeDrag;

    [SerializeField] protected TextMeshProUGUI _amountVisual;

    [HideInInspector] public int amount;
    [HideInInspector] public int durability;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Image image;

    public ItemSO ItemSO { get; protected set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData) { }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

        } else if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventoryManager.Instance.SplitInventoryItem(this as InventoryMaterialItem);
        }
    }

    //public void InitializeItem(ItemSO itemSO, int amount = 1)
    //{
    //    ItemSO = itemSO;
    //    image.sprite = itemSO.sprite;
    //    this.amount = amount;
    //    RefreshAmount();
    //}

    public void RefreshAmount()
    {
        _amountVisual.text = amount.ToString();
        bool isTextActive = amount > 1;
        _amountVisual.gameObject.SetActive(isTextActive);
    }
}
