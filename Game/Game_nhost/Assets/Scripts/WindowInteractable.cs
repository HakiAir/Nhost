using UnityEngine;

public class WindowInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Открыть окно";

    [SerializeField] private float focusGain = 15f;
    [SerializeField] private bool debugLogs = true;

    private float nextUseTime;

    public void Interact()
    {
        if (Time.time < nextUseTime)
        {
            if (debugLogs) Debug.Log($"[Window] Cooldown: {nextUseTime - Time.time:0.0}s left");
            return;
        }

        var actions = PlayerActionSystem.Instance;
        if (actions == null) return;

        bool started = actions.TryStartTimed(GameplayBalance.Actions.WindowDuration, "Проветриваешь...", () =>
        {
            var s = PlayerStats.Instance;
            if (s == null) return;

            s.AddFocus(focusGain);

            if (debugLogs) Debug.Log($"[Window] Applied: +Focus {focusGain}");
        });

        if (started)
        {
            nextUseTime = Time.time + GameplayBalance.Actions.WindowCooldown;
            if (debugLogs) Debug.Log($"[Window] Started (cooldown {GameplayBalance.Actions.WindowCooldown:0.0}s)");
        }
    }
}
