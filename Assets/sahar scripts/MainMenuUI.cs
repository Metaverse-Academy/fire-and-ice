using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "merged scene"; // set this in the Inspector

    // Hook this to your Start button's OnClick
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // (Optional) Hook this to a Quit button
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

