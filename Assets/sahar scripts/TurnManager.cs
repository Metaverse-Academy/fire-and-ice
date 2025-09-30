using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Side { Player1, Player2 }

    public static TurnManager I { get; private set; }

    [Header("Start Turn")]
    public Side currentTurn = Side.Player1;

    private int activeBalls = 0;     // how many balls are still flying from the current turn
    private bool turnLocked = false; // prevents the shooter from firing again in the same turn

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    public bool CanShoot(Side asker)
    {
        return !turnLocked && asker == currentTurn;
    }

    public void NotifyShotFired(Side shooter)
    {
        if (shooter != currentTurn) return;
        turnLocked = true;   // shooter can’t fire again this turn
        activeBalls++;
    }

    public void NotifyBallEnded()
    {
        activeBalls = Mathf.Max(0, activeBalls - 1);
        if (activeBalls == 0)
        {
            // switch turns
            currentTurn = (currentTurn == Side.Player1) ? Side.Player2 : Side.Player1;
            turnLocked = false;
        }
    }
}

