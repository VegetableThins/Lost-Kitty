using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    private EnemyPathfinding[] enemies;
    private int _turnCounter;

    private BoardGenerator board;

    private int level = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = FindObjectsOfType<EnemyPathfinding>();
        board = GetComponent<BoardGenerator>();
        InitGame();
    }

    private void OnLevelWasLoaded(int index)
    {
        level++;

        InitGame();
    }

    public int TurnCounter
    {
        get { return _turnCounter; }
        set
        {
            _turnCounter = value;
            HandleTurn();
        }
    }

    private void InitGame()
    {
        board = FindObjectOfType<BoardGenerator>();
        board.SetupScene(1);
        enemies = FindObjectsOfType<EnemyPathfinding>();
    }

    private void HandleTurn()
    {
        foreach (var enemy in enemies)
        {
            enemy.AttemptMove();
        }
        board.UpdatePathfinding();
    }
}