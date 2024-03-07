using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private int currentFloor = 1;
    public int numSteps;
    private int inventorySlots = 8;
    public int CurrentFloor => currentFloor;
    public int InventorySlots => inventorySlots;

    private float delay = 0.25f;

    [SerializeField] private GameObject world;
    [SerializeField] private GameObject battleScreen;
    [SerializeField] private BattleSystem battleSystem;

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

    public void SwitchToWorld()
    {
        StartCoroutine(Co_SwitchToWorld());
    }

    IEnumerator Co_SwitchToWorld()
    {
        yield return new WaitForSeconds(delay);
        world.SetActive(true);
        playerMovement.transform.DOTogglePause();
        battleScreen.SetActive(false);
    }

    public void GameOver()
    {
        StartCoroutine(Co_GameOver());
    }

    IEnumerator Co_GameOver()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
