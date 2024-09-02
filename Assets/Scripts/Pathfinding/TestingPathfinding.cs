using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPathfinding : MonoBehaviour
{
    //[SerializeField] private PathfindingVisual pathfindingVisual;

    //private Pathfinding pathfinding;
    
    //private void Start()
    //{
    //    pathfinding = new Pathfinding(25, 25, transform.position);
    //    pathfindingVisual.SetGrid(pathfinding.GetGrid());
    //}

    //private void Update()
    //{
    //    //if (Input.GetMouseButtonDown(0))
    //    //{
    //    //    Vector3 mouseWorldPosition = MyGridUtils.GetMouse3DWorldPosition();
    //    //    pathfinding.GetGrid().GetXZ(mouseWorldPosition, out int x, out int z);
    //    //    List<PathNode> path = pathfinding.FindPath(0, 0, x, z);
    //    //    if (path != null)
    //    //    {
    //    //        for (int i = 0; i < path.Count - 1; i++)
    //    //        {
    //    //            //Debug.DrawLine(new Vector3(path[i].x, path[i].z) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].z) * 10f + Vector3.one * 5f, Color.green);
    //    //            Vector3 startPosition = pathfinding.GetGrid().GetWorldPosition(path[i].x, path[i].z) + new Vector3(pathfinding.GetGrid().CellSize / 2, 0.5f, pathfinding.GetGrid().CellSize / 2);
    //    //            Vector3 endPosition = pathfinding.GetGrid().GetWorldPosition(path[i + 1].x, path[i + 1].z) + new Vector3(pathfinding.GetGrid().CellSize / 2, 0.5f, pathfinding.GetGrid().CellSize / 2);
    //    //            Debug.DrawLine(startPosition, endPosition, Color.green, 20f);
    //    //        }
    //    //    }
    //    //}
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        Vector3 mouseWorldPosition = MyGridUtils.GetMouse3DWorldPosition();
    //        pathfinding.GetGrid().GetXZ(mouseWorldPosition, out int x, out int z);
    //        pathfinding.GetNode(x, z).SetIsWalkable(!pathfinding.GetNode(x, z).isWalkable);
    //    }
    //}
}
