using UnityEngine;
using UnityEngine.UI;

public class PlayerActionButtons : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Button meleeButton;
    [SerializeField] private Button rangedButton;
    [SerializeField] private Button healSelfButton;
    [SerializeField] private Button healAlliesButton;

    private void Start()
    {
        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();

    }

    void OnMelee()
    {
        var player = gameManager.CurrentPlayer;
        if (player != null && player.isMyTurn)
            player.MeleeAttack();
    }

    void OnRanged()
    {
        var player = gameManager.CurrentPlayer;
        if (player != null && player.isMyTurn)
            player.RangedAttack();
    }

    void OnHealSelf()
    {
        var player = gameManager.CurrentPlayer;
        if (player != null && player.isMyTurn)
            player.HealSelf();
    }

    void OnHealAllies()
    {
        var player = gameManager.CurrentPlayer;
        if (player != null && player.isMyTurn && player.CanHealOthers)
            player.HealOther();
    }
    private void AddListeners()
    {
        meleeButton.onClick.AddListener(OnMelee);
        rangedButton.onClick.AddListener(OnRanged);
        healSelfButton.onClick.AddListener(OnHealSelf);
        healAlliesButton.onClick.AddListener(OnHealAllies);
    }

    private void RemoveListeners()
    {
        meleeButton.onClick.RemoveListener(OnMelee);
        rangedButton.onClick.RemoveListener(OnRanged);
        healSelfButton.onClick.RemoveListener(OnHealSelf);
        healAlliesButton.onClick.RemoveListener(OnHealAllies);
    }
}
