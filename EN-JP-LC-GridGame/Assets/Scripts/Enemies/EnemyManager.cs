using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private EnemyMovement enemy;

    private RoomManager playerRoom;

    private List<EnemyMovement> activeEnemies;

    private float turnSpeed = 0.2f;


    private void Update()
    {
        GetActiveEnemies();
    }

    private void GetActiveEnemies()
    {
        playerRoom = playerMovement.GetPlayerRoom();

        activeEnemies = playerRoom._enemies;

        StartCoroutine(Co_MoveEnemies());
    }

    public void EnemyTurn()
    {
        playerMovement.SetPlayerTurn(false);
        GetActiveEnemies();
    }

    IEnumerator Co_MoveEnemies()
    {
        yield return new WaitForSeconds(turnSpeed);

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            activeEnemies[i].EnemyTurn();
        }

        yield return new WaitForSeconds(turnSpeed);
        playerMovement.SetPlayerTurn(true);
    }
}
