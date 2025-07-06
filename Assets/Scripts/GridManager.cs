using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 6;
    [SerializeField] private float cellSize = 1.5f;

    [SerializeField] private GameObject fighterPrefab;
    [SerializeField] private GameObject healerPrefab;
    [SerializeField] private GameObject rangedPrefab;
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameObject enemy2Prefab;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private CharacterUIManager uiManager;

    private List<Vector2Int> usedPositions = new List<Vector2Int>();

    public int Rows => rows;
    public int Columns => columns;

    void Start()
    {
        GenerateGrid();
        SpawnCharacters();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 pos = GridToWorld(new Vector2Int(x, y));
                Instantiate(gridCellPrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    void SpawnCharacters()
    {
        var fighterPos = GetRandomFreePosition();
        var healerPos = GetRandomFreePosition();
        var rangedPos = GetRandomFreePosition();
        var enemy1Pos = GetRandomFreePosition();
        var enemy2Pos = GetRandomFreePosition();

        var fighter = Instantiate(fighterPrefab, GridToWorld(fighterPos), Quaternion.identity).GetComponent<Player>();
        var healer = Instantiate(healerPrefab, GridToWorld(healerPos), Quaternion.identity).GetComponent<Player>();
        var ranged = Instantiate(rangedPrefab, GridToWorld(rangedPos), Quaternion.identity).GetComponent<Player>();
        Debug.Log($"Ranged componente obtenido: {ranged}");

        var enemy1 = Instantiate(enemy1Prefab, GridToWorld(enemy1Pos), Quaternion.identity).GetComponent<Enemy>();
        var enemy2 = Instantiate(enemy2Prefab, GridToWorld(enemy2Pos), Quaternion.identity).GetComponent<Enemy>();

        fighter.InitializeCharacter();
        healer.InitializeCharacter();
        ranged.InitializeCharacter();
        enemy1.InitializeCharacter();
        enemy2.InitializeCharacter();

        fighter.GridPosition = fighterPos;
        healer.GridPosition = healerPos;
        ranged.GridPosition = rangedPos;
        enemy1.GridPosition = enemy1Pos;
        enemy2.GridPosition = enemy2Pos;

        fighter.SetGridManager(this);
        healer.SetGridManager(this);
        ranged.SetGridManager(this);
        enemy1.SetGridManager(this);
        enemy2.SetGridManager(this);

        fighter.SetUIManager(uiManager);
        healer.SetUIManager(uiManager);
        ranged.SetUIManager(uiManager);
        enemy1.SetUIManager(uiManager);
        enemy2.SetUIManager(uiManager);


        fighter.SetGameManager(gameManager);
        healer.SetGameManager(gameManager);
        ranged.SetGameManager(gameManager);
        enemy1.SetGameManager(gameManager);
        enemy2.SetGameManager(gameManager);

        fighter.transform.position = GridToWorld(fighter.GridPosition);
        healer.transform.position = GridToWorld(healer.GridPosition);
        ranged.transform.position = GridToWorld(ranged.GridPosition);
        enemy1.transform.position = GridToWorld(enemy1.GridPosition);
        enemy2.transform.position = GridToWorld(enemy2.GridPosition);

        var playerList = new List<Player> { fighter, healer, ranged };
        var enemyList = new List<Enemy> { enemy1, enemy2 };

        gameManager.RegisterPlayers(playerList);
        gameManager.RegisterEnemies(enemyList);
        gameManager.BuildTurnOrder();
        gameManager.StartCombat();
        uiManager.BuildPanels(enemyList, playerList);
    }

    Vector2Int GetRandomFreePosition()
    {
        Vector2Int pos;
        do
        {
            pos = new Vector2Int(Random.Range(0, columns), Random.Range(0, rows));
        } while (usedPositions.Contains(pos));

        usedPositions.Add(pos);
        return pos;
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize + cellSize / 2f, gridPos.y * cellSize + cellSize / 2f, 0);
    }
}
