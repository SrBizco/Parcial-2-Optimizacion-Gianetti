using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Enemy : Character
{
    public bool isMyTurn = false;
    private List<Player> allPlayers;

    private IActionStrategy meleeStrategy = new AttackMeleeStrategy();
    private IActionStrategy rangedStrategy = new AttackRangedStrategy();

    public void StartTurn(List<Player> players)
    {
        isMyTurn = true;
        allPlayers = players;
        Debug.Log($"{gameObject.name} empieza su turno.");

        HighlightAsActive();

        StartCoroutine(ExecuteEnemyTurn());
    }

    private IEnumerator ExecuteEnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);

        MoveRandom();
        Debug.Log($"{gameObject.name} se movió.");

        yield return new WaitForSeconds(0.5f);

        Player target = gameManager.FindNearest(GridPosition, allPlayers);
        if (target != null)
        {
            float dist = Vector2Int.Distance(GridPosition, target.GridPosition);

            if (dist <= 1.5f)
                meleeStrategy.Execute(this, target);
            else if (dist <= RangedRange)
                rangedStrategy.Execute(this, target);
            else
                Debug.Log($"{gameObject.name} no alcanzó a {target.name}.");
        }

        yield return new WaitForSeconds(0.5f);

        EndTurn();
    }

    private void MoveRandom()
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        Vector2Int dir = directions[Random.Range(0, directions.Length)];
        Vector2Int newPos = GridPosition + dir;

        if (gridManager.IsValidPosition(newPos))
        {
            if (!gameManager.IsPositionOccupied(newPos))
            {
                GridPosition = newPos;
                transform.position = gridManager.GridToWorld(GridPosition);
            }
            else
            {
                Debug.Log($"{gameObject.name} no pudo moverse, posición ocupada.");
            }
        }
    }

    private void EndTurn()
    {
        isMyTurn = false;
        Debug.Log($"{gameObject.name} terminó su turno.");
        gameManager.OnCharacterEndTurn(this);
    }
}