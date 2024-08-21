using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CraftRecipeSO : ScriptableObject
{
    public string recipeName;
    public List<Ingredient> ingredientsList;
    //public ResourseSO craftedItemSO;
    //public Resource craftedItem;

    [System.Serializable] public struct Ingredient
    {
        public ResourseSO requiredIResourceSO;
        public Resource requiredResource;
        public int requiredAmount;
    }
}
