using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class RoomManager : MonoBehaviour
{
    public int numRows = 6;
    public int numColumns = 6;

    private int enemyAmt;

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
    [SerializeField] public GameManager gameManager;
    [SerializeField] private EnemyMovement enemyPrefab;
    public List<RoomTile> _tiles;
    public List<EnemyMovement> _enemies;

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
                    enemyAmt = 0;
                    tile.tileType = RoomTile.Type.exit;
                    tile.UpdateSprite();
                }
                else if (roomType == Type.start)
                {
                    enemyAmt = 0;
                    tile.tileType = RoomTile.Type.start;
                    tile.UpdateSprite();
                }
                else if (roomType == Type.basic)
                {
                    enemyAmt = Random.Range(0, 3 + (gameManager.CurrentFloor / 5));
                    tile.tileType = RoomTile.Type.basic;
                    tile.UpdateSprite();
                }

                // set which tiles chests and enemies cannot spawn on
                if (numRows % 2 == 0 && numColumns % 2 == 0)
                {
                    if (x == numColumns / 2 || x == (numColumns / 2) - 1)
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }

                    if (y == numRows / 2 || y == (numRows / 2) - 1)
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }
                }
                else if (numRows % 2 == 0 && numColumns % 2 != 0)
                {
                    if (x == numColumns / 2)
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }

                    if (y == numRows / 2 || y == (numRows / 2) - 1)
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }
                }
                else if (numRows % 2 != 0 && numColumns % 2 == 0)
                {
                    if (x == numColumns / 2 || x == (numColumns / 2) - 1)
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }

                    if (y == numRows / 2)
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }
                }
                else if (numRows % 2 != 0 && numColumns % 2 != 0)
                {
                    if (x == numColumns / 2)
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }

                    if (y == numRows / 2)
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }
                }
            }
        }


        if (roomType == Type.money)
        {
            enemyAmt = Random.Range(1, 4 + (gameManager.CurrentFloor / 5));
            StartCoroutine(Co_SpawnChest());
        }
        else if (roomType == Type.mystery)
        {
            enemyAmt = Random.Range(0, 4 + (gameManager.CurrentFloor / 5));
        }

        // spawn enemies
        for (int i = 0; i < enemyAmt; i++)
        {
            StartCoroutine(Co_SpawnEnemy());
        }
    }

    IEnumerator Co_SpawnEnemy()
    {
        bool valid = false;
        int enemySpawnTile;
        int checkEnemySpawns = 0;
        EnemyMovement enemy = Instantiate(enemyPrefab, transform);
        enemy.transform.SetParent(transform, false);

        while (!valid)
        {
            enemySpawnTile = Random.Range(0, _tiles.Count);
            enemy.transform.position = _tiles[enemySpawnTile].transform.position;
            enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1);

            if (_tiles[enemySpawnTile].gameObject.tag != "Don't Spawn" && _tiles[enemySpawnTile].gameObject.tag != "Exit Tile")
            {
                checkEnemySpawns = 0;
                // don't spawn one enemy on top of another
                for (int j = 0; j < _enemies.Count; j++)
                {
                    if (_enemies[j].transform.position != enemy.transform.position)
                    {
                        checkEnemySpawns++;
                    }
                }

                if (checkEnemySpawns == _enemies.Count)
                {
                    valid = true;
                }
            }
            yield return new WaitForSeconds(0.00000001f);
        }


        _enemies.Add(enemy);
    }

    IEnumerator Co_SpawnChest()
    {
        int chest = Random.Range(0, _tiles.Count);
        while (_tiles[chest].gameObject.tag == "Don't Spawn" || _tiles[chest].gameObject.tag == "Exit Tile")
        {
            yield return new WaitForSeconds(0.00000001f);
            chest = Random.Range(0, _tiles.Count);
        }
        _tiles[chest].tileType = RoomTile.Type.chest;
        _tiles[chest].UpdateSprite();
    }
}
