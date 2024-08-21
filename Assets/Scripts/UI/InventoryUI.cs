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
    private Transform itemVisualTemplate;

    private const string ITEM_SLOT_CONTAINER = "ItemSlotContainer";
    private const string ITEM_SLOT_TEMPLATE = "ItemSlotTemplate";
    private const string ITEM_VISUAL_TEMPLATE = "ItemVisualTemplate";
    private const string IMAGE = "Image";
    private const string ITEM_AMOUNT = "ItemAmount";

    private void Awake()
    {
        itemSlotContainer = transform.Find(ITEM_SLOT_CONTAINER);
        itemSlotTemplate = itemSlotContainer.Find(ITEM_SLOT_TEMPLATE);
        itemVisualTemplate = itemSlotTemplate.Find(ITEM_VISUAL_TEMPLATE);
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
        int i = 0;
        foreach (Transform child in itemSlotContainer)
        {

            Transform grandChild = child.Find(ITEM_VISUAL_TEMPLATE);

            if (i < inventory.GetItemList().Count)
            {
                Item item = inventory.GetItemList()[i];
                Image image = grandChild.Find(IMAGE).GetComponent<Image>();
                TextMeshProUGUI itemAmount = grandChild.Find(ITEM_AMOUNT).GetComponent<TextMeshProUGUI>();

                grandChild.gameObject.SetActive(true);
                image.sprite = item.GetItemSO().sprite;
                itemAmount.SetText(item.GetItemAmount() > 1 ? item.GetItemAmount().ToString() : "");
            } else
            {
                grandChild.gameObject.SetActive(false);
            }
            i++;

            //InventoryClickHandler clickHandler = itemVisualRectTransform.GetComponent<InventoryClickHandler>();

            //if (clickHandler != null)
            //{
            //    clickHandler = itemVisualRectTransform.gameObject.AddComponent<InventoryClickHandler>();
            //}

            //clickHandler.OnLeftClick += (sender, e) =>
            //{
            //    inventory.UseItem(item);
            //    RefreshInventoryItems();
            //};

            //clickHandler.OnRightClick += (sender, e) =>
            //{
            //    if (item.GetItemAmount() > 1)
            //    {
            //        item.SetItemAmount(item.GetItemAmount() - 1);

            //        Item.DropItem(item);

            //        RefreshInventoryItems();
            //    } else
            //    {
            //        inventory.RemoveItem(item);
            //        Item.DropItem(item);
            //    }
            //
        }
    }
}
