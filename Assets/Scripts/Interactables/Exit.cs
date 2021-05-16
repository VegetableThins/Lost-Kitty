using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Exit : MonoBehaviour
{
    private GameController gameController;
    public float exitDelay = 1f;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerMovePoint")
        {
            StartCoroutine(gameController.LoadNextLevel(exitDelay));
        }
    }
}