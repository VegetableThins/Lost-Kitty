using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Exit : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Player")
            Debug.Log("Exiting Level...");
    }
}