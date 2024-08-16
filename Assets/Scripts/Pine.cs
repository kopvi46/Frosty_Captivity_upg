using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pine : MonoBehaviour, IInteractable
{
    public void Interact(Player player)
    {
        Debug.Log("Player interacted with Tree!");
    }
}
