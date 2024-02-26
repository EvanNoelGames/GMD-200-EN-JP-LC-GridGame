using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public MapManager mapManager;
    public Vector2Int gridCoords;

    public enum Type
    {
        start,
        basic,
        money,
        mystery,
        exit
    }

    public Type roomType;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite startRoom;
    [SerializeField] private Sprite basicRoom;
    [SerializeField] private Sprite exitRoom;
    [SerializeField] private Sprite moneyRoom;
    [SerializeField] private Sprite mysteryRoom;

    private void Awake()
    {
        StartCoroutine(Co_GetNewSprite());
    }

    IEnumerator Co_GetNewSprite()
    {
        yield return new WaitForEndOfFrame();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (roomType == Type.start)
        {
            spriteRenderer.sprite = startRoom;
        }
        else if (roomType == Type.basic)
        {
            spriteRenderer.sprite = basicRoom;
        }
        else if (roomType == Type.exit)
        {
            spriteRenderer.sprite = exitRoom;
        }
        else if (roomType == Type.money)
        {
            spriteRenderer.sprite = moneyRoom;
        }
        else if (roomType == Type.mystery)
        {
            spriteRenderer.sprite = mysteryRoom;
        }
    }
}
