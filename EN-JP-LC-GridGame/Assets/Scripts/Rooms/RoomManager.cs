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

    bool nextEnemy = true;

    [SerializeField] private RoomTile tilePrefab;
    [SerializeField] public GameManager gameManager;
    [SerializeField] private EnemyMovement enemyPrefab;
    public List<RoomTile> _tiles;
    public List<EnemyMovement> _enemies;

    [SerializeField] private EnemyData cyclopes;
    [SerializeField] private EnemyData knight;
    [SerializeField] private EnemyData serpent;
    [SerializeField] private EnemyData skeleton;
    [SerializeField] private EnemyData zombie;

    private void Awake()
    {
        transform.position = new Vector3(transform.position.x - 2.75f, transform.position.y - 2.75f, 0);
    }

    public void InitRoom()
    {
        gameManager.numSteps += numColumns + numRows;
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
                    tile.SetSprite("exit");
                }
                else if (roomType == Type.start)
                {
                    tile.tileType = RoomTile.Type.start;
                    tile.UpdateSprite();
                }
                else if (roomType == Type.basic)
                {
                    tile.tileType = RoomTile.Type.basic;
                    tile.UpdateSprite();
                }

                // set which tiles chests and enemies cannot spawn on
                if (numRows % 2 == 0 && numColumns % 2 == 0)
                {
                    if ((x == numColumns / 2 || x == (numColumns / 2) - 1) && (y == numRows - 1 || y == 0))
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }

                    if ((y == numRows / 2 || y == (numRows / 2) - 1) && (x == numColumns - 1 || x == 0))
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }
                }
                else if (numRows % 2 == 0 && numColumns % 2 != 0)
                {
                    if ((x == numColumns / 2) && (y == numRows - 1 || y == 0))
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }

                    if ((y == numRows / 2 || y == (numRows / 2) - 1) && (x == numColumns - 1 || x == 0))
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }
                }
                else if (numRows % 2 != 0 && numColumns % 2 == 0)
                {
                    if ((x == numColumns / 2 || x == (numColumns / 2) - 1) && (y == numRows - 1 || y == 0))
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }

                    if ((y == numRows / 2) && (x == numColumns - 1 || x == 0))
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }
                }
                else if (numRows % 2 != 0 && numColumns % 2 != 0)
                {
                    if ((x == numColumns / 2) && (y == numRows - 1 || y == 0))
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }

                    if ((y == numRows / 2) && (x == numColumns - 1 || x == 0))
                    {
                        tile.gameObject.tag = "Don't Spawn";
                    }
                }
            }
        }

        if (roomType == Type.exit)
        {
            enemyAmt = 1;
        }
        else if (roomType == Type.start)
        {
            enemyAmt = 0;
        }
        else if (roomType == Type.basic)
        {
            enemyAmt = Random.Range(0, 3 + (gameManager.CurrentFloor / 5));
        }
        else if (roomType == Type.money)
        {
            enemyAmt = Random.Range(1, 4 + (gameManager.CurrentFloor / 5));
            StartCoroutine(Co_SpawnChest());
        }
        else if (roomType == Type.mystery)
        {
            enemyAmt = Random.Range(0, 4 + (gameManager.CurrentFloor / 5));
        }

        // spawn enemies
        StartCoroutine(Co_SpawnEnemies());
    }

    public void RemoveEnemy(EnemyMovement removeEnemy)
    {
        _enemies.Remove(removeEnemy);
    }

    IEnumerator Co_SpawnEnemies()
    {
        for (int i = 0; i < enemyAmt; i++)
        {
            StartCoroutine(Co_SpawnEnemy());
            while(!nextEnemy)
            {
                yield return null;
            }
        }
    }

    IEnumerator Co_SpawnEnemy()
    {
        nextEnemy = false;
        bool valid = false;
        int enemySpawnTile;
        int checkEnemySpawns = 0;
        EnemyMovement enemy = Instantiate(enemyPrefab, transform);
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
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
                    if (Vector2.Distance(_enemies[j].transform.localPosition, enemy.transform.localPosition) > 0.5f)
                    {
                        checkEnemySpawns++;
                    }
                }

                if (checkEnemySpawns == _enemies.Count)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            }
            yield return null;
        }

        if (roomType == Type.exit)
        {
            enemyStats.aggressive = true;
            enemyStats.data = cyclopes;
        }
        else
        {
            float selectedEnemy = Random.value;
            if (roomType == Type.mystery)
            {
                if (Random.value > 0.8f)
                {
                    enemyStats.aggressive = true;
                }

                if (selectedEnemy > 0.5f)
                {
                    enemyStats.data = skeleton;
                }
                else if (selectedEnemy < 0.95f)
                {
                    enemyStats.data = zombie;
                }
                else
                {
                    enemyStats.data = knight;
                }
            }
            else if (roomType == Type.money)
            {
                if (Random.value > 0.7f)
                {
                    enemyStats.aggressive = true;
                }

                if (selectedEnemy > 0.3f)
                {
                    enemyStats.data = skeleton;
                }
                else
                {
                    enemyStats.data = zombie;
                }
            }
            else if (roomType == Type.basic)
            {
                if (Random.value > 0.95f)
                {
                    enemyStats.aggressive = true;
                }

                if (selectedEnemy > 0.1f)
                {
                    enemyStats.data = skeleton;
                }
                else
                {
                    enemyStats.data = zombie;
                }
            }
        }

        enemy.gameObject.name = $"Enemy_{enemyStats.data}";
        enemyStats.UpdateSprite();
        _enemies.Add(enemy);
        nextEnemy = true;
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
        _tiles[chest].gameObject.layer = 8;
        _tiles[chest].UpdateSprite();
    }
}
