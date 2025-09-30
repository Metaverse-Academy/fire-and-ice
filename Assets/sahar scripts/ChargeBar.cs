using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;   // assign your Image here
    [SerializeField] private CanvasGroup group; // same object’s CanvasGroup

    void Awake()
    {
        if (!fillImage) fillImage = GetComponent<Image>();
        if (!group) group = GetComponent<CanvasGroup>();
        HideAndReset();
    }

    // Call every frame WHILE holding
    public void UpdateCharge(float t01)
    {
        if (group) group.alpha = 1f;                // visible while charging
        if (fillImage) fillImage.fillAmount = Mathf.Clamp01(t01); // goes UP
    }

    // Call when released or canceled
    public void HideAndReset()
    {
        if (fillImage) fillImage.fillAmount = 0f;
        if (group) group.alpha = 0f;                // hide when not charging
    }
}



