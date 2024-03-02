using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public MapManager mapManager;
    [SerializeField] RoomManager roomPrefab;
    [SerializeField] GameManager gameManager;
    [SerializeField] RoomTile exitTilePrefab;
    [SerializeField] PlayerMovement playerMovement;

    public List<RoomManager> rooms;

    private float padding;

    private bool doneLoading = false;
    public bool DoneLoading => doneLoading;

    private void Awake()
    {
        padding = roomPrefab.padding;
    }

    public void GenerateRooms()
    {
        for (int i = 0; i < mapManager.tiles.Count; i++)
        {
            RoomManager room = Instantiate(roomPrefab, transform);
            room.transform.position = new Vector3(mapManager.tiles[i].gridCoords.x * ((roomPrefab.numColumns * (1 + padding)) + (1 + padding)), mapManager.tiles[i].gridCoords.y * ((roomPrefab.numRows * (1 + padding)) + (1 + padding)), 0);
            room.roomType = (RoomManager.Type)mapManager.tiles[i].roomType;

            if (room.roomType == RoomManager.Type.start)
            {
                room.gameObject.tag = "Start Room";
            }
            else if (room.roomType == RoomManager.Type.basic)
            {
                room.gameObject.tag = "Basic Room";
            }
            else if (room.roomType == RoomManager.Type.money)
            {
                room.gameObject.tag = "Money Room";
            }
            else if (room.roomType == RoomManager.Type.mystery)
            {
                room.gameObject.tag = "Mystery Room";
            }
            else if (room.roomType == RoomManager.Type.exit)
            {
                room.gameObject.tag = "Exit Room";
            }

            room.name = $"Room_{room.transform.position.x}_{room.transform.position.y}_{room.roomType}";
            room.gridCoords = room.transform.position;
            room.gameManager = gameManager;
            rooms.Add(room);
            room.InitRoom();
        }

        // we have to loop through again to add all of the room exits
        for (int i = 0; i < mapManager.tiles.Count; i++)
        {
            AddExits(rooms[i]);
        }

        mapManager.SetRoomsList(rooms);

        doneLoading = true;
        playerMovement.gameObject.SetActive(true);
    }

    private void AddExits(RoomManager room)
    {
        if (!NoRoomExistsAtPosition(room.gridCoords + new Vector2(0, (room.numRows * (1 + padding)) + (1 + padding))))
        {
            if (room.numColumns % 2 == 0)
            {
                RoomTile exit1 = Instantiate(exitTilePrefab, room.transform);
                exit1.tileType = RoomTile.Type.exit;
                exit1.transform.parent = room.transform;
                exit1.roomManager = room;
                exit1.name = $"Tile_{exit1.transform.position.x}_{exit1.transform.position.y}_{exit1.tileType}_1";
                exit1.transform.position += new Vector3(((room.numColumns * (1 + padding)) / 2) - (1 + padding), room.numRows * (1 + padding), 0);
                exit1.UpdateSprite();

                RoomTile exit2 = Instantiate(exitTilePrefab, room.transform);
                exit2.tileType = RoomTile.Type.exit;
                exit2.transform.parent = room.transform;
                exit2.roomManager = room;
                exit2.name = $"Tile_{exit2.transform.position.x}_{exit2.transform.position.y}_{exit2.tileType}_2";
                exit2.transform.position += new Vector3((room.numColumns * (1 + padding)) / 2, room.numRows * (1 + padding), 0);
                exit2.UpdateSprite();

                exit1.gameObject.layer = 8;
                exit2.gameObject.layer = 8;

                exit1.gameObject.tag = "Exit Tile";
                exit2.gameObject.tag = "Exit Tile";

                room._tiles.Add(exit1);
                room._tiles.Add(exit2);
            }
            else
            {
                RoomTile exit1 = Instantiate(exitTilePrefab, room.transform);
                exit1.tileType = RoomTile.Type.exit;
                exit1.transform.parent = room.transform;
                exit1.roomManager = room;
                exit1.name = $"Tile_{exit1.transform.position.x}_{exit1.transform.position.y}_{exit1.tileType}";
                exit1.transform.position += new Vector3(((room.numColumns * (1 + padding)) / 2) - ((1 + padding) / 2), room.numRows * (1 + padding), 0);
                exit1.UpdateSprite();

                exit1.gameObject.layer = 8;

                exit1.gameObject.tag = "Exit Tile";

                room._tiles.Add(exit1);
            }
        }

        if (!NoRoomExistsAtPosition(room.gridCoords + new Vector2((room.numColumns * (1 + padding)) + (1 + padding), 0)))
        {
            if (room.numRows % 2 == 0)
            {
                RoomTile exit1 = Instantiate(exitTilePrefab, room.transform);
                exit1.tileType = RoomTile.Type.exit;
                exit1.transform.parent = room.transform;
                exit1.roomManager = room;
                exit1.name = $"Tile_{exit1.transform.position.x}_{exit1.transform.position.y}_{exit1.tileType}_1";
                exit1.transform.position += new Vector3(room.numColumns * (1 + padding), ((room.numRows * (1 + padding)) / 2) - (1 + padding), 0);
                exit1.UpdateSprite();

                RoomTile exit2 = Instantiate(exitTilePrefab, room.transform);
                exit2.tileType = RoomTile.Type.exit;
                exit2.transform.parent = room.transform;
                exit2.roomManager = room;
                exit2.name = $"Tile_{exit2.transform.position.x}_{exit2.transform.position.y}_{exit2.tileType}_2";
                exit2.transform.position += new Vector3(room.numColumns * (1 + padding), (room.numRows * (1 + padding)) / 2, 0);
                exit2.UpdateSprite();

                exit1.gameObject.layer = 8;
                exit2.gameObject.layer = 8;

                exit1.gameObject.tag = "Exit Tile";
                exit2.gameObject.tag = "Exit Tile";

                room._tiles.Add(exit1);
                room._tiles.Add(exit2);
            }
            else
            {
                RoomTile exit1 = Instantiate(exitTilePrefab, room.transform);
                exit1.tileType = RoomTile.Type.exit;
                exit1.transform.parent = room.transform;
                exit1.roomManager = room;
                exit1.name = $"Tile_{exit1.transform.position.x}_{exit1.transform.position.y}_{exit1.tileType}";
                exit1.transform.position += new Vector3(room.numColumns * (1 + padding), ((room.numRows * (1 + padding)) / 2) - ((1 + padding) / 2), 0);
                exit1.UpdateSprite();

                exit1.gameObject.layer = 8;

                exit1.gameObject.tag = "Exit Tile";

                room._tiles.Add(exit1);
            }
        }
    }

    private bool NoRoomExistsAtPosition(Vector2 pos)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].gridCoords == pos)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveRooms()
    {
        // remove every room except for the exit room
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].gameObject.tag != "Exit Room")
            {
                rooms[i].gameObject.SetActive(false);
            }
            else
            {
                // remove the exit room's exits
                for (int j = 0; j < rooms[i]._tiles.Count; j++)
                {
                    if (rooms[i]._tiles[j].gameObject.tag == "Exit Tile")
                    {
                        rooms[i]._tiles[j].gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
