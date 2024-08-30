using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private HeatMapGenericVisual heatMapGenericVisual;

    private MyGrid<HeatMapGridObject> myGrid;
    private MyGrid<StringGridObject> myStringGrid;

    private void Start()
    {
        //myGrid = new MyGrid<HeatMapGridObject>(20, 10, 4f, 150, transform.position, (MyGrid<HeatMapGridObject> g, int x, int z) => new HeatMapGridObject(g, x, z));
        myStringGrid = new MyGrid<StringGridObject>(20, 10, 4f, 150, transform.position, (MyGrid<StringGridObject> g, int x, int z) => new StringGridObject(g, x, z));

        //heatMapGenericVisual.SetGrid(myGrid);
    }

    private void Update()
    {
        Vector3 position = MyGridUtils.GetMouse3DWorldPosition();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    //myGrid.AddValue(position, 100, 2, 5);
        //    //myGrid.SetValue(position, true);
        //    HeatMapGridObject heatMapGridObject = myGrid.GetGridObject(position);
        //    if (heatMapGridObject != null)
        //    {
        //        heatMapGridObject.AddValue(5);
        //    }
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log(myGrid.GetGridObject(MyGridUtils.GetMouse3DWorldPosition()));
        //}

        if (Input.GetKeyDown(KeyCode.A)) { myStringGrid.GetGridObject(position).AddLetter("A"); }
        if (Input.GetKeyDown(KeyCode.B)) { myStringGrid.GetGridObject(position).AddLetter("B"); }
        if (Input.GetKeyDown(KeyCode.C)) { myStringGrid.GetGridObject(position).AddLetter("C"); }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { myStringGrid.GetGridObject(position).AddLetter("1"); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { myStringGrid.GetGridObject(position).AddLetter("2"); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { myStringGrid.GetGridObject(position).AddLetter("3"); }

    }
}

public class HeatMapGridObject
{
    private const int MIN = 0;
    private const int MAX = 100;

    private MyGrid<HeatMapGridObject> myGrid;
    private int x;
    private int z;
    private int value;

    public HeatMapGridObject(MyGrid<HeatMapGridObject> myGrid, int x, int z)
    {
        this.myGrid = myGrid;
        this.x = x;
        this.z = z;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        myGrid.TriggerGridObjectChanged(x, z);
    }

    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}

public class StringGridObject
{
    private MyGrid<StringGridObject> myGrid;
    private int x;
    private int z;
    private string letters;
    private string numbers;

    public StringGridObject(MyGrid<StringGridObject> myGrid, int x, int z)
    {
        this.myGrid = myGrid;
        this.x = x;
        this.z = z;
        letters = "";
        numbers = "";
    }

    public void AddLetter(string letter)
    {
        letters += letter;
        myGrid.TriggerGridObjectChanged(x, z);
    }

    public void AddNumber(int number)
    {
        numbers += number;
        myGrid.TriggerGridObjectChanged(x, z);
    }

    public override string ToString()
    {
        return letters + "\n" + numbers;
    }
}
