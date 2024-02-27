using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public enum EnemyType
    {
        normal,
        hard,
        boss
    }
    public EnemyType enemyType;
    public int enemyBaseDamage;
    public int enemyBaseHealth;
    public float statsMultiplier;
}
