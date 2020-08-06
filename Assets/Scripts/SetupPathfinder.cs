using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SetupPathfinder : MonoBehaviour
{
    private BoardGenerator board;

    private void Start()
    {
        board = FindObjectOfType<BoardGenerator>();
        var graph = (GridGraph)AstarPath.active.data.graphs[0];
        graph.SetDimensions(board.width, board.height, 1f);
        graph.center = new Vector3(board.width / 2 - 0.5f, board.width / 2 - 0.5f);
        AstarPath.active.Scan();
    }
}