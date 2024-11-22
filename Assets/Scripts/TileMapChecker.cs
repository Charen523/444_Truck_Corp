using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapChecker : MonoBehaviour
{
    public Tilemap tilemap;

    public bool[,] GetTileArray(Vector3Int start, Vector3Int end)
    {
        int width = end.x - start.x + 1;
        int height = end.y - start.y + 1;

        bool[,] tilePresenceArray = new bool[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3Int tilePosition = new Vector3Int(start.x + x, start.y + y, 0);
                tilePresenceArray[x, y] = tilemap.HasTile(tilePosition);
            }
        }

        return tilePresenceArray;
    }

    private void Start()
    {

    }
}
