using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class MyGrid<TGridObject>
{
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    private int _fontSize;
    private Vector3 _originPosition;
    private TGridObject[,] _gridArray;

    public int Width { get; private set; }
    public int Depth { get; private set; }
    public float CellSize { get; private set; }

    public MyGrid(int width, int depth, float cellSize, int fontSize, Vector3 originPosition, Func<MyGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        Width = width;
        Depth = depth;
        CellSize = cellSize;
        _fontSize = fontSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[width, depth];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridArray.GetLength(1); z++)
            {
                _gridArray[x, z] = createGridObject(this, x, z);
            }
        }

                bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, depth];

            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < _gridArray.GetLength(1); z++)
                {
                    debugTextArray[x, z] = MyGridUtils.CreateWorldText(_gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, fontSize, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);

                }
            }
            Debug.DrawLine(GetWorldPosition(0, depth), GetWorldPosition(width, depth), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, depth), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.z].text = _gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * CellSize + _originPosition;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / CellSize);
        z = Mathf.FloorToInt((worldPosition - _originPosition).z / CellSize);
    }

    private void SetGridObject(int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < Width && z < Depth)
        {
            _gridArray[x, z] = value;
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
        }
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetGridObject(x, z, value);
    }

    public TGridObject GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < Width && z < Depth)
        {
            return _gridArray[x, z];
        } else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }

    //Specific methods for heatMap
    //public void AddValue(int x, int z, int value)
    //{
    //    SetValue(x, z, GetValue(x, z) + value);
    //}

    //public void AddValue(Vector3 worldPosition, int value, int fullValueRange, int totalRange)
    //{
    //    int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));

    //    GetXZ(worldPosition, out int originX, out int originZ);
    //    for (int x = 0; x < totalRange; x++)
    //    {
    //        for (int z = 0; z < totalRange - x; z++)
    //        {
    //            int radius = x + z;
    //            int addValueAmount = value;

    //            if (radius > fullValueRange)
    //            {
    //                addValueAmount -= lowerValueAmount * (radius - fullValueRange);
    //            }

    //            AddValue(originX + x, originZ + z, addValueAmount);

    //            if (x != 0)
    //            {
    //                AddValue(originX - x, originZ + z, addValueAmount);
    //            }
    //            if (z != 0)
    //            {
    //                AddValue(originX + x, originZ - z, addValueAmount);
    //                if (x != 0)
    //                {
    //                    AddValue(originX - x, originZ - z, addValueAmount);
    //                }
    //            }

    //        }
    //    }
    //}
}
