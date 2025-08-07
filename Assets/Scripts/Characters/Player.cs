using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMovement();
#endif
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
            AttemptMove(direction);
        }
    }

    private void AttemptMove(Vector2Int direction)
    {
        Vector2Int newPos = GridPosition + direction;

        if (gridManager.IsValidPosition(newPos))
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

    public void MoveUp() { if (isMyTurn && movesLeft > 0) AttemptMove(Vector2Int.up); }
    public void MoveDown() { if (isMyTurn && movesLeft > 0) AttemptMove(Vector2Int.down); }
    public void MoveLeft() { if (isMyTurn && movesLeft > 0) AttemptMove(Vector2Int.left); }
    public void MoveRight() { if (isMyTurn && movesLeft > 0) AttemptMove(Vector2Int.right); }

    public void MeleeAttack()
    {
        var possibleTargets = gameManager.GetPlayers().Where(p => p != this).Cast<Character>().ToList();
        possibleTargets.AddRange(gameManager.GetEnemies());

        var target = gameManager.FindNearest(GridPosition, possibleTargets);

        if (target != null)
            meleeStrategy.Execute(this, target);
        else
            Debug.Log("No hay objetivo válido para ataque melee.");

        EndTurn();
    }

    public void RangedAttack()
    {
        var possibleTargets = gameManager.GetPlayers().Where(p => p != this).Cast<Character>().ToList();
        possibleTargets.AddRange(gameManager.GetEnemies());

        var target = gameManager.FindNearest(GridPosition, possibleTargets);

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
        var target = gameManager.GetPlayers().Where(p => p != this).ToList();
        var nearest = gameManager.FindNearest(GridPosition, target);
        if (nearest != null)
            healOthersStrategy.Execute(this, nearest);
        else
            Debug.Log("No hay aliado válido para curar.");

        EndTurn();
    }

    public void StartTurn()
    {
        isMyTurn = true;
        justStartedTurn = true;
        movesLeft = Speed;
        HighlightAsActive();
        Debug.Log($"{gameObject.name} empieza su turno con {movesLeft} movimientos disponibles.");
    }

    private void EndTurn()
    {
        isMyTurn = false;
        Debug.Log($"{gameObject.name} terminó su turno.");
        gameManager.OnCharacterEndTurn(this);
    }
}
