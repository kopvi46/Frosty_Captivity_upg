using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftItemButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private const string REQUIRED_INGREDIENT = "RequiredIngredient";
    private const string AMOUNT_VISUAL = "AmountVisual";

    [SerializeField] private CraftRecipeSO _craftRecipeSO;
    [SerializeField] private Transform _ingredientListVisual;
    [SerializeField] private Transform _ingredientSlotVisual;

    [HideInInspector] public CanvasGroup canvasGroup;
    [HideInInspector] public bool canCraftRecipe;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Image crafredItemImage = gameObject.GetComponent<Image>();
        crafredItemImage.sprite = _craftRecipeSO.craftedItemSO.sprite;


        for (int i = 0; i < _craftRecipeSO.ingredientsList.Count; i++)
        {
            Transform ingredientSlotTransform = Instantiate<Transform>(_ingredientSlotVisual, gameObject.transform);
            ingredientSlotTransform.SetParent(_ingredientListVisual);
            Transform requiredIngredientTransform = ingredientSlotTransform.Find(REQUIRED_INGREDIENT);
            requiredIngredientTransform.SetParent(ingredientSlotTransform);
            Image requiredIngredientImage = requiredIngredientTransform.GetComponent<Image>();
            TextMeshProUGUI requiredIngredientAmount = requiredIngredientTransform.Find(AMOUNT_VISUAL).GetComponent<TextMeshProUGUI>();
            requiredIngredientImage.sprite = _craftRecipeSO.ingredientsList[i].requiredItemSO.sprite;
            requiredIngredientAmount.text = _craftRecipeSO.ingredientsList[i].requiredAmount.ToString();
        }

        _ingredientListVisual.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ingredientListVisual.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ingredientListVisual.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canCraftRecipe)
        {
            foreach (CraftRecipeSO.Ingredient ingredient in _craftRecipeSO.ingredientsList)
            {
                int amountToRemove = ingredient.requiredAmount;

                foreach (InventorySlot inventorySlot in InventoryManager.Instance.GetInventorySlotArray())
                {
                    InventoryItem inventoryItem = inventorySlot.GetComponentInChildren<InventoryItem>();

                    if (inventoryItem != null && inventoryItem.ItemSO.GetSpecificItemType().Equals(ingredient.requiredItemSO.GetSpecificItemType()))
                    {
                        for (int i = 0; i < ingredient.requiredAmount; i++)
                        {
                            int amountRemoved = Mathf.Min(inventoryItem.amount, amountToRemove);
                            inventoryItem.amount -= amountRemoved;
                            amountToRemove -= amountRemoved;

                            if (inventoryItem.amount <= 0)
                            {
                                Destroy(inventoryItem.gameObject);
                            } else
                            {
                                inventoryItem.RefreshAmount();
                            }

                            if (amountToRemove <= 0)
                            {
                                break;
                            }
                        }
                    }
                    if (amountToRemove <= 0)
                    {
                        break;
                    }
                }
            }

            InventoryManager.Instance.AddInventoryItem(_craftRecipeSO.craftedItemSO, _craftRecipeSO.craftedItemSO.item, 1, 55);
        }
    }
}
