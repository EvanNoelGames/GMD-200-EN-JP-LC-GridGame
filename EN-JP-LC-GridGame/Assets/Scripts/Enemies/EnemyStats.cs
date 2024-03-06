using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemyStats : MonoBehaviour
{
    public bool aggressive = true;

    public EnemyData data;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    public void UpdateSprite()
    {
        spriteRenderer.sprite = data.sprite;
    }
}
