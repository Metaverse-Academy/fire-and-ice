using UnityEngine;

public class TurnIndicator : MonoBehaviour
{
    public TurnManager.Side side = TurnManager.Side.Player1;

    [Header("Blink")]
    [SerializeField] float blinkSpeed = 3f;   // higher = faster
    [SerializeField] float minAlpha  = 0.35f; // faint
    [SerializeField] float maxAlpha  = 1f;    // bright

    // If your arrow is a world sprite, use SpriteRenderer.
    // If it's a UI Image under a Canvas, add a CanvasGroup and assign it.
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] CanvasGroup canvasGroup;

    void Awake()
    {
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!canvasGroup)    canvasGroup    = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        bool isMyTurn = (TurnManager.I != null && TurnManager.I.currentTurn == side);

        float t = 0.5f * (1f + Mathf.Sin(Time.unscaledTime * blinkSpeed * 2f)); // 0..1
        float a = Mathf.Lerp(minAlpha, maxAlpha, t);

        if (canvasGroup) // UI case
        {
            canvasGroup.gameObject.SetActive(isMyTurn);
            if (isMyTurn) canvasGroup.alpha = a;
        }
        else if (spriteRenderer) // world sprite case
        {
            spriteRenderer.enabled = isMyTurn;
            if (isMyTurn)
            {
                var c = spriteRenderer.color;
                c.a = a;
                spriteRenderer.color = c;
            }
        }
    }
}

