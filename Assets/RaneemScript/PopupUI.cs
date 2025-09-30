using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PopupUI : MonoBehaviour
{
    [Header("Timing")]
    public float showDuration = 0.25f;
    public float hideDuration = 0.2f;

    [Header("Scale")]
    public Vector3 hiddenScale = new Vector3(0.8f, 0.8f, 1f);
    public Vector3 shownScale = Vector3.one;

    [Header("Easing (0..1)")]
    public AnimationCurve easeOutBack = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private CanvasGroup cg;
    private Coroutine anim;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0f;
        transform.localScale = hiddenScale;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        if (anim != null) StopCoroutine(anim);
        gameObject.SetActive(true);
        anim = StartCoroutine(Animate(visible: true, showDuration));
    }

    public void Hide()
    {
        if (anim != null) StopCoroutine(anim);
        anim = StartCoroutine(Animate(visible: false, hideDuration));
    }

    IEnumerator Animate(bool visible, float duration)
    {
        float t = 0f;
        float startA = cg.alpha;
        float endA = visible ? 1f : 0f;

        Vector3 startS = transform.localScale;
        Vector3 endS = visible ? shownScale : hiddenScale;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / duration);
            float eased = EaseOutBack(p);
            cg.alpha = Mathf.Lerp(startA, endA, p);
            transform.localScale = Vector3.Lerp(startS, endS, eased);
            yield return null;
        }

        cg.alpha = endA;
        transform.localScale = endS;

        if (!visible) gameObject.SetActive(false);
        anim = null;
    }
    float EaseOutBack(float x)
    {
        const float k = 1.70158f;
        x = Mathf.Clamp01(x);
        return 1 + (k + 1) * Mathf.Pow(x - 1, 3) + k * Mathf.Pow(x - 1, 2);
    }
}
