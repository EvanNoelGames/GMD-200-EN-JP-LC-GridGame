using DG.Tweening;
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
    public bool lockPlayer = false;
    private bool playerMoving = false;
    private bool playerMovingOverExit = false;

    private Vector2 facingDirection;
    public Vector2 FacingDirection => facingDirection;

    public float moveSpeed = 0.075f;
    private float exitSpeed;

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
    [SerializeField] private WorldCamera cam;
    [SerializeField] private MapManager mapManager;

    public EnemyStats enemyFighting;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI stepCounter;

    private PlayerInventory playerInventory;

    private void Start()
    {
        Application.targetFrameRate = 300;
        playerInventory = GetComponent<PlayerInventory>();
        exitSpeed = moveSpeed * 2;
        defaultRoom = roomManager;

        ResetPlayerPosition();
    }

    public void ResetPlayerPosition()
    {
        // spawn point
        transform.position = new Vector3((roomManager.numColumns / 2) + ((roomManager.numColumns / 2) * roomManager.padding), (roomManager.numRows / 2) + ((roomManager.numRows / 2) * roomManager.padding), -3);
        cam.SetUpCam();
    }

    void Update()
    {
        stepCounter.text = "STEPS LEFT: " + gameManager.numSteps;
        PlayerCollision();

        if (!lockPlayer)
        {
            PlayerInput();
        }

        if (!DOTween.IsTweening(transform))
        {
            playerMoving = false;
        }
    }

    private void PlayerCollision()
    {
        // collision boxes to detect which directions are available
        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + (1 + (1 * roomManager.padding))), Vector2.one, 0, 1 << 7) || Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + (1 + (1 * roomManager.padding))), Vector2.one, 0, 1 << 8))
        {
            canMoveUp = true;
        }
        else
        {
            canMoveUp = false;
        }

        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - (1 + (1 * roomManager.padding))), Vector2.one, 0, 1 << 7) || Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - (1 + (1 * roomManager.padding))), Vector2.one, 0, 1 << 8))
        {
            canMoveDown = true;
        }
        else
        {
            canMoveDown = false;
        }

        if (Physics2D.OverlapBox(new Vector2(transform.position.x + (1 + (1 * roomManager.padding)), transform.position.y), Vector2.one, 0, 1 << 7) || Physics2D.OverlapBox(new Vector2(transform.position.x + (1 + (1 * roomManager.padding)), transform.position.y), Vector2.one, 0, 1 << 8))
        {
            canMoveRight = true;
        }
        else
        {
            canMoveRight = false;
        }

        if (Physics2D.OverlapBox(new Vector2(transform.position.x - (1 + (1 * roomManager.padding)), transform.position.y), Vector2.one, 0, 1 << 7) || Physics2D.OverlapBox(new Vector2(transform.position.x - (1 + (1 * roomManager.padding)), transform.position.y), Vector2.one, 0, 1 << 8))
        {
            canMoveLeft = true;
        }
        else
        {
            canMoveLeft = false;
        }
    }

    private void PlayerInput()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (playerTurn && !playerMoving)
        {
            if (input.x == 1 && canMoveRight)
            {
                facingDirection = Vector2.right;
                MovePlayer();
            }
            else if (input.x == -1 && canMoveLeft)
            {
                facingDirection = Vector2.left;
                MovePlayer();
            }
            else if (input.y == 1 && canMoveUp)
            {
                facingDirection = Vector2.up;
                MovePlayer();
            }
            else if (input.y == -1 && canMoveDown)
            {
                facingDirection = Vector2.down;
                MovePlayer();
            }
        }
    }

    private void MovePlayer()
    {
        if (playerMoving)
        {
            transform.DOKill(true);
        }

        playerMoving = true;

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

        if (lockPlayer)
        {
            StartCoroutine(Co_UnlockPlayer());
        }
        else if (playerTurn)
        {
            if (roomManager.roomType != RoomManager.Type.exit)
            {
                gameManager.numSteps -= 1;
            }
            if (gameManager.numSteps <= 0)
            {
                gameManager.GameOver();
            }
            if (!playerMovingOverExit)
            {
                StartCoroutine(Co_EnemyTurn());
            }
        }
    }

    IEnumerator Co_EnemyTurn()
    {
        playerTurn = false;
        yield return new WaitForSeconds(moveSpeed * 2);
        if (!playerMovingOverExit)
        {
            enemyManager.EnemyTurn();
        }
        else
        {
            playerTurn = true;
        }
    }

    public void SetPlayerTurn(bool newVal)
    {
        playerTurn = newVal;
    }

    public RoomManager GetPlayerRoom()
    {
        return roomManager;
    }

    IEnumerator Co_UnlockPlayer()
    {
        yield return new WaitForSeconds(moveSpeed * 4);

        moveSpeed = exitSpeed / 2;

        lockPlayer = false;
        playerMovingOverExit = false;
    }

    public string GetExitDistance()
    {
        Vector3 exitLocation = roomGenerator.GetExitLocation();
        float difference = 3.3f;
        if ((Mathf.Abs(exitLocation.y - transform.position.y + difference)) <= ((roomGenerator.roomPrefab.numRows / 2)) && (Mathf.Abs(exitLocation.x - transform.position.x + difference) <= (roomGenerator.roomPrefab.numColumns / 2)))
        {
            return "";
        }
        if (exitLocation.y > transform.position.y && ((Mathf.Abs(exitLocation.x - transform.position.x + 2.2f)) <= (roomGenerator.roomPrefab.numColumns / 2) + difference))
        {
            return "NORTH";
        }
        else if ((Mathf.Abs(exitLocation.y - transform.position.y + 2.2f)) <= (roomGenerator.roomPrefab.numRows / 2) + difference && exitLocation.x > transform.position.x)
        {
            return "EAST";
        }
        else if (exitLocation.y < transform.position.y && ((Mathf.Abs(exitLocation.x - transform.position.x + 2.2f)) <= (roomGenerator.roomPrefab.numColumns / 2) + difference))
        {
            return "SOUTH";
        }
        else if ((Mathf.Abs(exitLocation.y - transform.position.y + 2.2f) <= (roomGenerator.roomPrefab.numRows / 2) + difference) && exitLocation.x < transform.position.x)
        {
            return "WEST";
        }
        else if (exitLocation.y > transform.position.y && exitLocation.x > transform.position.x)
        {
            return "NORTH EAST";
        }
        else if (exitLocation.y > transform.position.y && exitLocation.x < transform.position.x)
        {
            return "NORTH WEST";
        }
        else if (exitLocation.y < transform.position.y && exitLocation.x > transform.position.x)
        {
            return "SOUTH EAST";
        }
        else if (exitLocation.y < transform.position.y && exitLocation.x < transform.position.x)
        {
            return "SOUTH WEST";
        }
        return "";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // the player collides with a room tile
        if (collision.GetComponent<RoomTile>() != null)
        {
            RoomTile colRoomTile = collision.GetComponent<RoomTile>();
            RoomManager colRoom = collision.GetComponentInParent<RoomManager>();

            // check the tile type, if it's not an exit tile then update the player's current room
            if (colRoomTile.tileType == RoomTile.Type.start || colRoomTile.tileType == RoomTile.Type.basic || colRoomTile.tileType == RoomTile.Type.chest)
            {
                RoomManager colRoomManager = collision.gameObject.GetComponentInParent<RoomManager>();
                if (colRoomManager != null)
                {
                    roomManager = colRoomManager;
                }
                mapManager.UpdatePlayerPosition();

                if (colRoomTile.tileType == RoomTile.Type.chest)
                {
                    playerInventory.AddRandomItemFromChest();
                    colRoomTile.tileType = RoomTile.Type.basic;
                    colRoomTile.UpdateSprite();
                }
            }
            // if it is an exit tile then set the roomManager back to its default state before we update it next movement
            else if (roomManager != defaultRoom)
            {
                playerMovingOverExit = true;
                roomManager = defaultRoom;

                // make the player move off of the exit tile
                MovePlayerOffExit();

                cam.MoveCamera(facingDirection);

            }
            // otherwise we are in an exit room (COVERED in exit tiles), so make sure the map knows that
            else
            {
                mapManager.UpdatePlayerPosition();
            }

            // once we enter the exit room delete every other room
            if (colRoom.roomType == RoomManager.Type.exit)
            {
                roomGenerator.RemoveRooms();
            }
            else
            {
                roomGenerator.HideRooms();
            }
        }

        if (collision.GetComponent<EnemyMovement>() != null)
        {
            EnemyMovement colEnemy = collision.GetComponent<EnemyMovement>();
            EnemyStats colEnemyStats = collision.GetComponent<EnemyStats>();

            colEnemy.endMovement = true;
            lockPlayer = true;

            roomManager._enemies.Remove(colEnemy);

            transform.DOTogglePause();

            enemyFighting = colEnemyStats;
            gameManager.SwitchToBattle();
        }
    }

    private void MovePlayerOffExit()
    {
        lockPlayer = true;

        moveSpeed = exitSpeed;

        MovePlayer();
    }
}
