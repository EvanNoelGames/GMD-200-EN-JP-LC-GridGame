using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerLevel = 1;
    public int playerHealth = 15;
    public int playerMaxHealth = 17;

    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI currentHealthText;

    public void ClampHealth()
    {
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
    }
}
