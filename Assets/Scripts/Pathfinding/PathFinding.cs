using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pathfinding
{
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private MyGrid<PathNode> _myGrid;
    private List<PathNode> _openList;
    private List<PathNode> _closedList;

    public Pathfinding(int width, int depth, Vector3 position)
    {
        Instance = this;
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

        if (startNode == null || endNode == null)
        {
            return null;
        }

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
                if (!neighbourNode.isWalkable)
                {
                    _closedList.Add(neighbourNode);
                    continue;
                }

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

    public List<Vector3> FindPath(Vector3 startWorldPosotion, Vector3 endWorldPosotion)
    {
        _myGrid.GetXZ(startWorldPosotion, out int startX, out int startZ);
        _myGrid.GetXZ(endWorldPosotion, out int endX, out int endZ);

        List<PathNode> path = FindPath(startX, startZ, endX, endZ);
        if (path == null)
        {
            return null;
        } else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.z) * _myGrid.CellSize + Vector3.one * _myGrid.CellSize * .5f);
            }
            return vectorPath;
        }
    }

    private List<PathNode> GetNeighboursList(PathNode currentNode)
    {
        List<PathNode> neighboursList = new List<PathNode>();

        //Check left neighbours
        if (currentNode.x - 1 >= 0)
        {
            TryAddNeighbour(neighboursList, currentNode.x - 1, currentNode.z);// Left
            TryAddNeighbour(neighboursList, currentNode.x - 1, currentNode.z - 1); // Left down
            TryAddNeighbour(neighboursList, currentNode.x - 1, currentNode.z + 1); // Left up
        }

        //Check right neighbours
        if (currentNode.x + 1 < _myGrid.Width)
        {
            TryAddNeighbour(neighboursList, currentNode.x + 1, currentNode.z);// Right
            TryAddNeighbour(neighboursList, currentNode.x + 1, currentNode.z - 1); // Right down
            TryAddNeighbour(neighboursList, currentNode.x + 1, currentNode.z + 1); // Right up
        }

        //Check down neighbours
        TryAddNeighbour(neighboursList, currentNode.x, currentNode.z - 1); // Down

        //Check up neighbours
        TryAddNeighbour(neighboursList, currentNode.x, currentNode.z + 1); // Up

        return neighboursList;
    }

    private void TryAddNeighbour(List<PathNode> neighboursList, int x, int z)
    {
        if (x >= 0 && z >= 0 && x < _myGrid.Width && z < _myGrid.Depth)
        {
            PathNode neighbourNode = GetNode(x, z);
            if (neighbourNode != null)
            {
                neighboursList.Add(neighbourNode);
            } else
            {
                Debug.LogError($"Neighbour is null for node: {x}, {z}");
            }
        }
    }

    public PathNode GetNode(int x, int z)
    {
        if (_myGrid.GetGridObject(x, z) == null) Debug.LogError("Neighbour is null for node: " + x + "-" + z);
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