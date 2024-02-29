using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private List<GameObject> activeEnemies;

    private void Update()
    {
        GetActiveEnemies();
    }

    private void GetActiveEnemies()
    {

    }

    public void EnemyTurn()
    {
        playerMovement.SetPlayerTurn(false);
        StartCoroutine(Co_MoveEnemies());
    }

    IEnumerator Co_MoveEnemies()
    {
        yield return new WaitForSeconds(0.2f);
        playerMovement.SetPlayerTurn(true);
    }
}
