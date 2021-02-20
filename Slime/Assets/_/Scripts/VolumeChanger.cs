using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeChanger : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixer;
    [SerializeField] private Slider slider;

    private float currentVolume;


    private void Awake()
    {
        currentVolume = SaveLoad.LoadFloat("Volume", 1f);
        SetVolume(currentVolume);
        slider.value = currentVolume;
    }

    public void ChangeVolume()
    {
        currentVolume = slider.value;
        SetVolume(currentVolume);
    }

    public void SaveValue()
    {
        SaveLoad.SaveFloat("Volume", currentVolume);
    }

    private void SetVolume(float value)
    {
        if (value > 0f)
        {
            mixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-40f, 0f, value));
        }

        else
        {
            mixer.audioMixer.SetFloat("MasterVolume", -80f);
        }
    }
}
