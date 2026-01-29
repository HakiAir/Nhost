using TMPro;
using UnityEngine;

public class GameClockHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text clockText;
    [SerializeField] private bool showDay = true;

    private GameTime time;

    private void Start()
    {
        time = GameTime.Instance;
        if (time == null)
        {
            Debug.LogError("GameTime.Instance not found. Add GameTime to a GameObject in the scene.");
            enabled = false;
            return;
        }

        time.OnMinuteChanged += UpdateClock;
        UpdateClock();
    }

    private void OnDestroy()
    {
        if (time != null)
            time.OnMinuteChanged -= UpdateClock;
    }

    private void UpdateClock()
    {
        if (clockText != null)
            clockText.text = time.GetClockString(showDay);
    }
}
