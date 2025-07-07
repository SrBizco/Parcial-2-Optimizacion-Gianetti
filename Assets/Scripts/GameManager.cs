using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private List<Player> players = new List<Player>();
    private List<Enemy> enemies = new List<Enemy>();

    private List<Character> turnOrder = new List<Character>();
    private int currentIndex = 0;
    private bool gameOver = false;

    public Player CurrentPlayer { get; private set; }

    public void RegisterPlayers(List<Player> playerList)
    {
        players = playerList;
    }

    public void RegisterEnemies(List<Enemy> enemyList)
    {
        enemies = enemyList;
    }

    public void BuildTurnOrder()
    {
        turnOrder.Clear();

        var fighter = players.Find(p => p.name.Contains("Fighter"));
        var ranged = players.Find(p => p.name.Contains("Ranged"));
        var healer = players.Find(p => p.name.Contains("Healer"));

        var enemy1 = enemies.Find(e => e.name.Contains("Enemy1"));
        var enemy2 = enemies.Find(e => e.name.Contains("Enemy2"));

        if (fighter != null) turnOrder.Add(fighter);
        if (enemy1 != null) turnOrder.Add(enemy1);
        if (ranged != null) turnOrder.Add(ranged);
        if (enemy2 != null) turnOrder.Add(enemy2);
        if (healer != null) turnOrder.Add(healer);
    }

    public void StartCombat()
    {
        if (turnOrder.Count > 0)
            StartNextTurn();
    }

    private void StartNextTurn()
    {
        if (gameOver) return;

        if (CheckDefeatConditions())
        {
            Debug.Log("¡Derrota! Un jugador murió mientras había enemigos.");
            gameOver = true;
            return;
        }

        if (CheckVictoryConditions())
        {
            gameOver = true;
            return;
        }

        Character current = turnOrder[currentIndex];
        if (current == null)
        {
            NextIndex();
            return;
        }

        if (current is Player player)
        {
            if (player.CurrentHP <= 0)
            {
                NextIndex();
                return;
            }

            CurrentPlayer = player;
            player.StartTurn();
            return;
        }

        if (current is Enemy enemy)
        {
            if (enemy.CurrentHP <= 0)
            {
                NextIndex();
                return;
            }

            CurrentPlayer = null;
            enemy.StartTurn(players);
        }
    }

    public void NextIndex()
    {
        currentIndex++;
        if (currentIndex >= turnOrder.Count)
            currentIndex = 0;

        StartNextTurn();
    }

    private bool CheckDefeatConditions()
    {
        bool enemiesAlive = enemies.Exists(e => e != null && e.CurrentHP > 0);

        if (enemiesAlive)
        {
            foreach (var p in players)
            {
                if (p == null || p.CurrentHP <= 0)
                    return true;
            }
        }

        return false;
    }

    private bool CheckVictoryConditions()
    {
        bool noEnemiesAlive = enemies.TrueForAll(e => e == null || e.CurrentHP <= 0);

        if (noEnemiesAlive)
        {
            int alivePlayers = 0;
            Player winner = null;

            foreach (var p in players)
            {
                if (p != null && p.CurrentHP > 0)
                {
                    alivePlayers++;
                    winner = p;
                }
            }

            if (alivePlayers == 1)
            {
                Debug.Log($"¡Victoria! {winner.name} ganó la partida.");
                return true;
            }
        }

        return false;
    }

    public List<Player> GetPlayers() => players;
    public List<Enemy> GetEnemies() => enemies;

    public bool IsPositionOccupied(Vector2Int pos)
    {
        foreach (var p in players)
        {
            if (p != null && p.GridPosition == pos && p.CurrentHP > 0)
                return true;
        }
        foreach (var e in enemies)
        {
            if (e != null && e.GridPosition == pos && e.CurrentHP > 0)
                return true;
        }
        return false;
    }

    public T FindNearest<T>(Vector2Int fromPos, List<T> candidates) where T : Character
    {
        List<T> nearestList = new List<T>();
        float minDist = float.MaxValue;

        foreach (var c in candidates)
        {
            if (c == null || c.CurrentHP <= 0) continue;

            float dist = Vector2Int.Distance(fromPos, c.GridPosition);
            if (dist < minDist)
            {
                minDist = dist;
                nearestList.Clear();
                nearestList.Add(c);
            }
            else if (Mathf.Approximately(dist, minDist))
            {
                nearestList.Add(c);
            }
        }

        if (nearestList.Count > 0)
            return nearestList[Random.Range(0, nearestList.Count)];

        return null;
    }

    public void NotifyDeath(Character deadCharacter)
    {
        int index = turnOrder.IndexOf(deadCharacter);
        if (index != -1)
        {
            turnOrder[index] = null;
        }
    }

    public void OnCharacterEndTurn(Character character)
    {
        if (turnOrder[currentIndex] == character)
        {
            NextIndex();
        }
    }
}
