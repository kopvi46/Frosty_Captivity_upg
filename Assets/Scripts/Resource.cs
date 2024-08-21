using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class Resource : MonoBehaviour, IInteractable
{
    [SerializeField] private ResourseSO resourceSO;

    public void Interact(Player player)
    {
        
    }
}
