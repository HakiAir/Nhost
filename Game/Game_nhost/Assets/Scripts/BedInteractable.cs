using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Спать";

    [SerializeField] private float duration = 3f;
    [SerializeField] private float cooldown = 6f;

    [SerializeField] private float sleepGain = 25f;
    [SerializeField] private float focusGain = 10f;

    [SerializeField] private bool debugLogs = true;

    private float nextUseTime;

    public void Interact()
    {
        if (Time.time < nextUseTime)
        {
            if (debugLogs) Debug.Log($"[Bed] Cooldown: {nextUseTime - Time.time:0.0}s left");
            return;
        }

        var actions = PlayerActionSystem.Instance;
        if (actions == null) return;

        bool started = actions.TryStartTimed(duration, "Спишь...", () =>
        {
            var s = PlayerStats.Instance;
            if (s == null) return;

            s.AddSleep(sleepGain);
            s.AddFocus(focusGain);

            if (debugLogs) Debug.Log($"[Bed] Applied: +Sleep {sleepGain}, +Focus {focusGain}");
        });

        if (started)
        {
            nextUseTime = Time.time + cooldown;
            if (debugLogs) Debug.Log($"[Bed] Started (cooldown {cooldown:0.0}s)");
        }
        else
        {
            if (debugLogs) Debug.Log("[Bed] Not started (player busy?)");
        }
    }
}
