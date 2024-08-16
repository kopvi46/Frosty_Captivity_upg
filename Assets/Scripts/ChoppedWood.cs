using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedWood : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Player interacted with Chopped Wood!");
    }
}
