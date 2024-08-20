using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private const string ITEM_SLOT_CONTAINER = "ItemSlotContainer";
    private const string ITEM_SLOT_TEMPLATE = "ItemSlotTemplate";
    private const string IMAGE = "Image";
    private const string ITEM_AMOUNT = "ItemAmount";

    private void Awake()
    {
        itemSlotContainer = transform.Find(ITEM_SLOT_CONTAINER);
        itemSlotTemplate = itemSlotContainer.Find(ITEM_SLOT_TEMPLATE);
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            InventoryClickHandler clickHandler = itemSlotRectTransform.GetComponent<InventoryClickHandler>();

            if (clickHandler != null)
            {
                clickHandler = itemSlotRectTransform.gameObject.AddComponent<InventoryClickHandler>();
            }

            clickHandler.OnLeftClick += (sender, e) =>
            {
                inventory.UseItem(item);
                RefreshInventoryItems();
            };

            clickHandler.OnRightClick += (sender, e) =>
            {
                if (item.GetItemAmount() > 1)
                {
                    item.SetItemAmount(item.GetItemAmount() - 1);

                    Item.DropItem(item);

                    RefreshInventoryItems();
                } else
                {
                    inventory.RemoveItem(item);
                    Item.DropItem(item);
                }
            };

            Image image = itemSlotRectTransform.Find(IMAGE).GetComponent<Image>();
            image.sprite = item.GetItemSO().sprite;

            TextMeshProUGUI itemAmount = itemSlotRectTransform.Find(ITEM_AMOUNT).GetComponent<TextMeshProUGUI>();
            if (item.GetItemAmount() > 1)
            {
                itemAmount.SetText(item.GetItemAmount().ToString());
            } else
            {
                itemAmount.SetText("");
            }
        }
    }
}
