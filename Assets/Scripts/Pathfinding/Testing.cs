using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private MyGrid myGrid;

    private void Start()
    {
        myGrid = new MyGrid(25, 25, 2f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            myGrid.SetValue(GetMouseWorldPosition(), Random.Range(1, 99));
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(myGrid.GetValue(GetMouseWorldPosition()));
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, 0);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            return worldPosition;
        }

        return Vector3.zero;
    }
}
