using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{
    private MyGrid myGrid;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(MyGrid myGrid)
    {
        this.myGrid = myGrid;
        UpdateHeatMapVisual();

        myGrid.OnGridValueChanged += MyGrid_OnGridValueChanged;
    }

    private void MyGrid_OnGridValueChanged(object sender, MyGrid.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;

            UpdateHeatMapVisual();
        }
    }

    private void UpdateHeatMapVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(myGrid.Width * myGrid.Depth, out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < myGrid.Width; x++)
        {
            for (int z = 0; z < myGrid.Depth; z++)
            {
                int index = x * myGrid.Depth + z;
                Vector3 quadSize = new Vector3(1, 0, 1) * myGrid.CellSize;

                //Vector2 uv00 = new Vector2(0, 0);
                //Vector2 uv11 = new Vector2(1, 1);

                int myGridValue = myGrid.GetValue(x, z);
                float gridValueNormalized = (float)myGridValue / MyGrid.HEAT_MAP_MAX_VALUE;
                Vector2 myGridValueUV = new Vector2(gridValueNormalized, 0f);

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, myGrid.GetWorldPosition(x, z) + quadSize * .5f, 0f, quadSize, myGridValueUV, myGridValueUV);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    
}
