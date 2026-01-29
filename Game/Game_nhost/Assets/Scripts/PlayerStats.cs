using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Initial (0-100)")]
    [SerializeField, Range(0, 100)] private float sleep = 80f;
    [SerializeField, Range(0, 100)] private float hunger = 80f;
    [SerializeField, Range(0, 100)] private float focus = 80f;

    [Header("Decay per minute")]
    [SerializeField] private float sleepDecayPerMin = 1.0f;
    [SerializeField] private float hungerDecayPerMin = 2.0f;
    [SerializeField] private float focusDecayPerMin = 3.0f;

    public float Sleep => sleep;
    public float Hunger => hunger;
    public float Focus => focus;

    public event Action OnChanged;

    private float _tick;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        // обновляем раз в 0.25 сек, чтобы не спамить UI каждый кадр
        _tick += Time.deltaTime;
        if (_tick < 0.25f) return;
        _tick = 0f;

        bool changed = false;

        changed |= ApplyDecay(ref sleep, sleepDecayPerMin);
        changed |= ApplyDecay(ref hunger, hungerDecayPerMin);
        changed |= ApplyDecay(ref focus, focusDecayPerMin);

        if (changed) OnChanged?.Invoke();
    }

    private bool ApplyDecay(ref float value, float perMin)
    {
        float old = value;
        value = Mathf.Clamp(value - perMin * (0.25f / 60f), 0f, 100f);
        return !Mathf.Approximately(old, value);
    }

    public void AddSleep(float amount)
    {
        sleep = Mathf.Clamp(sleep + amount, 0f, 100f);
        OnChanged?.Invoke();
    }

    public void AddHunger(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0f, 100f);
        OnChanged?.Invoke();
    }

    public void AddFocus(float amount)
    {
        focus = Mathf.Clamp(focus + amount, 0f, 100f);
        OnChanged?.Invoke();
    }
}
