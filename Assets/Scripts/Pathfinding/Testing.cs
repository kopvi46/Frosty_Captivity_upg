using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private HeatMapVisual heatMapVisual;

    private MyGrid myGrid;

    private void Start()
    {
        myGrid = new MyGrid(40, 20, 4f, 200, transform.position);

        heatMapVisual.SetGrid(myGrid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = GetMouseWorldPosition();
            myGrid.AddValue(position, 100, 2, 5);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(myGrid.GetValue(GetMouseWorldPosition()));
        }
    }

    // Get Mouse Position in 3D World
    private Vector3 GetMouseWorldPosition()
    {
        //Use this method if it is a 3D game

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, 0);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            return worldPosition;
        }

        return Vector3.zero;
    }

    // Get Mouse Position in 2D World with Z = 0f
    //public static Vector3 GetMouseWorldPosition()
    //{
    //    Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    //    vec.z = 0f;
    //    return vec;
    //}
    //public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    //{
    //    Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
    //    return worldPosition;
    //}
}
