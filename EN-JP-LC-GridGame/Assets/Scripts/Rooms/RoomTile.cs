using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTile : MonoBehaviour
{
    public RoomManager roomManager;
    public Vector2Int gridCoords;

    public enum Type
    {
        start,
        basic,
        chest,
        exit
    }

    public Type tileType;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite startTile;
    [SerializeField] private Sprite basicTile;
    [SerializeField] private Sprite chestTile;
    [SerializeField] private Sprite exitTile;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateSprite()
    {
        if (tileType == Type.start)
        {
            spriteRenderer.sprite = startTile;
        }
        else if (tileType == Type.basic)
        {
            spriteRenderer.sprite = basicTile;
        }
        else if (tileType == Type.chest)
        {
            spriteRenderer.sprite = chestTile;
        }
        else if (tileType == Type.exit)
        {
            spriteRenderer.sprite = exitTile;
        }
    }

    public void SetSprite(string type)
    {
        if (type == "exit")
        {
            spriteRenderer.sprite = exitTile;
        }
    }
}
