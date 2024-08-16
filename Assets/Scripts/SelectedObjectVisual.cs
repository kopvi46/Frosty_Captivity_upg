using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObjectVisual : MonoBehaviour
{
    //[SerializeField] private IInteractable interactable;
    [SerializeField] private GameObject interactableGameObject;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private IInteractable interactable;
    private void Start()
    {
        interactable = interactableGameObject.GetComponent<IInteractable>();
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
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
