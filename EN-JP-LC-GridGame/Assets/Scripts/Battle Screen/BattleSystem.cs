using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public List<GameObject> units;

    public Transform playerBattleStation; 
    public Transform enemyBattleStation;

    public Unit playerUnit;
    public Unit enemyUnit;

    public TextMeshProUGUI dialougeText; 

    public BattleHud playerHud; 
    public BattleHud enemyHud;

    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameManager gameManager;

    public BattleState state; 

    public void ResetBattleScreen()
    {
        if (units.Count > 0)
        {
            for (int i = 0; i < units.Count; i++)
            {
                Destroy(units[i]);
            }
        }

        state = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        playerUnit.damage = playerInventory.equippedWeapon.damage;
        playerUnit.currentHP = playerStats.playerHealth;
        playerUnit.maxHP = playerStats.playerMaxHealth;
        playerUnit.unitLevel = playerStats.playerLevel;

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        enemyUnit.unitName = playerMovement.enemyFighting.data.enemyName;
        enemyUnit.damage = playerMovement.enemyFighting.data.enemyBaseDamage;

        enemyUnit.UpdateSprite(playerMovement.enemyFighting.data.sprite);
        enemyGO.GetComponent<Image>().sprite = enemyUnit.sprite;

        units.Add(enemyGO);
        units.Add(playerGO);

        playerMovement.enemyFighting.DisableEnemy();

        dialougeText.text = "A wild " + enemyUnit.unitName + " approaches...";

        playerHud.SetHud(playerUnit);
        enemyHud.SetHud(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        playerTurn();
    }

    IEnumerator PlayerAttack()
    {
       bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHud.SetHP(enemyUnit.currentHP);
        dialougeText.text = "The attack is succesful";

        yield return new WaitForSeconds(2f);    

        if(isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void UpdatePlayerHP()
    {
        playerHud.SetHP(playerStats.playerHealth);
        playerHud.UpdateMaxHPHud(playerStats.playerMaxHealth);

    }

    IEnumerator EnemyTurn()
    {
        playerUnit.currentHP = playerStats.playerHealth;
        playerUnit.maxHP = playerStats.playerMaxHealth;

        dialougeText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead =  playerUnit.TakeDamage(enemyUnit.damage);

        playerHud.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            playerTurn();
        }

        playerStats.playerHealth = playerUnit.currentHP;
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialougeText.text = "You won the Battle!";
            playerStats.playerHealth = playerUnit.currentHP;
            playerStats.playerLevel = playerUnit.unitLevel;
            gameManager.SwitchToWorld();
        }
        else if (state == BattleState.LOST)
        {
            dialougeText.text = "You were Defeated.";
            gameManager.GameOver();
        }
        
    }

    void playerTurn()
    {
        dialougeText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        playerHud.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return; 

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }
}
