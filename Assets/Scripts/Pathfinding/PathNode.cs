using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private MyGrid<PathNode> _myGrid;
    
    public int x;
    public int z;
    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode cameFromNode;

    public PathNode(MyGrid<PathNode> myGrid, int x, int z)
    {
        _myGrid = myGrid;
        this.x = x;
        this.z = z;
        isWalkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        _myGrid.TriggerGridObjectChanged(x, z);
    }

    public override string ToString()
    {
        return x + ", " + z;
    }
}
