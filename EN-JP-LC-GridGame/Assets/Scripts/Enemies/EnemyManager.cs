using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    [SerializeField] private EnemyMovement enemy;

    private RoomManager playerRoom;

    public List<EnemyMovement> activeEnemies;
    public List<Vector3> enemyMovements;

    private float turnSpeed = 0.2f;

    private void Update()
    {
        GetActiveEnemies();
    }

    private void GetActiveEnemies()
    {
        playerRoom = playerMovement.GetPlayerRoom();

        activeEnemies = playerRoom._enemies;
    }

    public void EnemyTurn()
    {
        playerMovement.SetPlayerTurn(false);
        StartCoroutine(Co_MoveEnemies());
    }

    IEnumerator Co_MoveEnemies()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (playerMovement.enemyFighting == null)
            {
                activeEnemies[i].doneMoving = false;
                activeEnemies[i].enemyManager = this;

                activeEnemies[i].EnemyTurn();
                while (!activeEnemies[i].doneMoving)
                {
                    yield return null;
                }
            }
            else
            {
                //activeEnemies.Clear();
                i = activeEnemies.Count;
            }
        }

        enemyMovements.Clear();

        yield return new WaitForSeconds(turnSpeed);

        playerMovement.SetPlayerTurn(true);
    }
}
