using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyMovement : MonoBehaviour
{
    private Vector2 facingDirection;
    public Vector2 FacingDirection => facingDirection;

    public Vector2 nextPos;

    public float moveSpeed = 0.1f;
    private int nextDirection;

    public bool doneMoving = false;

    public bool endMovement = false;

    private Ease moveEaseType = Ease.InOutSine;

    private bool canMoveUp;
    private bool canMoveRight;
    private bool canMoveDown;
    private bool canMoveLeft;

    [SerializeField] private RoomManager roomManager;
    private RoomManager defaultRoom;
    public RoomManager RoomManager => roomManager;

    [SerializeField] private RoomGenerator roomGenerator;

    [SerializeField] public EnemyManager enemyManager;

    [SerializeField] private MapManager mapManager;

    private EnemyStats enemyStats;

    private void Awake()
    {
        enemyStats = gameObject.GetComponent<EnemyStats>();
    }

    private void Update()
    {
        if (endMovement)
        {
            transform.DOKill();
            transform.localPosition = transform.localPosition;
        }
        EnemyCollision();
    }

    public void EnemyTurn()
    {
        StartCoroutine(Co_GetNextEnemyDirection());
    }

    private void EnemyCollision()
    {
        // collision boxes to detect which directions are available
        canMoveUp = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + (1 + (1 * roomManager.padding))), Vector2.one, 0, 1 << 7);
        canMoveDown = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - (1 + (1 * roomManager.padding))), Vector2.one, 0, 1 << 7);
        canMoveRight = Physics2D.OverlapBox(new Vector2(transform.position.x + (1 + (1 * roomManager.padding)), transform.position.y), Vector2.one, 0, 1 << 7);
        canMoveLeft = Physics2D.OverlapBox(new Vector2(transform.position.x - (1 + (1 * roomManager.padding)), transform.position.y), Vector2.one, 0, 1 << 7);
    }

    IEnumerator Co_GetNextEnemyDirection()
    {
        bool cantMove = true;

        while (cantMove)
        {
            yield return new WaitForSeconds(0.00000001f);
            // if the enemy is set to be agressive, follow the player
            if (enemyStats.aggressive)
            {
                if (Mathf.Abs(enemyManager.playerMovement.transform.position.x - transform.position.x) < 0.01)
                {
                    if (enemyManager.playerMovement.transform.position.y > transform.position.y && canMoveUp && CheckDir(new Vector3(transform.localPosition.x, transform.localPosition.y + (1 + (1 * roomManager.padding)), 0)))
                    {
                        cantMove = false;
                        facingDirection = Vector2.up;
                    }
                    else if (enemyManager.playerMovement.transform.position.y < transform.position.y && canMoveDown && CheckDir(new Vector3(transform.localPosition.x, transform.localPosition.y - (1 + (1 * roomManager.padding)), 0)))
                    {
                        cantMove = false;
                        facingDirection = Vector2.down;
                    }
                }
                else
                {
                    if (enemyManager.playerMovement.transform.position.x > transform.position.x && canMoveRight && CheckDir(new Vector3(transform.localPosition.x + (1 + (1 * roomManager.padding)), transform.localPosition.y, 0)))
                    {
                        cantMove = false;
                        facingDirection = Vector2.right;
                    }
                    else if (enemyManager.playerMovement.transform.position.x < transform.position.x && canMoveLeft && CheckDir(new Vector3(transform.localPosition.x - (1 + (1 * roomManager.padding)), transform.localPosition.y, 0)))
                    {
                        cantMove = false;
                        facingDirection = Vector2.left;
                    }
                }

                if (cantMove && canMoveUp && canMoveDown && canMoveRight && canMoveLeft)
                {
                    cantMove = false;
                    facingDirection = Vector2.zero;
                }
            }
            else
            {
                nextDirection = Random.Range(0, 4);
                if (nextDirection == 0 && canMoveUp && CheckDir(new Vector3(transform.localPosition.x, transform.localPosition.y + (1 + (1 * roomManager.padding)), 0)))
                {
                    cantMove = false;
                    facingDirection = Vector2.up;
                }
                else if (nextDirection == 1 && canMoveRight && CheckDir(new Vector3(transform.localPosition.x + (1 + (1 * roomManager.padding)), transform.localPosition.y, 0)))
                {
                    cantMove = false;
                    facingDirection = Vector2.right;
                }
                else if (nextDirection == 2 && canMoveDown && CheckDir(new Vector3(transform.localPosition.x, transform.localPosition.y - (1 + (1 * roomManager.padding)), 0)))
                {
                    cantMove = false;
                    facingDirection = Vector2.down;
                }
                else if (nextDirection == 3 && canMoveLeft && CheckDir(new Vector3(transform.localPosition.x - (1 + (1 * roomManager.padding)), transform.localPosition.y, 0)))
                {
                    cantMove = false;
                    facingDirection = Vector2.left;
                }
                else if (cantMove && canMoveUp && canMoveDown && canMoveRight && canMoveLeft)
                {
                    cantMove = false;
                    facingDirection = Vector2.zero;
                }
            }
        }

        if (facingDirection != Vector2.zero)
        {
            MoveEnemy();
        }
    }

    private bool CheckDir(Vector3 dir)
    {
        // stop two or more enemies from going on the same tile
        for (int i = 0; i < enemyManager.enemyMovements.Count; i++)
        {
            if (Vector3.Distance(enemyManager.enemyMovements[i], dir) < 0.1f)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveEnemy()
    {
        if (facingDirection == Vector2.right)
        {
            transform.DOLocalMoveX(transform.localPosition.x + (1 + (1 * roomManager.padding)), moveSpeed).SetEase(moveEaseType);
            nextPos = new Vector2(transform.localPosition.x + (1 + (1 * roomManager.padding)), transform.localPosition.y);
        }
        else if (facingDirection == Vector2.left)
        {
            transform.DOLocalMoveX(transform.localPosition.x - (1 + (1 * roomManager.padding)), moveSpeed).SetEase(moveEaseType);
            nextPos = new Vector2(transform.localPosition.x - (1 + (1 * roomManager.padding)), transform.localPosition.y);
        }
        else if (facingDirection == Vector2.up)
        {
            transform.DOLocalMoveY(transform.localPosition.y + (1 + (1 * roomManager.padding)), moveSpeed).SetEase(moveEaseType);
            nextPos = new Vector2(transform.localPosition.x, transform.localPosition.y + (1 + (1 * roomManager.padding)));
        }
        else if (facingDirection == Vector2.down)
        {
            transform.DOLocalMoveY(transform.localPosition.y - (1 + (1 * roomManager.padding)), moveSpeed).SetEase(moveEaseType);
            nextPos = new Vector2(transform.localPosition.x, transform.localPosition.y - (1 + (1 * roomManager.padding)));
        }
        enemyManager.enemyMovements.Add(nextPos);
        doneMoving = true;
    }

    public RoomManager GetEnemyRoom()
    {
        return roomManager;
    }
}
