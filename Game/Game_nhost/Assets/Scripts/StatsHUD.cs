using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsHUD : MonoBehaviour
{
    [SerializeField] private Slider sleepSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider focusSlider;

    [SerializeField] private TMP_Text sleepText;
    [SerializeField] private TMP_Text hungerText;
    [SerializeField] private TMP_Text focusText;

    private PlayerStats stats;

    private void Start()
    {
        stats = PlayerStats.Instance;
        if (stats == null)
        {
            Debug.LogError("PlayerStats.Instance not found. Add PlayerStats to Player.");
            enabled = false;
            return;
        }

        stats.OnChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (stats != null)
            stats.OnChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        sleepSlider.value = stats.Sleep;
        hungerSlider.value = stats.Hunger;
        focusSlider.value = stats.Focus;

        sleepText.text = $"Сон {Mathf.RoundToInt(stats.Sleep)}";
        hungerText.text = $"Еда {Mathf.RoundToInt(stats.Hunger)}";
        focusText.text = $"Фокус {Mathf.RoundToInt(stats.Focus)}";
    }
}
