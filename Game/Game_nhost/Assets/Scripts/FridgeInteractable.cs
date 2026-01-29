using UnityEngine;

public class FridgeInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Поесть";

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

        bool started = actions.TryStartTimed(GameplayBalance.Actions.FridgeDuration, "Ешь...", () =>
        {
            var s = PlayerStats.Instance;
            if (s == null) return;

            s.AddHunger(hungerGain);

            if (debugLogs) Debug.Log($"[Fridge] Applied: +Hunger {hungerGain}");
        });

        if (started)
        {
            nextUseTime = Time.time + GameplayBalance.Actions.FridgeCooldown;
            if (debugLogs) Debug.Log($"[Fridge] Started (cooldown {GameplayBalance.Actions.FridgeCooldown:0.0}s)");
        }
    }
}
