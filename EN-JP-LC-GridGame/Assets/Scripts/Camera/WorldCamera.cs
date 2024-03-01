using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class WorldCamera : MonoBehaviour
{

    [SerializeField] private RoomManager roomManager;

    [SerializeField] private PlayerMovement player;

    [SerializeField] private float moveSpeed = 0.25f;

    private Vector2 startingPosition;

    private Ease camEaseType = Ease.InOutSine;

    private void Awake()
    {
        
    }

    public void SetUpCam()
    {
        // the starting position of the camera will be different depending on if there is an odd or even amount of rows and columns
        if (roomManager.numColumns % 2 == 0 && roomManager.numRows % 2 == 0)
        {
            transform.position = player.transform.position - new Vector3((1 + roomManager.padding) / 2, (1 + roomManager.padding) / 2, 7);
        }
        else if (roomManager.numColumns % 2 == 1 && roomManager.numRows % 2 == 0)
        {
            transform.position = player.transform.position - new Vector3(0, (1 + roomManager.padding) / 2, 7);
        }
        else if (roomManager.numColumns % 2 == 0 && roomManager.numRows % 2 == 1)
        {
            transform.position = player.transform.position - new Vector3((1 + roomManager.padding) / 2, 0, 7);
        }
        else
        {
            transform.position = player.transform.position - new Vector3(0, 0, 7);
        }

        startingPosition = transform.position;
    }

    public void MoveCamera(Vector2 direction)
    {
        if (direction == Vector2.up || direction == Vector2.down)
        {
            transform.DOLocalMoveY(transform.position.y + (direction.y * roomManager.numRows) + (direction.y * roomManager.numRows * roomManager.padding) + (direction.y + (direction.y * roomManager.padding)), moveSpeed).SetEase(camEaseType);
        }
        else if (direction == Vector2.right || direction == Vector2.left)
        {
            transform.DOLocalMoveX(transform.position.x + (direction.x * roomManager.numColumns) + (direction.x * roomManager.numColumns * roomManager.padding) + (direction.x + (direction.x * roomManager.padding)), moveSpeed).SetEase(camEaseType);
        }
    }
}
