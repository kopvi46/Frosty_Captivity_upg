using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapGenericVisual : MonoBehaviour
{
    private MyGrid<HeatMapGridObject> _myGrid;
    private Mesh _mesh;
    private bool _updateMesh;

    private void Awake()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    public void SetGrid(MyGrid<HeatMapGridObject> myGrid)
    {
        _myGrid = myGrid;
        UpdateHeatMapVisual();

        myGrid.OnGridValueChanged += MyGrid_OnGridValueChanged;
    }

    private void MyGrid_OnGridValueChanged(object sender, MyGrid<HeatMapGridObject>.OnGridValueChangedEventArgs e)
    {
        _updateMesh = true;
    }

    private void LateUpdate()
    {
        if (_updateMesh)
        {
            _updateMesh = false;

            UpdateHeatMapVisual();
        }
    }

    private void UpdateHeatMapVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(_myGrid.Width * _myGrid.Depth, out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < _myGrid.Width; x++)
        {
            for (int z = 0; z < _myGrid.Depth; z++)
            {
                int index = x * _myGrid.Depth + z;
                Vector3 quadSize = new Vector3(1, 0, 1) * _myGrid.CellSize;

                HeatMapGridObject gridObject = _myGrid.GetGridObject(x, z);
                float gridValueNormalized = gridObject.GetValueNormalized();
                Vector2 myGridValueUV = new Vector2(gridValueNormalized, 0f);

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, _myGrid.GetWorldPosition(x, z) + quadSize * .5f, 0f, quadSize, myGridValueUV, myGridValueUV);
            }
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
    }
}
