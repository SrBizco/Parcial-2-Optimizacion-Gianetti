using UnityEngine;
using System.Collections.Generic;

public class Player : Character
{
    public bool isMyTurn = false;
    private int movesLeft;
    private bool justStartedTurn = false;

    private IActionStrategy meleeStrategy = new AttackMeleeStrategy();
    private IActionStrategy rangedStrategy = new AttackRangedStrategy();
    private IActionStrategy selfHealStrategy = new SelfHealStrategy();
    private IActionStrategy healOthersStrategy = new HealOthersStrategy();

    private void Update()
    {
        if (!isMyTurn) return;

        if (justStartedTurn)
        {
            justStartedTurn = false;
            return;
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (movesLeft <= 0) return;

        Vector2Int direction = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) direction = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S)) direction = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A)) direction = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D)) direction = Vector2Int.right;

        if (direction != Vector2Int.zero)
        {
            Vector2Int newPos = GridPosition + direction;

            if (newPos.x >= 0 && newPos.x < gridManager.Columns && newPos.y >= 0 && newPos.y < gridManager.Rows)
            {
                if (!gameManager.IsPositionOccupied(newPos))
                {
                    GridPosition = newPos;
                    transform.position = gridManager.GridToWorld(GridPosition);
                    movesLeft--;
                    Debug.Log($"{gameObject.name} se movió. Movimientos restantes: {movesLeft}");
                }
                else
                {
                    Debug.Log($"{gameObject.name} no puede moverse, la posición está ocupada.");
                }
            }
            else
            {
                Debug.Log($"{gameObject.name} no puede moverse fuera de la grilla.");
            }
        }
    }

    // 👉 Públicos para UI
    public void MeleeAttack()
    {
        var target = FindNearestTarget();
        if (target != null)
            meleeStrategy.Execute(this, target);
        else
            Debug.Log("No hay objetivo válido para ataque melee.");

        EndTurn();
    }

    public void RangedAttack()
    {
        var target = FindNearestTarget();
        if (target != null)
            rangedStrategy.Execute(this, target);
        else
            Debug.Log("No hay objetivo válido para ataque a distancia.");

        EndTurn();
    }

    public void HealSelf()
    {
        selfHealStrategy.Execute(this, this);
        EndTurn();
    }

    public void HealOther()
    {
        var target = FindNearestAlly();
        if (target != null)
            healOthersStrategy.Execute(this, target);
        else
            Debug.Log("No hay aliado válido para curar.");

        EndTurn();
    }

    private Character FindNearestTarget()
    {
        List<Character> candidates = new List<Character>();
        float minDist = float.MaxValue;

        foreach (var enemy in gameManager.GetEnemies())
        {
            if (enemy == null || enemy.CurrentHP <= 0) continue;

            float dist = Vector2Int.Distance(GridPosition, enemy.GridPosition);
            if (dist < minDist)
            {
                minDist = dist;
                candidates.Clear();
                candidates.Add(enemy);
            }
            else if (Mathf.Approximately(dist, minDist))
            {
                candidates.Add(enemy);
            }
        }

        foreach (var player in gameManager.GetPlayers())
        {
            if (player == null || player == this || player.CurrentHP <= 0) continue;

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

    private Player FindNearestAlly()
    {
        var players = gameManager.GetPlayers();
        List<Player> candidates = new List<Player>();
        float minDist = float.MaxValue;

        foreach (var ally in players)
        {
            if (ally == null || ally == this || ally.CurrentHP <= 0) continue;

            float dist = Vector2Int.Distance(GridPosition, ally.GridPosition);
            if (dist < minDist)
            {
                minDist = dist;
                candidates.Clear();
                candidates.Add(ally);
            }
            else if (Mathf.Approximately(dist, minDist))
            {
                candidates.Add(ally);
            }
        }

        if (candidates.Count > 0)
            return candidates[Random.Range(0, candidates.Count)];

        return null;
    }

    public void StartTurn()
    {
        isMyTurn = true;
        justStartedTurn = true;
        movesLeft = Speed;
        Debug.Log($"{gameObject.name} empieza su turno con {movesLeft} movimientos disponibles.");
    }

    private void EndTurn()
    {
        isMyTurn = false;
        Debug.Log($"{gameObject.name} terminó su turno.");
        gameManager.OnCharacterEndTurn(this);
    }
}
