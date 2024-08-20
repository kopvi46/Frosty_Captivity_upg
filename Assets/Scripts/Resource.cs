using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Resource : MonoBehaviour, IInteractable
{
    [SerializeField] private ResourceSO resourceSO;

    private int interactionCount;

    public void Interact(Player player, Inventory inventory)
    {
        //Debug.Log($"Player interacted with {resourceSO.name} !");
        
        interactionCount++;

        if (interactionCount >= resourceSO.obtainProgressMax)
        {
            Destroy(gameObject);

            for (int i = 0; i < resourceSO.spawnItemAmount; i++)
            {
                SpawnItem();
            }
        }
    }

    private void SpawnItem()
    {
        float spawnRadius = 2f;

        Vector3 directionFromPlayer = (transform.position - Player.Instance.transform.position).normalized;

        Vector3 randomOffset = Vector3.Cross(directionFromPlayer, Vector3.up).normalized * Random.Range(-spawnRadius, spawnRadius);

        Vector3 spawnPosition = transform.position + directionFromPlayer * spawnRadius + randomOffset;

        float randomRotation = Random.Range(0f, 360f);
        Quaternion spawnRotation = Quaternion.Euler(0, randomRotation, 0);

        Transform itemTransform = Instantiate(resourceSO.itemSO.prefab, spawnPosition, spawnRotation);

        //Item newItem = itemTransform.GetComponent<Item>();
        //newItem.SetItemAmountVisual();
    }
}
