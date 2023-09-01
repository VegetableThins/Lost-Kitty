using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerMovement : MonoBehaviour
{
    public Transform movePoint;
    public float moveSpeed = 10f;
    public LayerMask whatStopsMovement;

    private GameController gameController;

    private JoystickMovement joystickMovement;

    private void Start()
    {
        movePoint.parent = null;
        gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (gameController.playersTurn)
            AttemptMove();
    }

    private void AttemptMove()
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.5f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1 && Input.GetButtonDown("Vertical"))
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.1f, whatStopsMovement))
                {
                    movePoint.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical")));
                    gameController.m_TurnEvent.Invoke();
                }
                else
                {
                    HandleIntersection("Vertical");
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1 && Input.GetButtonDown("Horizontal"))
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.1f, whatStopsMovement))
                {
                    movePoint.Translate(new Vector3(Input.GetAxisRaw("Horizontal"), 0f));
                    gameController.m_TurnEvent.Invoke();
                }
                else
                {
                    HandleIntersection("Horizontal");
                }
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            gameController.m_TurnEvent.Invoke();
        }
    }

    private void HandleIntersection(string axis)
    {
        //redo this somehow....
        if (axis == "Vertical")
        {
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.1f, whatStopsMovement).gameObject.tag == "Enemy")
            {
                Health enemyHealth = Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.1f, whatStopsMovement).gameObject.GetComponentInParent<Health>();
                Attack(enemyHealth);
                gameController.m_TurnEvent.Invoke();
            }
        }
        else
        {
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.1f, whatStopsMovement).gameObject.tag == "Enemy")
            {
                Health enemyHealth = Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.1f, whatStopsMovement).gameObject.GetComponentInParent<Health>();
                Attack(enemyHealth);
                gameController.m_TurnEvent.Invoke();
            }
        }
    }

    private void Attack(Health health)
    {
        Debug.Log("Kitty Scratches for " + GetComponent<DamageDealer>().GetDamage());
        health.DecreaseHealth(GetComponent<DamageDealer>().GetDamage());
    }

    private void OnDestroy()
    {
        GetComponentInChildren<Camera>().transform.parent = null;

        if(GetComponent<Health>().isAlive == false)
            gameController.GameOver();
        //show deeth scene here
    }
}