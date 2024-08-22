using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Craft Recipe")]
public class CraftRecipeSO : ScriptableObject
{
    public List<Ingredient> ingredientsList;
    public ItemSO craftedItemSO;
    public Item craftedItem;

    [System.Serializable] public struct Ingredient
    {
        public ItemSO requiredItemSO;
        public Item requiredItem;
        public int requiredAmount;
    }
}
