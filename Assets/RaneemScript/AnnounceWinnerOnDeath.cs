using UnityEngine;

public class AnnounceWinnerOnDeath : MonoBehaviour
{
    public GameOverManager GameOverManager;
    [Tooltip("What text to show as the winner when THIS object dies.")]
    public string winnerName = "Player 2";

    public bool shown = false;

    public void Trigger()
    {
        //if (shown) return;
        //shown = true;

        if (GameOverManager != null)
        {
            GameOverManager.ShowGameOver(winnerName);
        }
        else
        {
            Debug.LogWarning("GameOverManager not found in scene.");
        }
    }
}
