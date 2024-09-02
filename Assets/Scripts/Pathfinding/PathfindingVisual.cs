using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingVisual : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int depth;
    [SerializeField] private float cellSize;
    [SerializeField] private int fontSize;

    private Pathfinding pathfinding;
    private MyGrid<PathNode> myGrid;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Start()
    {
        pathfinding = new Pathfinding(width, depth, cellSize, fontSize, transform.position);
        SetGrid(pathfinding.GetGrid());
    }

    public void SetGrid(MyGrid<PathNode> myGrid)
    {
        this.myGrid = myGrid;
        UpdateHeatMapVisual();

        myGrid.OnGridValueChanged += MyGrid_OnGridValueChanged;
    }

    private void MyGrid_OnGridValueChanged(object sender, MyGrid<PathNode>.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = MyGridUtils.GetMouse3DWorldPosition();
            pathfinding.GetGrid().GetXZ(mouseWorldPosition, out int x, out int z);
            pathfinding.GetNode(x, z).SetIsWalkable(!pathfinding.GetNode(x, z).isWalkable);
        }
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

                PathNode pathNode = myGrid.GetGridObject(x, z);

                if (pathNode.isWalkable)
                {
                    quadSize = Vector3.zero;
                }

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, (myGrid.GetWorldPosition(x, z) + quadSize * .5f) - transform.position, 0f, quadSize, Vector2.zero, Vector2.zero);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
