using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHP;
    [SerializeField] private int speed;
    [SerializeField] private int meleeDamage;
    [SerializeField] private int rangedDamage;
    [SerializeField] private int rangedRange;
    [SerializeField] private int healAmount;
    [SerializeField] private bool canHealOthers;

    public int CurrentHP { get; private set; }
    public int MaxHP => maxHP;
    public int Speed => speed;
    public int MeleeDamage => meleeDamage;
    public int RangedDamage => rangedDamage;
    public int RangedRange => rangedRange;
    public int HealAmount => healAmount;
    public bool CanHealOthers => canHealOthers;

    public Vector2Int GridPosition { get; set; }

    protected GridManager gridManager;
    protected GameManager gameManager;
    protected CharacterUIManager characterUIManager;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void InitializeCharacter()
    {
        CurrentHP = maxHP;
    }

    public void InitializeReferences(GridManager grid, GameManager gm, CharacterUIManager ui)
    {
        gridManager = grid;
        gameManager = gm;
        characterUIManager = ui;
    }

    public void SetPosition(Vector2Int gridPos, GridManager grid)
    {
        GridPosition = gridPos;
        transform.position = grid.GridToWorld(gridPos);
    }

    public Sprite GetCharacterSprite()
    {
        return spriteRenderer != null ? spriteRenderer.sprite : null;
    }

    public virtual void ReceiveDamage(int amount)
    {
        CurrentHP -= amount;
        if (CurrentHP <= 0)
        {
            Die();
        }
        characterUIManager.UpdateHP(gameManager.GetEnemies(), gameManager.GetPlayers());
    }

    public virtual void Heal(int amount)
    {
        CurrentHP += amount;
        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;

        characterUIManager.UpdateHP(gameManager.GetEnemies(), gameManager.GetPlayers());
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        gameManager.NotifyDeath(this);
        characterUIManager.RemovePanel(this);
        Destroy(gameObject);
    }
    public void HighlightAsActive()
    {
        characterUIManager.HighlightActivePlayer(this);
    }
}
