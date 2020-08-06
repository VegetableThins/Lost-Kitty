using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestObstacle : MonoBehaviour
{
    // Start is called before the first frame update
    SingleNodeBlocker blocker;

    void Start()
    {
        blocker = GetComponent<SingleNodeBlocker>();
        blocker.BlockAtCurrentPosition();
    }
}
