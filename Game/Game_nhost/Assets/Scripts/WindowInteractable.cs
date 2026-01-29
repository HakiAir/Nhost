using UnityEngine;

public class WindowInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Открыть окно";

    [SerializeField] private float duration = 1.5f;
    [SerializeField] private float cooldown = 3f;
    [SerializeField] private float focusGain = 15f;

    private float nextUseTime;

    public void Interact()
    {
        if (Time.time < nextUseTime) return;

        var actions = PlayerActionSystem.Instance;
        if (actions == null) return;

        bool started = actions.TryStartTimed(duration, "Проветриваешь...", () =>
        {
            var s = PlayerStats.Instance;
            if (s == null) return;

            s.AddFocus(focusGain);
        });

        if (started) nextUseTime = Time.time + cooldown;
    }
}
