using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private MyGrid<PathNode> _myGrid;
    private List<PathNode> _openList;
    private List<PathNode> _closedList;

    public Pathfinding(int width, int depth, Vector3 position)
    {
        _myGrid = new MyGrid<PathNode>(width, depth, 4f, 150, position, (MyGrid<PathNode> g, int x, int z) => new PathNode(g, x, z));
    }

    public MyGrid<PathNode> GetGrid()
    {
        return _myGrid;
    }

    public List<PathNode> FindPath(int startX, int startZ, int endX, int endZ)
    {
        PathNode startNode = _myGrid.GetGridObject(startX, startZ);
        PathNode endNode = _myGrid.GetGridObject(endX, endZ);

        _openList = new List<PathNode> { startNode };
        _closedList = new List<PathNode>();

        for (int x = 0; x < _myGrid.Width; x++)
        {
            for (int z = 0; z < _myGrid.Depth; z++)
            {
                PathNode pathNode = _myGrid.GetGridObject(x, z);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (_openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(_openList);

            if (currentNode == endNode)
            {
                //Reached final node
                return CalculatePath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighboursList(currentNode))
            {
                if (_closedList.Contains(neighbourNode)) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!_openList.Contains(neighbourNode))
                    {
                        _openList.Add(neighbourNode);
                    }
                }
            }
        }

        //Out of nodes on the openList
        return null;
    }

    private List<PathNode> GetNeighboursList(PathNode currentNode)
    {
        List<PathNode> neighboursList = new List<PathNode>();

        if (currentNode.x - 1 >= 0)
        {
            //Left
            neighboursList.Add(GetNode(currentNode.x - 1, currentNode.z));
            //Left down
            if (currentNode.z - 1 >= 0) neighboursList.Add(GetNode(currentNode.x - 1, currentNode.z - 1));
            //Left up
            if (currentNode.z + 1 < _myGrid.Depth) neighboursList.Add(GetNode(currentNode.x - 1, currentNode.z + 1));
        }
        if (currentNode.x + 1 < _myGrid.Width)
        {
            //Right
            neighboursList.Add(GetNode(currentNode.x + 1, currentNode.z));
            //Right down
            if (currentNode.z - 1 >= 0) neighboursList.Add(GetNode(currentNode.x + 1, currentNode.z - 1));
            //Right up
            if (currentNode.z + 1 < _myGrid.Depth) neighboursList.Add(GetNode(currentNode.x + 1, currentNode.z + 1));
        }
        //Down
        if (currentNode.z - 1 >= 0) neighboursList.Add(GetNode(currentNode.x, currentNode.z - 1));
        //Up
        if (currentNode.z + 1 <= _myGrid.Depth) neighboursList.Add(GetNode(currentNode.x, currentNode.z + 1));

        return neighboursList;
    }

    private PathNode GetNode(int x, int z)
    {
        return _myGrid.GetGridObject(x, z);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int zDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;

    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
}