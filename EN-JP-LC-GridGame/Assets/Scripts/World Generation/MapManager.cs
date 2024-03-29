using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MapManager : MonoBehaviour
{
    private int numRooms;
    private int numMoneyRooms;
    private int numMysteryRooms;

    public float padding = 0.1f;

    public GameManager gameManager;

    [SerializeField] private MapTile room;
    [SerializeField] private MapCamera mapCamera;
    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Transform playerPositionIndicator;

    public List<MapTile> tiles;
    public List<RoomManager> roomList;
    private Vector2 newTilePos;

    private float maxTileX;
    private float minTileX;
    private float maxTileY;
    private float minTileY;
    public float MaxTileX => maxTileX;
    public float MinTileX => minTileX;
    public float MaxTileY => maxTileY;
    public float MinTileY => minTileY;

    private bool doneLoading = false;
    public bool DoneLoading => doneLoading;

    private void Awake()
    {
        InitGrid();
    }

    public void ResetMap()
    {
        tiles.Clear();
        roomList.Clear();

        foreach (Transform child in transform)
        {
            if (child.name != "player_position_on_map")
            {
                Destroy(child.gameObject);
            }
        }
        InitGrid();
        player.ResetPlayerPosition();
    }

    public void InitGrid()
    {
        numRooms = (int)(gameManager.CurrentFloor * 1.5f + Random.Range(4, 6));
        numMoneyRooms = (int)(numRooms / 2.5f);
        numMysteryRooms = (int)(numRooms * 1.2f);

        StartCoroutine(Co_AddTile());
    }

    IEnumerator Co_AddTile()
    {
        doneLoading = false;
        for (int i = 0; i < numRooms; i++)
        {
            yield return new WaitForSeconds(0.00000001f);
            MapTile tile = Instantiate(room, transform);
            tile.gameObject.layer = 6;
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
        doneLoading = true;
        mapCamera.SetupMap();
        roomGenerator.GenerateRooms();
    }

    private int GetNextRoomType()
    {
        int nextRoom = Random.Range(0, 3);

        // add another layer of chance for money and mystery rooms
        // 80%
        if (nextRoom == 2 && Random.value > 0.8f)
        {
            nextRoom = 0;
        }
        // 25%
        if (nextRoom == 3 && Random.value > 0.25f)
        {
            nextRoom = 0;
        }

        // if we ran out of money and mystery rooms just put down a basic room
        if (numMoneyRooms == 0 && nextRoom == 1)
        {
            nextRoom = 0;
        }
        if (numMysteryRooms == 0 && nextRoom == 2)
        {
            nextRoom = 0;
        }

        switch (nextRoom)
        {
            case 0:
                return 0;
            case 1:
                numMoneyRooms--;
                return 1;
            case 2:
                numMysteryRooms--;
                return 2;
            default:
                return 0;
        }
    }

    IEnumerator Co_GetNextRoomLocation()
    {
        // go in a random direction from the previous tile's position, keep trying until there is no tile in that direction
        newTilePos = tiles[tiles.Count - 1].gridCoords;

        // how many tiles are we going back from the most recently placed one?
        int goBack = 0;

        // in which directions have we tried to place the tile
        bool triedUp = false;
        bool triedRight = false;
        bool triedDown = false;
        bool triedLeft = false;
        while (!NoTilesExistAtPosition(newTilePos))
        {
            if (triedUp && triedRight && triedDown && triedLeft)
            {
                triedUp = false;
                triedRight = false;
                triedDown = false;
                triedLeft = false;
                goBack++;
            }

            newTilePos = tiles[tiles.Count - 1 - goBack].gridCoords;
            int direction = Random.Range(1, 5);
            if (direction == 1)
            {
                newTilePos += Vector2Int.up;
                triedUp = true;
            }
            else if (direction == 2)
            {
                newTilePos += Vector2Int.right;
                triedRight = true;
            }
            else if (direction == 3)
            {
                newTilePos += Vector2Int.down;
                triedDown = true;
            }
            else if (direction == 4)
            {
                newTilePos += Vector2Int.left;
                triedLeft = true;
            }
        }
        yield return new WaitForSeconds(0.00000001f);
    }

    public bool NoTilesExistAtPosition(Vector2 pos)
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

    public Vector3 GetAverageTilePosition()
    {
        maxTileX = tiles[0].transform.position.x;
        minTileX = tiles[0].transform.position.x;
        maxTileY = tiles[0].transform.position.y;
        minTileY = tiles[0].transform.position.y;

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].transform.position.x > maxTileX)
            {
                maxTileX = tiles[i].transform.position.x;
            }
            else if (tiles[i].transform.position.x < minTileX)
            {
                minTileX = tiles[i].transform.position.x;
            }
            if (tiles[i].transform.position.y > maxTileY)
            {
                maxTileY = tiles[i].transform.position.y;
            }
            else if (tiles[i].transform.position.y < minTileY)
            {
                minTileY = tiles[i].transform.position.y;
            }
        }
        return new Vector3((maxTileX + minTileX) / 2, (maxTileY + minTileY) / 2, 0);
    }

    public void SetRoomsList(List<RoomManager> rooms)
    {
        roomList = rooms;
    }

    public bool NoRoomInDirection(Vector2 dir)
    {
        RoomManager rm = player.GetPlayerRoom();

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i] == rm)
            {
                return NoTilesExistAtPosition(tiles[i].gridCoords + dir);
            }
        }
        return false;
    }

    public void UpdatePlayerPosition()
    {
        // get the room the player is in (the roomManager)
        RoomManager rm = player.GetPlayerRoom();

        // run through our list of rooms
        for (int i = 0; i < roomList.Count; i++)
        {
            // once we get to the roomManager that is the same as the one the player is on, update the indicator position
            if (roomList[i] == rm)
            {
                playerPositionIndicator.transform.position = tiles[i].transform.position;
                return;
            }
        }
        // if we cannot find the room the player is on than set the indicator to the exit tile (the player's room does not exist)
        playerPositionIndicator.transform.position = tiles[roomList.Count - 1].transform.position;
    }

}
