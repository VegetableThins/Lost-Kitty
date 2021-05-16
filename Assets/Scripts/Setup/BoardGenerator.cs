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

    public int randomizeLevelSizeAmout;

    private Count bushCount;
    private Count obstacleCount;

    public GameObject wallObject;
    public GameObject bushObject;
    public GameObject exitObject;
    public GameObject playerObject;
    public GameObject obstacleObject;
    public GameObject fireObject;
    public GameObject barrelObject;

    public GameObject[] enemies;

    public Sprite[] wallSprites;
    public Sprite[] bushSprites;
    public Sprite[] obstacleSprites;
    public Sprite[] fireSprites;

    private List<Vector2> gridPositions = new List<Vector2>();

    private void Start()
    {
    }

    private void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                gridPositions.Add(new Vector2(x, y));
                //Debug.Log(gridPositions[x]);
            }
        }
    }

    private void BoardSetup()
    {
        RandomizeLevelSize();
        FindObjectOfType<SetupPathfinder>().Setup();
        for (int x = -outerWallLength; x < width + outerWallLength; x++)
        {
            for (int y = -outerWallLength; y < height + outerWallLength; y++)
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    GameObject wall = Instantiate(wallObject, new Vector3(x, y, 0f), Quaternion.identity);
                    wall.GetComponent<SpriteRenderer>().sprite = wallSprites[Random.Range(0, wallSprites.Length)];
                    wall.transform.SetParent(transform);
                }
            }
        }
        InitialiseList();
    }

    private void RandomizeLevelSize()
    {
        width = Random.Range(width - randomizeLevelSizeAmout, width + randomizeLevelSizeAmout);
        height = Random.Range(height - randomizeLevelSizeAmout, height + randomizeLevelSizeAmout);
    }

    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector2 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        Debug.Log(randomPosition);
        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject tile, Sprite[] tileSprites, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject instance = Instantiate(tile, randomPosition, Quaternion.identity);
            instance.GetComponent<SpriteRenderer>().sprite = tileSprites[Random.Range(0, tileSprites.Length)];
            instance.transform.parent = transform;
        }
    }

    private void LayoutObjectAtRandom(GameObject tile, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject instance = Instantiate(tile, randomPosition, Quaternion.identity);
            instance.transform.parent = transform;
        }
    }

    private void CalculateEnemies(int level)
    {
        //use the level variable to test whether to spawn harder or easier enemies
        int enemyCount = (int)Mathf.Log(level+5, 1.5f);
        //Debug.Log(enemies.Length);
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemyAtRandom(enemies[Random.Range(0, enemies.Length)]);
        }

        //int enemyCount = Mathf.RoundToInt((level * level) * 0.15f) + 1;
        //Debug.Log("Enemy Count = " + enemyCount);
        //for (int i = 0; i < enemyCount; i++)
        //{
        //    Debug.Log("calculate enemies for loop value =" + i);
        //    var randIndex = Random.Range(0, i);
        //    Debug.Log("calculate enemies for loop randIndex =" + randIndex);
        //    SpawnEnemyAtRandom(enemies[enemies.Length]);
        //}
    }

    private void SpawnEnemyAtRandom(GameObject enemy)
    {
        Vector3 randomPosition = RandomPosition();
        GameObject instance = Instantiate(enemy, randomPosition, Quaternion.identity);
        instance.transform.parent = GameObject.FindGameObjectWithTag("Enemies").transform;
    }

    private void PlaceCustomLocation(Vector2 location, GameObject objectToPlace)
    {
        Vector2 exitPosition = location;
        gridPositions.Remove(exitPosition);
        Instantiate(objectToPlace, exitPosition, Quaternion.identity);
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        PlaceCustomLocation(new Vector2(Random.Range(0, width - 1), 0), playerObject);
        PlaceCustomLocation(new Vector2(Random.Range(0, width - 1), height - 1), exitObject);

        //this is causing index out of range errors
        bushCount = new Count(gridPositions.Count / 8, gridPositions.Count / 4);
        LayoutObjectAtRandom(bushObject, bushSprites, bushCount.minimum, bushCount.maximum);

        obstacleCount = new Count(gridPositions.Count / 8, gridPositions.Count / 4);
        LayoutObjectAtRandom(obstacleObject, obstacleSprites, bushCount.minimum, bushCount.maximum);

        LayoutObjectAtRandom(fireObject, fireSprites, 1, 1);
        LayoutObjectAtRandom(barrelObject, 1, 1);

        //cap out enemy count
        //linear in a range of 3

        //different difficulties with the boundaries
        // Random.Range(level, level+1);

        //int enemyCount = (1 / (1+(2.7^(-0.9x+3)))) *
        CalculateEnemies(level);
    }
}