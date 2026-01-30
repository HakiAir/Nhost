using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] private GameObject returnPanel;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void OnEnable()
    {
        AudioSettings.Load();

        if (masterSlider) masterSlider.SetValueWithoutNotify(AudioSettings.Master);
        if (sfxSlider) sfxSlider.SetValueWithoutNotify(AudioSettings.Sfx);
        if (musicSlider) musicSlider.SetValueWithoutNotify(AudioSettings.Music);

        if (masterSlider) masterSlider.onValueChanged.AddListener(AudioSettings.SetMaster);
        if (sfxSlider) sfxSlider.onValueChanged.AddListener(AudioSettings.SetSfx);
        if (musicSlider) musicSlider.onValueChanged.AddListener(AudioSettings.SetMusic);
    }

    private void OnDisable()
    {
        if (masterSlider) masterSlider.onValueChanged.RemoveListener(AudioSettings.SetMaster);
        if (sfxSlider) sfxSlider.onValueChanged.RemoveListener(AudioSettings.SetSfx);
        if (musicSlider) musicSlider.onValueChanged.RemoveListener(AudioSettings.SetMusic);
    }

    public void Back()
    {
        gameObject.SetActive(false);
        if (returnPanel) returnPanel.SetActive(true);
    }
}
