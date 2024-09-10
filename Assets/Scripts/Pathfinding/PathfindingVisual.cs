using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingVisual : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _depth;
    [SerializeField] private float _cellSize;
    [SerializeField] private int _fontSize;
    [SerializeField] private LayerMask _obstacleLayerMask;

    private Pathfinding _pathfinding;
    private MyGrid<PathNode> _myGrid;
    private Mesh _mesh;
    private bool _updateMesh;

    private void Awake()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    private void Start()
    {
        _pathfinding = new Pathfinding(_width, _depth, _cellSize, _fontSize, transform.position);
        SetGrid(_pathfinding.GetGrid());

        for (int x = 0; x < _myGrid.Width; x++)
        {
            for (int z = 0; z <  _myGrid.Depth; z++)
            {
                Vector3 nodeWorldPosition = _myGrid.GetWorldPosition(x, z) + new Vector3(_myGrid.CellSize, 0, _myGrid.CellSize) * .5f;

                if (Physics.CheckBox(nodeWorldPosition, new Vector3(_myGrid.CellSize * .5f, .5f, _myGrid.CellSize * .5f), Quaternion.identity, _obstacleLayerMask))
                {
                    _myGrid.GetGridObject(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public void SetGrid(MyGrid<PathNode> myGrid)
    {
        this._myGrid = myGrid;
        UpdateHeatMapVisual();

        myGrid.OnGridValueChanged += MyGrid_OnGridValueChanged;
    }

    private void MyGrid_OnGridValueChanged(object sender, MyGrid<PathNode>.OnGridValueChangedEventArgs e)
    {
        _updateMesh = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = MyUtils.GetMouse3DWorldPosition();
            _pathfinding.GetGrid().GetXZ(mouseWorldPosition, out int x, out int z);
            _pathfinding.GetNode(x, z).SetIsWalkable(!_pathfinding.GetNode(x, z).isWalkable);
        }
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

                PathNode pathNode = _myGrid.GetGridObject(x, z);

                if (pathNode.isWalkable)
                {
                    quadSize = Vector3.zero;
                }

                //MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, (_myGrid.GetWorldPosition(x, z) + quadSize * .5f) - transform.position, 0f, quadSize, Vector2.zero, Vector2.zero);
            }
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
    }
}
