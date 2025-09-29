using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ForceBarAdapter : MonoBehaviour
{
    [Header("Bindings (any you use)")]
    public Slider slider;            // optional
    public Image fillImage;          // optional (set Image.type = Filled)
    [Tooltip("Call your custom bar’s setter (float 0..1) here.")]
    public UnityEvent<float> onValue; // optional passthrough for custom assets
    public CanvasGroup canvasGroup;  // optional for fade/show

    [Header("Show/Hide")]
    public bool hideWhenZero = true;
    public float fadeSpeed = 20f;

    float current = 0f;
    float targetAlpha = 0f;

    void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (canvasGroup)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            if (hideWhenZero) gameObject.SetActive(canvasGroup.alpha > 0.01f);
        }
    }

    public void SetValue(float t01)
    {
        current = Mathf.Clamp01(t01);
        if (slider)     slider.value = current;
        if (fillImage)  fillImage.fillAmount = current;
        onValue?.Invoke(current);
    }

    public void Show(bool show)
    {
        if (!canvasGroup)
        {
            // fallback show/hide
            gameObject.SetActive(show || !hideWhenZero);
            return;
        }
        targetAlpha = show ? 1f : 0f;
        if (show && hideWhenZero && !gameObject.activeSelf) gameObject.SetActive(true);
    }
}
