using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class MyGrid
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
    private int[,] _gridArray;

    public int Width { get; private set; }
    public int Depth { get; private set; }
    public float CellSize { get; private set; }

    public MyGrid(int width, int depth, float cellSize, int fontSize, Vector3 originPosition)
    {
        Width = width;
        Depth = depth;
        CellSize = cellSize;
        _fontSize = fontSize;
        _originPosition = originPosition;

        _gridArray = new int[width, depth];

        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, depth];

            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < _gridArray.GetLength(1); z++)
                {
                    debugTextArray[x, z] = CreateWorldText(_gridArray[x, z].ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, fontSize, Color.black, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.black, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.black, 100f);

                }
            }
            Debug.DrawLine(GetWorldPosition(0, depth), GetWorldPosition(width, depth), Color.black, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, depth), Color.black, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.z].text = _gridArray[eventArgs.x, eventArgs.z].ToString();
            };
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * CellSize + _originPosition;
    }

    private void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / CellSize);
        z = Mathf.FloorToInt((worldPosition - _originPosition).z / CellSize);
    }

    private void SetValue(int x, int z, int value)
    {
        if (x >= 0 && z >= 0 && x < Width && z < Depth)
        {
            _gridArray[x, z] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetValue(x, z, value);
    }

    private int GetValue(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < Width && z < Depth)
        {
            return _gridArray[x, z];
        } else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetValue(x, z);
    }

    private TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);

    }

    private TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;

        transform.localEulerAngles = new Vector3(90, 0, 0);
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}
