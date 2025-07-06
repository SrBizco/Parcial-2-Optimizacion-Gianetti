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
        meleeButton.onClick.AddListener(OnMelee);
        rangedButton.onClick.AddListener(OnRanged);
        healSelfButton.onClick.AddListener(OnHealSelf);
        healAlliesButton.onClick.AddListener(OnHealAllies);
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
}
