using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceHeatZone : MonoBehaviour
{
    public static FireplaceHeatZone Instance {  get; private set; }

    private bool isPlayerTriggered = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            isPlayerTriggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            isPlayerTriggered = false;
        }
    }

    public bool IsPlayerTriggered()
    {
        return isPlayerTriggered;
    }
}
