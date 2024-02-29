using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public int numRows = 6;
    public int numColumns = 6;

    public float padding = 0.1f;

    public Vector2 gridCoords;

    public enum Type
    {
        start,
        basic,
        money,
        mystery,
        exit
    }

    public Type roomType;

    [SerializeField] private RoomTile tilePrefab;
    public List<RoomTile> _tiles;

    private void Awake()
    {
        transform.position = new Vector3(transform.position.x - 2.75f, transform.position.y - 2.75f, 0);
    }

    public void InitRoom()
    {
        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numColumns; x++)
            {
                RoomTile tile = Instantiate(tilePrefab, transform);
                Vector2 tilePos = new Vector2(x + (padding * x), y + (padding * y));
                tile.transform.localPosition = tilePos;
                tile.name = $"Tile_{x}_{y}";
                tile.roomManager = this;
                tile.gridCoords = new Vector2Int(x, y);
                tile.gameObject.layer = 7;
                _tiles.Add(tile);

                if (roomType == Type.exit)
                {
                    tile.tileType = RoomTile.Type.exit;
                    tile.UpdateSprite();
                }
                else if (roomType == Type.start)
                {
                    tile.tileType = RoomTile.Type.start;
                    tile.UpdateSprite();
                }
            }
        }

        if (roomType == Type.money)
        {
            int chest = Random.Range(0, _tiles.Count);
            _tiles[chest].tileType = RoomTile.Type.chest;
            _tiles[chest].UpdateSprite();
        }
    }
}
