using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private AudioSource uiAudio;      // AudioSource on your Canvas
    [SerializeField] private AudioClip clickClip;      // the click sound
    [SerializeField, Range(0f,1f)] private float clickVolume = 1f;

    public void StartGame()  { StartCoroutine(StartAfterClick()); }
    public void ExitGame()   { StartCoroutine(ExitAfterClick());  }

    private System.Collections.IEnumerator StartAfterClick()
    {
        if (uiAudio && clickClip)
        {
            uiAudio.PlayOneShot(clickClip, clickVolume);
            yield return new WaitForSecondsRealtime(clickClip.length);
        }
        SceneManager.LoadScene(gameSceneName);
    }

    private System.Collections.IEnumerator ExitAfterClick()
    {
        if (uiAudio && clickClip)
        {
            uiAudio.PlayOneShot(clickClip, clickVolume);
            yield return new WaitForSecondsRealtime(clickClip.length);
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
