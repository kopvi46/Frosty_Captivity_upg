using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ObtainableResourse : MonoBehaviour, IInteractable
{
    [SerializeField] private ObtainableResourseSO _ObtainableResourseSO;

    private int _interactionCount;

    public void Interact(Player player)
    {
        _interactionCount++;

        if (_interactionCount >= _ObtainableResourseSO.obtainProgressMax)
        {
            Destroy(gameObject);

            for (int i = 0; i < _ObtainableResourseSO.spawnItemAmount; i++)
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

        Transform itemTransform = Instantiate(_ObtainableResourseSO.itemSO.prefab, spawnPosition, spawnRotation);
    }
}
