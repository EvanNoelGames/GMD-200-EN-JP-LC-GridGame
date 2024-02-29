using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Vector2 input;

    private bool playerTurn = true;
    private bool playerMoving = false;

    public float moveSpeed = 10f;

    private bool canMoveUp;
    private bool canMoveRight;
    private bool canMoveDown;
    private bool canMoveLeft;

    [SerializeField] private RoomManager roomManager;
    [SerializeField] private EnemyManager enemyManager;

    private void Awake()
    {
        // spawn point
        transform.position = new Vector3((roomManager.numColumns / 2) + ((roomManager.numColumns / 2) * roomManager.padding), (roomManager.numRows / 2) + ((roomManager.numRows / 2) * roomManager.padding), -3);
    }

    void Update()
    {
        PlayerCollision();
        PlayerInput();
    }

    private void PlayerCollision()
    {
        // collision boxes to detect which directions are available
        canMoveUp = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + (1 + (1 * roomManager.padding))), Vector2.one, 0, 1 << 7);
        canMoveDown = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - (1 + (1 * roomManager.padding))), Vector2.one, 0, 1 << 7);
        canMoveRight = Physics2D.OverlapBox(new Vector2(transform.position.x + (1 + (1 * roomManager.padding)), transform.position.y), Vector2.one, 0, 1 << 7);
        canMoveLeft = Physics2D.OverlapBox(new Vector2(transform.position.x - (1 + (1 * roomManager.padding)), transform.position.y), Vector2.one, 0, 1 << 7);
    }

    private void PlayerInput()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (playerTurn && !playerMoving)
        {
            if (input.x == 1 && canMoveRight)
            {
                StartCoroutine(Co_MovePlayer(new Vector3(transform.position.x + (1 + (1 * roomManager.padding)), transform.position.y, transform.position.z)));
            }
            else if (input.x == -1 && canMoveLeft)
            {
                StartCoroutine(Co_MovePlayer(new Vector3(transform.position.x - (1 + (1 * roomManager.padding)), transform.position.y, transform.position.z)));
            }
            else if (input.y == 1 && canMoveUp)
            {
                StartCoroutine(Co_MovePlayer(new Vector3(transform.position.x, transform.position.y + (1 + (1 * roomManager.padding)), transform.position.z)));
            }
            else if (input.y == -1 && canMoveDown)
            {
                StartCoroutine(Co_MovePlayer(new Vector3(transform.position.x, transform.position.y - (1 + (1 * roomManager.padding)), transform.position.z)));
            }
        }
    }

    IEnumerator Co_MovePlayer(Vector3 targetPosition)
    {
        playerMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        playerMoving = false;
        enemyManager.EnemyTurn();
    }

    public void SetPlayerTurn(bool newVal)
    {
        playerTurn = newVal;
    }
}
