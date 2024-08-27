using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IHasHealth
{
    public static PlayerHealthManager Instance { get; private set; }

    public event EventHandler<IHasHealth.OnHealthChangedEventArgs> OnHealthChanged;

    private int _playerHealth = 100;
    private float _playerHealthChangeDelay = 3f;
    private float _playerHealtChangeTimer = 0f;

    public int PlayerHealth
    {
        get
        {
            return _playerHealth;
        }
        set
        {
            int playerMinHealth = 0;
            int playerMaxHealth = 100;
            if (value < playerMinHealth)
            {
                _playerHealth = playerMinHealth;
            } else if (value > playerMaxHealth)
            {
                _playerHealth = playerMaxHealth;
            } else
            {
                _playerHealth = value;
            }

            OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs
            {
                healthNormalized = (float)PlayerHealth / playerMaxHealth
            });
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _playerHealtChangeTimer -= Time.deltaTime;

        if (FireplaceHeatZone.Instance.IsPlayerTriggered())
        {
            if (_playerHealtChangeTimer < 0)
            {
                _playerHealtChangeTimer = _playerHealthChangeDelay;
                PlayerHealth += 10;
            }
        } else
        {
            if (PlayerInventory.Instance.PlayerLeftHandPoint.childCount != 0
                && PlayerInventory.Instance.PlayerLeftHandPoint.GetComponentInChildren<Item>().ItemSO is EquipmentSO equipmentSO
                && equipmentSO.equipmentType == EquipmentSO.EquipmentType.Torch)
            {
                if (_playerHealtChangeTimer < 0)
                {
                    //Debug.Log("Player under torch protection!");
                }
            } else
            {
                if (_playerHealtChangeTimer < 0)
                {
                    _playerHealtChangeTimer = _playerHealthChangeDelay;
                    PlayerHealth -= 10;
                }
            }
        }
    }
}
