using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static Pathfinding.BlockManager;

public class EnemyPathfinding : MonoBehaviour
{
    private BlockManager blockManager;
    private List<SingleNodeBlocker> obstacles;
    private Transform target;
    private TraversalProvider traversalProvider;
    private SingleNodeBlocker blocker;
    private ABPath path;

    [SerializeField]
    private float enemyRange;

    private void Start()
    {
        blocker = GetComponent<SingleNodeBlocker>();
        blockManager = FindObjectOfType<BlockManager>();

        blocker.manager = blockManager;
        blocker.BlockAtCurrentPosition();

        target = FindObjectOfType<PlayerMovement>().transform;

        CalculateObstacles();
    }

    private void CalculateObstacles()
    {
        //gotta be a better way to do this...
        obstacles = new List<SingleNodeBlocker>();
        obstacles.AddRange(FindObjectsOfType<SingleNodeBlocker>());
        obstacles.Remove(blocker);
        traversalProvider = new TraversalProvider(blockManager, BlockMode.OnlySelector, obstacles);
    }

    public void AttemptMove()
    {
        CalculateObstacles();
        path = ABPath.Construct(transform.position, target.position, null);
        path.traversalProvider = traversalProvider;

        AstarPath.StartPath(path);
        path.BlockUntilCalculated();

        if (path.error)
        {
            //move around randomly? kitty is hidden
            Debug.Log("No path was found");
        }
        else
        {
            if (path.GetTotalLength() > 1f && path.GetTotalLength() < enemyRange)
            {
                transform.position = path.vectorPath[1];
                //add animation to show it is tracking player
            }
            else
            {
                //move around randomly
            }
        }
        blocker.BlockAtCurrentPosition();
    }

    public void Update()
    {
        if (path != null)
        {
            for (int i = 0; i < path.vectorPath.Count - 1; i++)
            {
                Debug.DrawLine(path.vectorPath[i], path.vectorPath[i + 1], Color.cyan);
            }
        }
    }
}