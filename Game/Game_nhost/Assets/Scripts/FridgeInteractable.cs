using UnityEngine;

public class FridgeInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Поесть";

    [SerializeField] private float duration = 2f;
    [SerializeField] private float cooldown = 4f;
    [SerializeField] private float hungerGain = 30f;

    private float nextUseTime;

    public void Interact()
    {
        if (Time.time < nextUseTime) return;

        var actions = PlayerActionSystem.Instance;
        if (actions == null) return;

        bool started = actions.TryStartTimed(duration, "Ешь...", () =>
        {
            var s = PlayerStats.Instance;
            if (s == null) return;

            s.AddHunger(hungerGain);
        });

        if (started) nextUseTime = Time.time + cooldown;
    }
}
