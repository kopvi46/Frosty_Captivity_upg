using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public event EventHandler OnInventoryChanged;
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private InventorySlot[] _inventorySlotArray;
    [SerializeField] private GameObject _inventoryItemPrefab;
    [SerializeField] private GameObject _inventoryEquipmentPrefab;
    [SerializeField] private SpecificInventorySlot _leftHandSlot;
    [SerializeField] private SpecificInventorySlot _rightHandSlot;
    [SerializeField] private RectTransform _inventoryUI;

    [SerializeField] private ItemSO _branch;
    [SerializeField] private ItemSO _log;
    [SerializeField] private ItemSO _stone;

    public RectTransform InventoryUI
    {
        get { return _inventoryUI; }
        private set { _inventoryUI = value; }
    }
    public SpecificInventorySlot LeftHandSlot
    {
        get { return _leftHandSlot; }
        private set { _leftHandSlot = value; }
    }
    public SpecificInventorySlot RightHandSlot
    {
        get { return _rightHandSlot; }
        private set { _rightHandSlot = value; }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeDefoltInventory();
    }

    public bool AddInventoryItem(ItemSO itemSO, Item item, int amount, int durability)
    {
        //Check if picked item is stackable and if there is any slot that can store one more same item and if so than place item in this slot
        if (itemSO.isStackable)
        {
            for (int i = 0; i < _inventorySlotArray.Length; i++)
            {
                InventorySlot inventorySlot = _inventorySlotArray[i];
                InventoryMaterialItem materialItemInSlot = inventorySlot.GetComponentInChildren<InventoryMaterialItem>();

                if (materialItemInSlot != null && materialItemInSlot.ItemSO == itemSO && materialItemInSlot.amount < materialItemInSlot.ItemSO.maxStackAmount)
                {
                    materialItemInSlot.amount += amount;
                    materialItemInSlot.RefreshAmount();

                    OnInventoryChanged?.Invoke(this, EventArgs.Empty);

                    return true;
                }
            }
        }
        //Find empty slot for placing picked item and place it there
        for (int i = 0; i < _inventorySlotArray.Length; i++)
        {
            InventorySlot inventorySlot = _inventorySlotArray[i];
            InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                //SpawnNewInventoryItem(itemSO, inventorySlot, item.amount, item.durability);
                SpawnNewInventoryItem(itemSO, inventorySlot, amount, durability);

                OnInventoryChanged?.Invoke(this, EventArgs.Empty);

                return true;
            }
        }
        return false;
    }

    private void SpawnNewInventoryItem(ItemSO itemSO, InventorySlot inventorySlot, int amount, int durability)
    {
        if (itemSO is EquipmentSO)
        {
            GameObject newItemGO = Instantiate(_inventoryEquipmentPrefab, inventorySlot.transform);
            InventoryEquipmentItem inventoryEquipment = newItemGO.GetComponent<InventoryEquipmentItem>();
            inventoryEquipment.InitializeEquipmentItem(itemSO as EquipmentSO, durability);
        } else
        {
            GameObject newItemGO = Instantiate(_inventoryItemPrefab, inventorySlot.transform);
            InventoryMaterialItem inventoryMaterialItem = newItemGO.GetComponent<InventoryMaterialItem>();
            inventoryMaterialItem.InitializeMaterialItem(itemSO, amount);
        }
    }

    public void SplitInventoryItem(InventoryMaterialItem inventoryMaterialItem)
    {
        int newInventoryItemAmount = inventoryMaterialItem.amount / 2;
        inventoryMaterialItem.amount = inventoryMaterialItem.amount - newInventoryItemAmount;

        inventoryMaterialItem.RefreshAmount();

        //Looking for nearest empty slot and placing there half of split item amount
        for (int i = 0; i < _inventorySlotArray.Length; i++)
        {
            InventorySlot inventorySlot = _inventorySlotArray[i];
            InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                SpawnNewInventoryItem(inventoryMaterialItem.ItemSO, inventorySlot, newInventoryItemAmount, 1);
                break;
            }
        }
    }

    public void DropInventoryItem(ItemSO itemSO, InventoryItem inventoryItem, int amount, int durability)
    {
        //Spawn item in a world and delete from inventory
        float spawnRadius = 2f;

        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius), .1f, UnityEngine.Random.Range(-spawnRadius, spawnRadius));

        Vector3 spawnPosition = Player.Instance.transform.position + randomOffset;

        float randomRotation = UnityEngine.Random.Range(0f, 360f);
        Quaternion spawnRotation = Quaternion.Euler(0, randomRotation, 0);

        if (inventoryItem.ItemSO is EquipmentSO)
        {
            spawnRotation = Quaternion.Euler(90, randomRotation, 0);
        }

        Transform itemTransform = Instantiate(itemSO.prefab, spawnPosition, spawnRotation);
        Item newItem = itemTransform.GetComponent<Item>();
        newItem.amount = amount;
        newItem.durability = durability;

        Destroy(inventoryItem.gameObject);

        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public InventorySlot[] GetInventorySlotArray()
    {
        return _inventorySlotArray;
    }

    private void InitializeDefoltInventory()
    {
        SpawnNewInventoryItem(_branch, _inventorySlotArray[0], _branch.maxStackAmount, 1);
        SpawnNewInventoryItem(_log, _inventorySlotArray[1], _log.maxStackAmount, 1);
        SpawnNewInventoryItem(_stone, _inventorySlotArray[2], _stone.maxStackAmount, 1);

        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }
}
