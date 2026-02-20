using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioParameters mixerParameters;
    [SerializeField] private float minVolume = -60f;

    private void Start()
    {
        SetMixerVolume(PlayerPrefs.GetFloat(mixerParameters.ToString(), 0.8f));
        slider.SetValueWithoutNotify(GetMixerVolume());
    }

    public void UpdateMixerVolume(float volume)
    {
        SetMixerVolume(volume);
    }
    
    private void SetMixerVolume(float volume)
    {
        float mixerVolume;
        if (volume == 0) mixerVolume = -80f;
        else mixerVolume = Mathf.Lerp(minVolume,0,volume);
        
        mixer.SetFloat(mixerParameters.ToString(), mixerVolume);
    }

    private float GetMixerVolume()
    {
        mixer.GetFloat(mixerParameters.ToString(), out var volume);
        return Mathf.Approximately(volume,-80f) ? 0f : Mathf.Lerp(1,0,volume/minVolume);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(mixerParameters.ToString(),GetMixerVolume());
    }
}

public enum AudioParameters
{
    MasterVolume,
    MusicVolume,
    EffectsVolume
}