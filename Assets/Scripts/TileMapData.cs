using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapData : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private int width;
    [SerializeField] private int height;

    public bool[,] Tiles;

    private void Awake()
    {
        Tiles = GetTileArray(new Vector3Int(0, 0));
        ShowTiles(Tiles);
    }

    private void ShowTiles(bool[,] tiles)
    {
        string tileString = "";

        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            string row = "";
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                row += tiles[x, y] ? "0" : "  ";
            }
            tileString += row + "\n";
        }
        Debug.Log(tileString);
    }

    public bool[,] GetTileArray(Vector3Int start)
    {
        bool[,] tilePresenceArray = new bool[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3Int tilePosition = new Vector3Int(start.x + x, start.y - y - 1, 0);
                tilePresenceArray[x, y] = tilemap.HasTile(tilePosition);
            }
        }

        return tilePresenceArray;
    }

    public List<Vector2Int> GetTileVectorList()
    {
        List<Vector2Int> list = new List<Vector2Int>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Tiles[x, y])
                {
                    list.Add(new Vector2Int(x, y));
                }
            }
        }
        return list;
    }
}