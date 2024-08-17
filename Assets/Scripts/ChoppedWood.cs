using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedWood : MonoBehaviour, IInteractable
{
    public void Interact(Player player, Inventory inventory)
    {
        Debug.Log("Player interacted with Chopped Wood!");
    }
}
