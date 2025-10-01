using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Side { Player1, Player2 }

    public static TurnManager I { get; private set; }

    [Header("Start Turn")]
    [SerializeField] private bool randomizeStartTurn = true;           // <-- NEW
    [SerializeField] private Side defaultStart = Side.Player1;         // optional fallback
    public Side currentTurn;                                           // (no default here)

    private int activeBalls = 0;
    private bool turnLocked = false;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;

        // Pick who starts
        if (randomizeStartTurn)
            currentTurn = (Random.value < 0.5f) ? Side.Player1 : Side.Player2;
        else
            currentTurn = defaultStart;

        // reset per-turn state
        activeBalls = 0;
        turnLocked = false;
    }

    public bool CanShoot(Side asker)
    {
        return !turnLocked && asker == currentTurn;
    }

    public void NotifyShotFired(Side shooter)
    {
        if (shooter != currentTurn) return;
        turnLocked = true;
        activeBalls++;
    }

    public void NotifyBallEnded()
    {
        activeBalls = Mathf.Max(0, activeBalls - 1);
        if (activeBalls == 0)
        {
            currentTurn = (currentTurn == Side.Player1) ? Side.Player2 : Side.Player1;
            turnLocked = false;
        }
    }

    // OPTIONAL: call this if you ever do "next round" without reloading the scene
    public void RandomizeStartTurn()
    {
        currentTurn = (Random.value < 0.5f) ? Side.Player1 : Side.Player2;
        activeBalls = 0;
        turnLocked = false;
    }
}
