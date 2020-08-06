using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform movePoint;
    public float moveSpeed = 5f;
    public LayerMask whatStopsMovement;
    BoxCollider2D boxCollider2D;

    GameController gameController;

    PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        gameController = FindObjectOfType<GameController>();
        player = FindObjectOfType<PlayerMovement>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        var blocker = GetComponent<SingleNodeBlocker>();
        blocker.BlockAtCurrentPosition();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        
    }

    public void Move()
    {
        //allow ability to change enemy movement speed
        //pick random direction top, down, left right
        //check if the direction is moveable with an overlap circle
        //if the enemy 'sees' the player follow it?
        //show where the enemy will move next?
        
        

        
        
    }

    public IEnumerator AttemptMove()
    {
        //disable player movement
        yield return new WaitForSeconds(0.1f);

        //allow ability to change enemy movement speed
        //pick random direction top, down, left right
        //check if the direction is moveable with an overlap circle
        //if the enemy 'sees' the player follow it?
        //show where the enemy will move next?

        int xDir = 0;
        int yDir = 0;

        //adjust movement
        //enemy not attacking or moving vertically properly
        //if (Mathf.Abs(player.gameObject.transform.position.x - transform.position.x) < float.Epsilon)
        //    yDir = player.gameObject.transform.position.y > transform.position.y ? 1 : -1;
        //else
        //    xDir = player.gameObject.transform.position.x > transform.position.x ? 1 : -1;

        //Vector2 start = transform.position;
        //Vector2 end = start + new Vector2(xDir, yDir);

        //RaycastHit2D hit = Physics2D.Linecast(start, end, whatStopsMovement);

        //Debug.Log(hit.transform);

        //if (hit.transform == null)
        //{
        //    movePoint.Translate(new Vector2(xDir, yDir));
        //}
        //else if(hit.transform.gameObject.name == "MovePoint")
        //{
        //    Debug.Log(gameObject.name + " bites the player!");
        //}
        //else if(hit.transform.gameObject.name == "Bushes")
        //{
        //    //Debug.Log(gameObject.name + " ran into a bush");
        //}
        //else
        //{
        //    Debug.Log("dog derps??");
        //}
    }
}
