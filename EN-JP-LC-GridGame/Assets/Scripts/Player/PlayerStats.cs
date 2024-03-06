using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerLevel = 1;
    public int playerHealth = 10;
    public int playerMaxHealth = 12;

    public void ClampHealth()
    {
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
    }
}
