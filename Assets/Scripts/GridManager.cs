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
        var playerList = SpawnPlayers();
        var enemyList = SpawnEnemies();

        gameManager.RegisterPlayers(playerList);
        gameManager.RegisterEnemies(enemyList);
        gameManager.BuildTurnOrder();
        gameManager.StartCombat();
        uiManager.BuildPanels(enemyList, playerList);
    }

    List<Player> SpawnPlayers()
    {
        var fighter = SpawnPlayer(fighterPrefab, GetRandomFreePosition());
        var healer = SpawnPlayer(healerPrefab, GetRandomFreePosition());
        var ranged = SpawnPlayer(rangedPrefab, GetRandomFreePosition());

        return new List<Player> { fighter, healer, ranged };
    }

    List<Enemy> SpawnEnemies()
    {
        var enemy1 = SpawnEnemy(enemy1Prefab, GetRandomFreePosition());
        var enemy2 = SpawnEnemy(enemy2Prefab, GetRandomFreePosition());

        return new List<Enemy> { enemy1, enemy2 };
    }

    Player SpawnPlayer(GameObject prefab, Vector2Int position)
    {
        var player = Instantiate(prefab, GridToWorld(position), Quaternion.identity).GetComponent<Player>();
        SetupCharacter(player, position);
        return player;
    }

    Enemy SpawnEnemy(GameObject prefab, Vector2Int position)
    {
        var enemy = Instantiate(prefab, GridToWorld(position), Quaternion.identity).GetComponent<Enemy>();
        SetupCharacter(enemy, position);
        return enemy;
    }

    void SetupCharacter(Character character, Vector2Int position)
    {
        character.InitializeCharacter();
        character.InitializeReferences(this, gameManager, uiManager);
        character.SetPosition(position, this);
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

    public bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < columns && pos.y >= 0 && pos.y < rows;
    }
}
