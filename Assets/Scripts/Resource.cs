using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour, IInteractable
{
    [SerializeField] private ResourceSO resourceSO;

    private int interactionCount;

    public void Interact()
    {
        Debug.Log($"Player interacted with {resourceSO.name} !");
        
        interactionCount++;

        if (interactionCount >= resourceSO.obtainProgressMax)
        {
            Destroy(gameObject);

            Transform itemTransform = Instantiate(resourceSO.itemSO.prefab, transform.position, transform.rotation);
        }
    }
}
