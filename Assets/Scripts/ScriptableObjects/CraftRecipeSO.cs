using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CraftRecipeSO : ScriptableObject
{
    public string recipeName;
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
