using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardGenerator : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int width;
    public int height;
    public int outerWallLength;

    private Count bushCount;

    public GameObject wallObject;
    public GameObject bushObject;
    public GameObject exitObject;
    public GameObject playerObject;
    public GameObject enemyObject;

    public Sprite[] wallSprites;
    public Sprite[] bushSprites;
    public Sprite[] enemySprites;

    private List<Vector2> gridPositions = new List<Vector2>();

    private void Start()
    {
        bushCount = new Count((width + height / 4), (width + height / 2));
    }

    private void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < height - 1; x++)
        {
            for (int y = 1; y < width - 1; y++)
            {
                gridPositions.Add(new Vector2(x, y));
            }
        }
    }

    private void BoardSetup()
    {
        for (int x = -outerWallLength; x < height + outerWallLength; x++)
        {
            for (int y = -outerWallLength; y < width + outerWallLength; y++)
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    GameObject wall = Instantiate(wallObject, new Vector3(x, y, 0f), Quaternion.identity);
                    wall.GetComponent<SpriteRenderer>().sprite = wallSprites[Random.Range(0, wallSprites.Length)];
                    wall.transform.SetParent(transform);
                }
            }
        }
    }

    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector2 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject tile, Sprite[] tileSprites, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject instance = Instantiate(tile, randomPosition, Quaternion.identity);
            instance.GetComponent<SpriteRenderer>().sprite = tileSprites[Random.Range(0, tileSprites.Length)];
            instance.transform.parent = transform;
        }
    }

    private void PlacePlayer()
    {
        Vector2 playerPosition = new Vector2(Random.Range(0, width - 1), 0);
        gridPositions.Remove(playerPosition);
        Instantiate(playerObject, playerPosition, Quaternion.identity);
    }

    private void PlaceExit()
    {
        Vector2 exitPosition = new Vector2(Random.Range(0, width - 1), height - 1);
        gridPositions.Remove(exitPosition);
        Instantiate(exitObject, exitPosition, Quaternion.identity);
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        PlacePlayer();
        PlaceExit();
        LayoutObjectAtRandom(bushObject, bushSprites, bushCount.minimum, bushCount.maximum);
        int enemyCount = (int)Mathf.Log(10, 2f);
        LayoutObjectAtRandom(enemyObject, enemySprites, enemyCount, enemyCount);
    }

    private void AddBlockManager(GameObject gameObject)
    {
        if (!gameObject.GetComponent<SingleNodeBlocker>())
            return;

        gameObject.GetComponent<SingleNodeBlocker>().manager = FindObjectOfType<BlockManager>();
    }

    public void UpdatePathfinding()
    {
        Bounds bounds = new Bounds(Vector3.one, new Vector2(width, height));
        AstarPath.active.UpdateGraphs(bounds);
    }
}