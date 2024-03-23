using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerLevel = 1;
    public int playerHealth = 12;
    public int playerMaxHealth = 12;

    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI currentHealthText;

    public void ClampHealth()
    {
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
    }

    private void Update()
    {
        maxHealthText.text = "Max Health: " + playerMaxHealth.ToString();
        currentHealthText.text = "Current Health: " + playerHealth.ToString();
    }
}
