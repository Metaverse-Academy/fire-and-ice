using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Main Menu Scene Name")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private PopupUI gameOverPopup;

    public static GameOverManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (restartButton != null) restartButton.onClick.AddListener(RestartLevel);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void ShowGameOver(string winnerName)
    {
        Time.timeScale = 0f;

        if (winnerText != null)
        {
            winnerText.text = $"{winnerName} wins!";
            gameOverPopup.Show(); 
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

  public void RestartLevel()
{
    Time.timeScale = 1f;
    // (reload scene) ...
}

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        
    }
}
