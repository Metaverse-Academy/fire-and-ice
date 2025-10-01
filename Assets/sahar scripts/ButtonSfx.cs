using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSfx : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioSource audioSource;   // e.g., an AudioSource on your Canvas
    [SerializeField] private AudioClip hoverClip;       // plays when mouse enters
    [SerializeField] private AudioClip clickClip;       // plays when clicked
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    // Optional: auto-find an AudioSource on parent Canvas if not assigned
    void Awake()
    {
        if (!audioSource)
            audioSource = GetComponentInParent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource && hoverClip) audioSource.PlayOneShot(hoverClip, volume);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource && clickClip) audioSource.PlayOneShot(clickClip, volume);
    }
}

