using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private HeatMapVisual heatMapVisual;

    private MyGrid myGrid;

    private void Start()
    {
        myGrid = new MyGrid(10, 5, 4f, 200, transform.position);

        heatMapVisual.SetGrid(myGrid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = GetMouseWorldPosition();
            int value = myGrid.GetValue(position);
            myGrid.SetValue(position, value + 5);
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
