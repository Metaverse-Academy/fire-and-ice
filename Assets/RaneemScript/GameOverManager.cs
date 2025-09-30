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
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private PopupUI gameOverPopup;
    [SerializeField] private GameObject canvas;
    public AnnounceWinnerOnDeath announceWinnerOnDeath1;
    public AnnounceWinnerOnDeath announceWinnerOnDeath2;
    

    //public static GameOverManager Instance { get; private set; }

    private void Awake()
    {
        // if (Instance != null)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        // else
        // {
        //     Instance = this;
        //     DontDestroyOnLoad(gameObject);
        // } 
        
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // if (restartButton != null) restartButton.onClick.AddListener(RestartLevel);
        // if (mainMenuButton != null) mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void ShowGameOver(string winnerName)
    {
        Time.timeScale = 0f;

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
        // resume time before loading
        Time.timeScale = 1f;
        announceWinnerOnDeath1.shown = false;
        announceWinnerOnDeath2.shown = false;
        Debug.Log("Restart Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Scene reloaded");
        // var scene = SceneManager.GetActiveScene();
        // SceneManager.LoadScene(scene.buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}