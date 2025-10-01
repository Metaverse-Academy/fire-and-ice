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

    [Header("Click SFX")]
    [SerializeField] private AudioSource uiAudio;             // <-- assign an AudioSource on your Canvas
    [SerializeField] private AudioClip clickClip;             // <-- your button click sound
    [SerializeField, Range(0f, 1f)] private float clickVolume = 1f;

    private bool isGameOver = false;

    private void Awake()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Optional: wire buttons here (or set in Inspector)
        if (restartButton)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(OnRestartPressed);
        }
        if (mainMenuButton)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(OnMainMenuPressed);
        }
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
        // Wait for Die animation to finish (unscaled time, works when paused)
        yield return new WaitForSecondsRealtime(deathAnimLength);

        // Freeze the game
        Time.timeScale = 0f;

        // Update UI
        if (winnerText != null)
        {
            winnerText.text = $"{winnerName} wins!";
            if (canvas) canvas.SetActive(true);
            if (gameOverPopup) gameOverPopup.Show();
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // ---------- Button hooks (play click, then act) ----------

    public void OnRestartPressed()
    {
        StartCoroutine(PlayClickThen(RestartNow));
    }

    public void OnMainMenuPressed()
    {
        StartCoroutine(PlayClickThen(GoToMainMenuNow));
    }

    private IEnumerator PlayClickThen(System.Action action)
    {
        if (uiAudio && clickClip)
        {
            uiAudio.PlayOneShot(clickClip, clickVolume);
            yield return new WaitForSecondsRealtime(clickClip.length); // not affected by Time.timeScale
        }
        action?.Invoke();
    }

    // ---------- Actual actions ----------

    private void RestartNow()
    {
        Time.timeScale = 1f;
        if (announceWinnerOnDeath1) announceWinnerOnDeath1.shown = false;
        if (announceWinnerOnDeath2) announceWinnerOnDeath2.shown = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMainMenuNow()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
