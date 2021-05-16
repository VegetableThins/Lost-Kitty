using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SetupPathfinder : MonoBehaviour
{
    private BoardGenerator board;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        board = FindObjectOfType<BoardGenerator>();
        var graph = (GridGraph)AstarPath.active.data.graphs[0];
        graph.SetDimensions(board.width, board.height, 1f);
        graph.center = new Vector3(((float)board.width / 2) - 0.5f, ((float)board.height / 2) - 0.5f);
        AstarPath.active.Scan();
    }
}