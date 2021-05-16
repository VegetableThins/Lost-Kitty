using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static Pathfinding.BlockManager;

public class EnemyPathfinding : MonoBehaviour
{
    private List<SingleNodeBlocker> obstacles;
    private TraversalProvider traversalProvider;
    private SingleNodeBlocker blocker;
    private ABPath path;

    public BlockManager blockManager;
    public Transform target;
    public Transform movingToPoint;

    public float enemyRange;
    public float moveTime = 0.05f;
    public float moveSpeed = 10f;
    public int turnSpeed;

    private int turnSpeedCounter = 1;

    private void Start()
    {
        GameController.instance.AddEnemyToList(this);

        blocker = GetComponent<SingleNodeBlocker>();

        blocker.BlockAtCurrentPosition();

        movingToPoint.parent = null;

        CalculateObstacles();
    }

    private void OnDestroy()
    {
        GameController.instance.RemoveEnemyToList(this);
    }

    private void CalculateObstacles()
    {
        blockManager = FindObjectOfType<BlockManager>();
        blocker.manager = blockManager;

        obstacles = new List<SingleNodeBlocker>();
        obstacles.AddRange(FindObjectsOfType<SingleNodeBlocker>());
        obstacles.Remove(blocker);
        traversalProvider = new TraversalProvider(blockManager, BlockMode.OnlySelector, obstacles);
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void AttemptMove()
    {
        if (turnSpeed != turnSpeedCounter)
        {
            turnSpeedCounter++;
            return;
        }

        turnSpeedCounter = 1;

        CalculateObstacles();
        path = ABPath.Construct(movingToPoint.position, target.position, null);
        path.traversalProvider = traversalProvider;

        AstarPath.StartPath(path);
        path.BlockUntilCalculated();

        if (!path.error)
        {
            if (path.vectorPath.Count > 2)
            {
                movingToPoint.position = path.vectorPath[1];
                blocker.BlockAt(path.vectorPath[1]);
                path.vectorPath.RemoveAt(0);
            }
            else if (path.vectorPath.Count <= 2 && target == FindObjectOfType<PlayerMovePoint>().transform)
            {
                //ATTACK PLAYER
                Health playerHealth = FindObjectOfType<PlayerMovement>().GetComponent<Health>();
                playerHealth.DecreaseHealth(GetComponent<DamageDealer>().GetDamage());
            }
        }
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movingToPoint.position, moveSpeed * Time.deltaTime);

        //if (path != null)
        //{
        //    for (int i = 0; i < path.vectorPath.Count - 1; i++)
        //    {
        //        Debug.DrawLine(path.vectorPath[i], path.vectorPath[i + 1], Color.cyan);
        //    }
        //}
    }
}