using UnityEngine;
using System.Collections.Generic;

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

        MoveRandom();

        Player target = FindNearestPlayer();
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

        if (newPos.x >= 0 && newPos.x < gridManager.Columns && newPos.y >= 0 && newPos.y < gridManager.Rows)
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

    private Player FindNearestPlayer()
    {
        List<Player> candidates = new List<Player>();
        float minDist = float.MaxValue;

        foreach (var player in allPlayers)
        {
            if (player == null || player.CurrentHP <= 0) continue;

            float dist = Vector2Int.Distance(GridPosition, player.GridPosition);
            if (dist < minDist)
            {
                minDist = dist;
                candidates.Clear();
                candidates.Add(player);
            }
            else if (Mathf.Approximately(dist, minDist))
            {
                candidates.Add(player);
            }
        }

        if (candidates.Count > 0)
            return candidates[Random.Range(0, candidates.Count)];

        return null;
    }

    private void EndTurn()
    {
        isMyTurn = false;
        Debug.Log($"{gameObject.name} terminó su turno.");
        gameManager.OnCharacterEndTurn(this);
    }
}
