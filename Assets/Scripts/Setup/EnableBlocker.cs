using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnableBlocker : MonoBehaviour
{
    private SingleNodeBlocker blocker;

    private void Start()
    {
        blocker = GetComponent<SingleNodeBlocker>();
        blocker.manager = FindObjectOfType<BlockManager>();
        UpdateBlocker();
    }

    public void UpdateBlocker()
    {
        blocker.BlockAtCurrentPosition();
    }
}