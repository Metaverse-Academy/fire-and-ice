using System.Collections;
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
    [SerializeField] private GameObject canvas;
    public AnnounceWinnerOnDeath announceWinnerOnDeath1;
    public AnnounceWinnerOnDeath announceWinnerOnDeath2;

    [Header("Die Animation")]
    [Tooltip("Time in seconds to wait before showing Game Over to allow Die animation to play.")]
    [SerializeField] private float deathAnimLength = 1.0f;

    private bool isGameOver = false;

    private void Awake()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Call this when a player dies. The UI will show after the delay.
    /// </summary>
    public void ShowGameOver(string winnerName)
    {
        if (isGameOver) return; // prevent multiple calls
        isGameOver = true;

        StartCoroutine(DelayedGameOver(winnerName));
    }

    private IEnumerator DelayedGameOver(string winnerName)
    {
        // Wait for Die animation to finish
        yield return new WaitForSecondsRealtime(deathAnimLength);

        // Freeze the game
        Time.timeScale = 0f;

        // Update UI
        if (winnerText != null)
        {
            winnerText.text = $"{winnerName} wins!";
            canvas.SetActive(true);
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
        announceWinnerOnDeath1.shown = false;
        announceWinnerOnDeath2.shown = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
