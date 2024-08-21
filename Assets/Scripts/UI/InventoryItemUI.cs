using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;

    [HideInInspector] public ItemSO itemSO;
    [HideInInspector] public Transform parentAfterDrag;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 scaleChange = new Vector3(.3f, .3f, .3f);

    public Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        rectTransform.localScale += scaleChange;
        canvasGroup.blocksRaycasts = false;

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
        canvasGroup.alpha = 1f;
        rectTransform.localScale -= scaleChange;
        canvasGroup.blocksRaycasts = true;

        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void InitializeItem(ItemSO itemSO)
    {
        this.itemSO = itemSO;
        image.sprite = itemSO.sprite;
    }
}
