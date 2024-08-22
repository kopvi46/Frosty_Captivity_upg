using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance {  get; private set; }

    [SerializeField] private InventorySlot[] inventorySlotArray;
    [SerializeField] private GameObject inventoryItemPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    public bool AddInventoryItem(ItemSO itemSO, Item item, int amount = 1)
    {
        //Check if picked item is stakable and is there any slot that can store one more same item
        if (itemSO.isStackable)
        {
            for (int i = 0; i < inventorySlotArray.Length; i++)
            {
                InventorySlot inventorySlot = inventorySlotArray[i];
                InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null && itemInSlot.ItemSO == itemSO && itemInSlot.amount < itemInSlot.ItemSO.maxStackAmount)
                {
                    itemInSlot.amount += amount;
                    itemInSlot.RefreshAmount(); 
                    return true;
                }
            }
        }
        //Find empy slot for placing picked item
        for (int i = 0; i < inventorySlotArray.Length; i++)
        {
            InventorySlot inventorySlot = inventorySlotArray[i];
            InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewInventoryItem(itemSO, inventorySlot, item.amount);
                return true;
            }
        }
        return false;
    }

    private void SpawnNewInventoryItem(ItemSO itemSO, InventorySlot inventorySlot, int amount)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, inventorySlot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(itemSO, amount);
    }

    public void SplitInventoryItem(InventoryItem inventoryItem)
    {
        int newInventoryItemAmount = inventoryItem.amount / 2;
        inventoryItem.amount = inventoryItem.amount - newInventoryItemAmount;

        inventoryItem.RefreshAmount();

        for (int i = 0; i < inventorySlotArray.Length; i++)
        {
            InventorySlot inventorySlot = inventorySlotArray[i];
            InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewInventoryItem(inventoryItem.ItemSO, inventorySlot, newInventoryItemAmount);
                break;
            }
        }
    }

    public void DropInventoryItem(ItemSO itemSO, int amount)
    {
        float spawnRadius = 2f;

        //Vector3 randomOffset = Vector3.Cross(Player.Instance.transform.position, Vector3.up).normalized * Random.Range(-spawnRadius, spawnRadius);
        Vector3 randomOffset = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));

        Vector3 spawnPosition = Player.Instance.transform.position + randomOffset;

        float randomRotation = Random.Range(0f, 360f);
        Quaternion spawnRotation = Quaternion.Euler(0, randomRotation, 0);

        Transform itemTransform = Instantiate(itemSO.prefab, spawnPosition, spawnRotation);
        Item newItem = itemTransform.GetComponent<Item>();
        newItem.amount = amount;
    }
}
