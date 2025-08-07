using UnityEngine;
using UnityEngine.UI;

public class MobileMovementButtons : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private void Start()
    {
        upButton.onClick.AddListener(MoveUp);
        downButton.onClick.AddListener(MoveDown);
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);
    }

    private void OnDestroy()
    {
        upButton.onClick.RemoveListener(MoveUp);
        downButton.onClick.RemoveListener(MoveDown);
        leftButton.onClick.RemoveListener(MoveLeft);
        rightButton.onClick.RemoveListener(MoveRight);
    }

    void MoveUp()
    {
        var player = gameManager.CurrentPlayer;
        if (player != null && player.isMyTurn)
            player.MoveUp();
    }

    void MoveDown()
    {
        var player = gameManager.CurrentPlayer;
        if (player != null && player.isMyTurn)
            player.MoveDown();
    }

    void MoveLeft()
    {
        var player = gameManager.CurrentPlayer;
        if (player != null && player.isMyTurn)
            player.MoveLeft();
    }

    void MoveRight()
    {
        var player = gameManager.CurrentPlayer;
        if (player != null && player.isMyTurn)
            player.MoveRight();
    }
}
