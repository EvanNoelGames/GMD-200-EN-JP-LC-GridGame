using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public PlayerStats playerStats; 

    private int currentFloor = 1;
    private int inventorySlots = 8;

    public int CurrentFloor => currentFloor;
    public int InventorySlots => inventorySlots;

    public int numSteps = 0;

    private float delay = 0.25f;

    [SerializeField] private GameObject world;
    [SerializeField] private GameObject battleScreen;
    [SerializeField] private BattleSystem battleSystem;

    [SerializeField] private MapManager mapManager;
    [SerializeField] private RoomGenerator roomGenerator;

    [SerializeField] private PlayerMovement playerMovement;

    public void SwitchToBattle()
    {
        StartCoroutine(Co_SwitchToBattle());
    }

    IEnumerator Co_SwitchToBattle()
    {
        yield return new WaitForSeconds(delay);
        battleScreen.SetActive(true);
        battleSystem.ResetBattleScreen();
        world.SetActive(false);
    }

    public void SwitchToWorld(bool reset)
    {
        StartCoroutine(Co_SwitchToWorld(reset));
    }

    IEnumerator Co_SwitchToWorld(bool reset)
    {
        yield return new WaitForSeconds(delay);
        world.SetActive(true);
        playerMovement.transform.DOTogglePause();
        battleScreen.SetActive(false);
        if (reset)
        {
            playerMovement.lockPlayer = true;
            numSteps = 0;
            NextFloor();
        }
    }

    public void GameOver()
    {
        StartCoroutine(Co_GameOver());
    }

   public IEnumerator Co_GameOver()
    {
         
        yield return new WaitForSeconds(delay);

        if(playerStats.playerHealth == 0)
        {
            SceneManager.LoadScene("End");
            
        }
        else 
        {
            SceneManager.LoadScene("Main");
        }
   }
    public void NextFloor()
    {
        currentFloor++;
        playerMovement.gameObject.SetActive(false);
        roomGenerator.ResetRooms();
        mapManager.ResetMap();
    }
}
