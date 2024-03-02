using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyMovement : MonoBehaviour
{
    private Vector2 facingDirection;
    public Vector2 FacingDirection => facingDirection;

    public float moveSpeed = 0.1f;
    private int nextDirection;

    private Ease moveEaseType = Ease.InOutSine;

    private bool canMoveUp;
    private bool canMoveRight;
    private bool canMoveDown;
    private bool canMoveLeft;

    [SerializeField] private RoomManager roomManager;
    private RoomManager defaultRoom;
    public RoomManager RoomManager => roomManager;
    [SerializeField] private RoomGenerator roomGenerator;

    [SerializeField] private EnemyManager enemyManager;

    [SerializeField] private MapManager mapManager;

    private void Update()
    {
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
            nextDirection = Random.Range(0, 4);
            if (nextDirection == 0 && canMoveUp)
            {
                cantMove = false;
                facingDirection = Vector2.up;
            }
            else if (nextDirection == 1 && canMoveRight)
            {
                cantMove = false;
                facingDirection = Vector2.right;
            }
            else if (nextDirection == 2 && canMoveDown)
            {
                cantMove = false;
                facingDirection = Vector2.down;
            }
            else if (nextDirection == 3 && canMoveLeft)
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

        if (facingDirection != Vector2.zero)
        {
            MoveEnemy();
        }
    }

    private void MoveEnemy()
    {
        if (facingDirection == Vector2.right)
        {
            transform.DOLocalMoveX(transform.position.x + (1 + (1 * roomManager.padding)), moveSpeed).SetEase(moveEaseType);
        }
        else if (facingDirection == Vector2.left)
        {
            transform.DOLocalMoveX(transform.position.x - (1 + (1 * roomManager.padding)), moveSpeed).SetEase(moveEaseType);
        }
        else if (facingDirection == Vector2.up)
        {
            transform.DOLocalMoveY(transform.position.y + (1 + (1 * roomManager.padding)), moveSpeed).SetEase(moveEaseType);
        }
        else if (facingDirection == Vector2.down)
        {
            transform.DOLocalMoveY(transform.position.y - (1 + (1 * roomManager.padding)), moveSpeed).SetEase(moveEaseType);
        }
    }

    public RoomManager GetEnemyRoom()
    {
        return roomManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // the player collides with a room tile
        if (collision.GetComponent<EnemyMovement>() != null)
        {
            // here add something so that multiple enemies can run into the player at once, or instead make them move back (if we cant add group battles in time)
            // (could also make the player fight each of the enemies one at a time)
        }
    }
}
