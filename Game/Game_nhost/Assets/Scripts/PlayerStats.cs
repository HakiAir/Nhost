using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Initial (0-100)")]
    [SerializeField, Range(0, 100)] private float sleep = 80f;
    [SerializeField, Range(0, 100)] private float hunger = 80f;
    [SerializeField, Range(0, 100)] private float focus = 80f;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = false;

    public float Sleep => sleep;
    public float Hunger => hunger;
    public float Focus => focus;

    public event Action OnChanged;

    private GameTime time;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        time = GameTime.Instance;
        if (time == null)
        {
            Debug.LogError("[PlayerStats] GameTime.Instance not found. Add GameTime to a GameObject in the scene.");
            enabled = false;
            return;
        }

        time.OnMinuteChanged += OnGameMinute;
    }

    private void OnDestroy()
    {
        if (time != null)
            time.OnMinuteChanged -= OnGameMinute;
    }

    private void OnGameMinute()
    {
        // 1 игровая минута прошла => снимаем по (perHour/60)
        ApplyDrainForMinutes(1);
    }

    private void ApplyDrainForMinutes(int minutes)
    {
        if (minutes <= 0) return;

        float hDrain = GameplayBalance.Stats.HungerDrainPerHour * (minutes / 60f);
        float sDrain = GameplayBalance.Stats.SleepDrainPerHour * (minutes / 60f);
        float fDrain = GameplayBalance.Stats.FocusDrainPerHour * (minutes / 60f);

        bool changed = false;

        changed |= Subtract(ref hunger, hDrain);
        changed |= Subtract(ref sleep, sDrain);
        changed |= Subtract(ref focus, fDrain);

        if (changed)
        {
            if (debugLogs)
                Debug.Log($"[Stats] -{minutes}min => Hunger {hunger:0}, Sleep {sleep:0}, Focus {focus:0}");

            OnChanged?.Invoke();
        }
    }

    private bool Subtract(ref float value, float amount)
    {
        float old = value;
        value = Mathf.Clamp(value - amount, 0f, 100f);
        return !Mathf.Approximately(old, value);
    }

    // --- Add ---
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

    // --- Set ---
    public void SetSleep(float value)
    {
        sleep = Mathf.Clamp(value, 0f, 100f);
        OnChanged?.Invoke();
    }

    public void SetHunger(float value)
    {
        hunger = Mathf.Clamp(value, 0f, 100f);
        OnChanged?.Invoke();
    }

    public void SetFocus(float value)
    {
        focus = Mathf.Clamp(value, 0f, 100f);
        OnChanged?.Invoke();
    }

 
    public void ApplyTimeSkipMinutes(int minutes)
    {
        ApplyDrainForMinutes(minutes);
    }
}
