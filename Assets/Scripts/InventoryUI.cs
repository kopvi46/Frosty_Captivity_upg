using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        foreach (Item itemObject in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            Image image = itemSlotRectTransform.Find(IMAGE).GetComponent<Image>();
            image.sprite = itemObject.GetItemSO().sprite;

            TextMeshProUGUI itemAmount = itemSlotRectTransform.Find(ITEM_AMOUNT).GetComponent<TextMeshProUGUI>();
            if (itemObject.GetItemSO().amount > 1)
            {
                itemAmount.SetText(itemObject.GetItemSO().amount.ToString());
            } else
            {
                itemAmount.SetText("");
            }
        }
    }
}
