using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class AudioSourceExtensions
{
    public static void PlayWithRandomPitch(this AudioSource audioSource, float minPitch, float maxPitch)
    {
        SetRandomPitch(audioSource, minPitch, maxPitch);
        audioSource.Play();
    }

    public static void PlayOneShotWithRandomPitch(this AudioSource audioSource, AudioClip clip, float minPitch, float maxPitch)
    {
        SetRandomPitch(audioSource, minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
    }

    public static void PlayOneShotFromArray(this AudioSource audioSource, AudioClip[] clips)
    {
        var clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }

    public static void PlayOneShotWithRandomPitchFromArray(this AudioSource audioSource, AudioClip[] clips, float minPitch, float maxPitch)
    {
        SetRandomPitch(audioSource, minPitch, maxPitch);
        var clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }

    private static void SetRandomPitch(AudioSource audioSource, float minPitch, float maxPitch)
    {
        float pitch = Random.Range(minPitch, maxPitch);
        audioSource.pitch = pitch;
    }
}
