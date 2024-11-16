using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_AudioMixerSlider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider slider;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string audioParameter;
    [SerializeField] float multiplier = 25;

    public void SetupSlider()
    {
        slider.onValueChanged.AddListener(SliderValue);
        slider.minValue = 0.001f;
        slider.value = PlayerPrefs.GetFloat(audioParameter, slider.value);

    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(audioParameter, slider.value);
    }
    void SliderValue(float value)
    {
        audioMixer.SetFloat(audioParameter, Mathf.Log10(value) * multiplier);
    }
}
