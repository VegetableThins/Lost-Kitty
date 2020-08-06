using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    int width;
    [SerializeField]
    int height;

    int wallsWidth;
    int wallsHeight;
    int[] playSpace;

    [SerializeField]
    Tilemap wallTileMap;
    [SerializeField]
    TileBase[] wallTiles;

    [SerializeField]
    Tilemap bushTileMap;
    [SerializeField]
    TileBase[] bushTiles;

    [SerializeField]
    Tilemap exitMap;
    [SerializeField]
    TileBase[] exitTile;

    // Start is called before the first frame update
    void Start()
    {
        wallsWidth = width * 2;
        wallsHeight = height * 2;
        playSpace = new int[] { width / 2, height / 2 };

        RenderMap(GenerateWallsArray(width, height), wallTileMap, wallTiles);
        RenderMap(GenerateBushesArray(width, height), bushTileMap, bushTiles);
        RenderMap(GenerateExitArray(width, height), exitMap, exitTile);
        UpdateMap();

    }
	
    public int[,] GenerateWallsArray(int width, int height)
    {
        int[,] map = new int[wallsWidth, wallsHeight];
        
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (!(x >= playSpace[0] && x < wallsWidth - playSpace[0]-1) || !(y >= playSpace[1] && y < wallsHeight - playSpace[1]-1))
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = 0;
                }
            }
        }
        return map;
    }

    public int[,] GenerateBushesArray(int width, int height)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                var randNumb = Random.Range(0, 6);
                //1/6 chance of a bush
                if (randNumb == 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = 0;
                }
            }
        }
        return map;

    }

    public int[,] GenerateExitArray(int width, int height)
    {
        int[,] map = new int[wallsWidth, wallsHeight];
        var randX = Random.Range(playSpace[0], wallsWidth - playSpace[0] - 1);
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                
                if (y == wallsHeight - playSpace[1] - 2 && randX == x)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = 0;
                }


            }
        }
        return map;

    }

    public static void RenderMap(int[,] map, Tilemap tilemap, TileBase[] wallTiles)
    {
        tilemap.ClearAllTiles();

        int offsetX = map.GetUpperBound(0) / 2;
        int offsetY = map.GetUpperBound(1) / 2;

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x-offsetX, y-offsetY, 0), wallTiles[Random.Range(0, wallTiles.Length)]);
                }
            }
        }
    }

    public void UpdateMap()
    {
        Bounds bounds = new Bounds(Vector3.one, new Vector2(width, height));
        AstarPath.active.UpdateGraphs(bounds);
    }
}
