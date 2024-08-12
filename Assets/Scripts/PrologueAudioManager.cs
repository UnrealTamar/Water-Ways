using UnityEngine;

public class PrologueAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] voiceClips;

    public void PlayVoiceClip(int index)
    {
        if (index < 0 || index >= voiceClips.Length)
        {
            Debug.LogWarning("No audio clip available for the given index: " + index);
            return;
        }

        audioSource.clip = voiceClips[index];
        audioSource.Play();
    }
}