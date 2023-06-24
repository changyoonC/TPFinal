using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;

    private void Start()
    {
        bgmSlider.value = SoundManager.instance.bgmVolume;
        seSlider.value = SoundManager.instance.seVolume;

        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        seSlider.onValueChanged.AddListener(OnSEVolumeChanged);
    }

    public void OnBGMVolumeChanged(float value)
    {
        SoundManager.instance.SetBGMVolume(value);
    }

    public void OnSEVolumeChanged(float value)
    {
        SoundManager.instance.SetSEVolume(value);
    }
}