using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Спать";

    [SerializeField] private float duration = 3f;
    [SerializeField] private float cooldown = 6f;

    [SerializeField] private float sleepGain = 25f;
    [SerializeField] private float focusGain = 10f;

    private float nextUseTime;

    public void Interact()
    {
        if (Time.time < nextUseTime) return;

        var actions = PlayerActionSystem.Instance;
        if (actions == null) return;

        bool started = actions.TryStartTimed(duration, "Спишь...", () =>
        {
            var s = PlayerStats.Instance;
            if (s == null) return;

            s.AddSleep(sleepGain);
            s.AddFocus(focusGain);
        });

        if (started) nextUseTime = Time.time + cooldown;
    }
}
