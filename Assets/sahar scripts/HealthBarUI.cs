using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health target; // the player's Health component
    [SerializeField] private Image fill;    // this Image (Type=Filled)

    void Reset()
    {
        fill = GetComponent<Image>();
    }

    void LateUpdate()
    {
        if (!target || !fill) return;
        fill.fillAmount = (float)target.CurrentHealth / Mathf.Max(1, target.MaxHealth);
    }
}

