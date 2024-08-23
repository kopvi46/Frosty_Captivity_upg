using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObjectVisual : MonoBehaviour
{
    private IInteractable interactable;

    [SerializeField] private GameObject _interactableGameObject;
    [SerializeField] private GameObject[] _visualGameObjectArray;
    private void Start()
    {
        interactable = _interactableGameObject.GetComponent<IInteractable>();
        Player.Instance.OnSelectedObjectChanged += Player_OnSelectedObjectChanged;
    }

    private void Player_OnSelectedObjectChanged(object sender, Player.OnSelectedObjectChangedEventArgs e)
    {
        if (e.selectedObject == interactable)
        {
            Show();
        } else
        {
            Hide();
        }
    }

    private void OnDestroy()
    {
        Player.Instance.OnSelectedObjectChanged -= Player_OnSelectedObjectChanged;
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in _visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in _visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
