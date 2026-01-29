using UnityEngine;

public class FridgeInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Поесть";

    [SerializeField] private float duration = 2f;
    [SerializeField] private float cooldown = 4f;
    [SerializeField] private float hungerGain = 30f;

    [SerializeField] private bool debugLogs = true;

    private float nextUseTime;

    public void Interact()
    {
        if (Time.time < nextUseTime)
        {
            if (debugLogs) Debug.Log($"[Fridge] Cooldown: {nextUseTime - Time.time:0.0}s left");
            return;
        }

        var actions = PlayerActionSystem.Instance;
        if (actions == null) return;

        bool started = actions.TryStartTimed(duration, "Ешь...", () =>
        {
            var s = PlayerStats.Instance;
            if (s == null) return;

            s.AddHunger(hungerGain);

            if (debugLogs) Debug.Log($"[Fridge] Applied: +Hunger {hungerGain}");
        });

        if (started)
        {
            nextUseTime = Time.time + cooldown;
            if (debugLogs) Debug.Log($"[Fridge] Started (cooldown {cooldown:0.0}s)");
        }
        else
        {
            if (debugLogs) Debug.Log("[Fridge] Not started (player busy?)");
        }
    }
}
