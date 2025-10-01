using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;
    [SerializeField] private AudioSource soundEffectsObject;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void PlaySoundEffectsClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        if (!audioClip || !soundEffectsObject) return;

        AudioSource a = Instantiate(soundEffectsObject, spawnTransform.position, Quaternion.identity);
        a.clip = audioClip;
        a.volume = volume;
        a.Play();
        Destroy(a.gameObject, a.clip.length);
    }

    public void PlayRandomSoundEffectsClip(AudioClip[] audioClips, Transform spawnTransform, float volume)
    {
        if (audioClips == null || audioClips.Length == 0) return;
        PlaySoundEffectsClip(audioClips[Random.Range(0, audioClips.Length)], spawnTransform, volume);
    }

    // NEW — only play if it's this side's turn
    public void PlayIfTurn(TurnManager.Side side, AudioClip clip, Transform spawnTransform, float volume)
    {
        if (TurnManager.I == null || !TurnManager.I.CanShoot(side)) return;
        PlaySoundEffectsClip(clip, spawnTransform, volume);
    }

    // NEW — random version
    public void PlayRandomIfTurn(TurnManager.Side side, AudioClip[] clips, Transform spawnTransform, float volume)
    {
        if (TurnManager.I == null || !TurnManager.I.CanShoot(side)) return;
        PlayRandomSoundEffectsClip(clips, spawnTransform, volume);
    }
}
