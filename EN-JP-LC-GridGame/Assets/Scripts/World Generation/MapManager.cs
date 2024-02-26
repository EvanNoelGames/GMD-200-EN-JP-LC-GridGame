using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private int numRooms;

    public float padding = 0.1f;

    public GameManager gameManager;

    [SerializeField] private MapTile room;

    public List<MapTile> tiles;
    Vector2 newTilePos;

    private void Awake()
    {
        numRooms = gameManager.CurrentFloor * 5;
        InitGrid();
    }

    public void InitGrid()
    {
        for (int i = 0; i < numRooms; i++)
        {
            MapTile tile = Instantiate(room, transform);
            // if its the first tile on the floor ensure it is a start room
            if (i == 0)
            {
                tile.roomType = MapTile.Type.start;
            }
            // if its the last room to be generated, make it an exit
            else if (i == numRooms - 1)
            {
                tile.roomType = MapTile.Type.exit;
            }
            else
            {
                int nextRoom = GetNextRoomType();
                if (nextRoom == 0)
                {
                    tile.roomType = MapTile.Type.basic;
                }
                else if (nextRoom == 1)
                {
                    tile.roomType = MapTile.Type.money;
                }
                else if (nextRoom == 2)
                {
                    tile.roomType = MapTile.Type.mystery;
                }
            }

            // if its the first tile on the floor set its position to 0 0
            if (i == 0)
            {
                newTilePos = new Vector2(0, 0);
            }
            // else get the previous tile's position and build off of it
            else
            {
                StartCoroutine(Co_GetNextRoomLocation());
            }
            tile.transform.localPosition = new Vector2(newTilePos.x + (padding * newTilePos.x), newTilePos.y + (padding * newTilePos.y));
            tile.name = $"Tile_{newTilePos.x}_{newTilePos.y}_{tile.roomType}";
            tile.mapManager = this;
            tile.gridCoords = new Vector2Int((int)newTilePos.x, (int)newTilePos.y);
            tiles.Add(tile);
        }
    }

    private int GetNextRoomType()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                return 0;
            case 1:
                return 1;
            case 2:
                return 2;
            default:
                return 0;
        }
    }

    IEnumerator Co_GetNextRoomLocation()
    {
        // go in a random direction from the previous tile's position, keep trying until there is no tile in that direction
        newTilePos = tiles[tiles.Count - 1].gridCoords;
        while (!NoTilesExistAtPosition(newTilePos))
        {
            newTilePos = tiles[tiles.Count - 1].gridCoords;
            int direction = Random.Range(1, 5);
            if (direction == 1)
            {
                newTilePos += Vector2Int.up;
            }
            else if (direction == 2)
            {
                newTilePos += Vector2Int.right;
            }
            else if (direction == 3)
            {
                newTilePos += Vector2Int.down;
            }
            else if (direction == 4)
            {
                newTilePos += Vector2Int.left;
            }
        }
        yield return new WaitForSeconds(0.00001f);
    }

    private bool NoTilesExistAtPosition(Vector2 pos)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].gridCoords == pos)
            {
                return false;
            }
        }
        return true;
    }

}
