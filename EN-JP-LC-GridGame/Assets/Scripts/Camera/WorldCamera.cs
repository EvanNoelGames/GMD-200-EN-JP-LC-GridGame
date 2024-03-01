using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class WorldCamera : MonoBehaviour
{

    [SerializeField] private RoomManager roomManager;

    [SerializeField] private PlayerMovement player;

    [SerializeField] private float moveSpeed = 3f;

    private Vector2 startingPosition;

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
            StartCoroutine(Co_MoveCamera(transform.position + new Vector3(0, (direction.y * roomManager.numRows) + (direction.y * roomManager.numRows * roomManager.padding) + (direction.y + (direction.y * roomManager.padding)), -10)));
        }
        else if (direction == Vector2.right || direction == Vector2.left)
        {
            StartCoroutine(Co_MoveCamera(transform.position + new Vector3((direction.x * roomManager.numColumns) + (direction.x * roomManager.numColumns * roomManager.padding) + (direction.x + (direction.x * roomManager.padding)), 0, -10)));
        }
    }

    IEnumerator Co_MoveCamera(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
    }
}
