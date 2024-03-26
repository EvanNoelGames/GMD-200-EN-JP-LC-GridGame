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

    private bool done = false;

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

    IEnumerator Co_EndTurn()
    {
        yield return new WaitForSeconds(2);
        done = true;
    }

    IEnumerator Co_MoveEnemies()
    {
        done = false;
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (playerMovement.enemyFighting == null)
            {
                activeEnemies[i].doneMoving = false;
                activeEnemies[i].enemyManager = this;

                activeEnemies[i].EnemyTurn();
                StartCoroutine(Co_EndTurn());
                while (!activeEnemies[i].doneMoving && !done)
                {
                    yield return null;
                    if (done)
                    {
                        for (int j = 0; j < activeEnemies.Count; j++)
                        {
                            activeEnemies[j].doneMoving = true;
                            activeEnemies[j].StopAllCoroutines();
                        }
                    }
                }
            }
            else
            {
                i = activeEnemies.Count;
            }
        }

        enemyMovements.Clear();

        yield return new WaitForSeconds(turnSpeed);

        playerMovement.SetPlayerTurn(true);
        StopAllCoroutines();
    }
}
