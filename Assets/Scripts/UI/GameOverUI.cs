using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text resultadoText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button creditsButton;

    [Header("Créditos")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private TMP_Text creditsText;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(ReloadScene);
        creditsButton.onClick.AddListener(OpenCredits);
        backButton.onClick.AddListener(CloseCredits);

        gameOverPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowVictory(string winnerName)
    {
        gameOverPanel.SetActive(true);
        resultadoText.text = $"¡Victoria de {winnerName}!";
    }

    public void ShowDefeat(string reason)
    {
        gameOverPanel.SetActive(true);
        resultadoText.text = $"Derrota: {reason}";
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    private void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
}
