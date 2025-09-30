using UnityEngine;

public class AnnounceWinnerOnDeath : MonoBehaviour
{
    [Tooltip("What text to show as the winner when THIS object dies.")]
    public string winnerName = "Player 2";

    private static bool shown;

    public void Trigger()
    {
        if (shown) return;
        shown = true;

        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.ShowGameOver(winnerName);
        }
        else
        {
            Debug.LogWarning("GameOverManager.Instance not found in scene.");
        }
    }
}
