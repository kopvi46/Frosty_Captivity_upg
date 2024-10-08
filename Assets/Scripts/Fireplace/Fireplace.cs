using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour, IHasHealth, IInteractable
{
    public static Fireplace Instance { get; private set; }
    
    public event EventHandler<IHasHealth.OnHealthChangedEventArgs> OnHealthChanged;

    private int _fireplaceHealth = 60;
    private float _fireplaceHealtChangeDelay = 3f;
    private float _fireplaceHealtChangeTimer = 0f;

    public int FireplaceMaxHealth { get; private set; } = 100;
    public int FireplaceHealth
    {
        get
        {
            return _fireplaceHealth;
        }
        set
        {
            int fireplaceMinHealth = 0;
            if (value < fireplaceMinHealth)
            {
                _fireplaceHealth = fireplaceMinHealth;
            } else if (value > FireplaceMaxHealth)
            {
                _fireplaceHealth = FireplaceMaxHealth;
            } else
            {
                _fireplaceHealth = value;
            }

            OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs
            {
                healthNormalized = (float)FireplaceHealth / FireplaceMaxHealth
            });
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _fireplaceHealtChangeTimer -= Time.deltaTime;

        if (_fireplaceHealtChangeTimer < 0 )
        {
            _fireplaceHealtChangeTimer = _fireplaceHealtChangeDelay;
            FireplaceHealth -= 1;
        }
    }

    public void RestoreFireplaceHealt(InventoryItem inventoryItem) 
    {
        MaterialSO materialSO = inventoryItem.ItemSO as MaterialSO;
        bool isNeedToReturnRest = false;
        int amountToReturn = inventoryItem.amount;

        if (materialSO.fireRestoraion > 0)
        {
            for (int i = 0; i < inventoryItem.amount; i++)
            {
                FireplaceHealth += materialSO.fireRestoraion;

                if (FireplaceHealth >= FireplaceMaxHealth)
                {
                    isNeedToReturnRest = true;
                    amountToReturn -= i + 1;
                    break;
                }
            }

        } else
        {
            isNeedToReturnRest = true;
        }

        if (isNeedToReturnRest)
        {
            InventoryManager.Instance.AddInventoryItem(materialSO, inventoryItem.ItemSO.item, amountToReturn, inventoryItem.durability);
        }
    }

    public void Interact(Player player)
    {
        //Not implemented yet
    }
}
