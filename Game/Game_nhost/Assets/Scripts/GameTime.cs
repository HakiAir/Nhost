using System;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public static GameTime Instance { get; private set; }

    [Header("Start time")]
    [SerializeField, Range(1, 9999)] private int startDay = 1;
    [SerializeField, Range(0, 23)] private int startHour = 8;
    [SerializeField, Range(0, 59)] private int startMinute = 0;

    [Header("Speed")]
    [Tooltip("Сколько РЕАЛЬНЫХ секунд = 1 ИГРОВАЯ минута")]
    [SerializeField, Min(0.01f)] private float realSecondsPerGameMinute = 1.0f;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = false;

    public int Day { get; private set; }
    public int Hour { get; private set; }
    public int Minute { get; private set; }

    public int TotalMinutesOfDay => Hour * 60 + Minute;

    public event Action OnMinuteChanged;

    private float acc;
    private bool paused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Day = startDay;
        Hour = startHour;
        Minute = startMinute;
    }

    private void Update()
    {
        if (paused) return;

        acc += Time.deltaTime;

        while (acc >= realSecondsPerGameMinute)
        {
            acc -= realSecondsPerGameMinute;
            AddMinutes(1);
        }
    }

    public void SetPaused(bool value) => paused = value;

    public void AddMinutes(int minutes)
    {
        if (minutes == 0) return;

        int total = Hour * 60 + Minute + minutes;

        while (total < 0)
        {
            total += 24 * 60;
            Day = Mathf.Max(1, Day - 1);
        }

        while (total >= 24 * 60)
        {
            total -= 24 * 60;
            Day += 1;
        }

        Hour = total / 60;
        Minute = total % 60;

        if (debugLogs) Debug.Log($"[GameTime] Day {Day} {Hour:00}:{Minute:00}");

        OnMinuteChanged?.Invoke();
    }

    public void SetTime(int day, int hour, int minute)
    {
        Day = Mathf.Max(1, day);
        Hour = Mathf.Clamp(hour, 0, 23);
        Minute = Mathf.Clamp(minute, 0, 59);

        if (debugLogs) Debug.Log($"[GameTime] SetTime -> Day {Day} {Hour:00}:{Minute:00}");

        OnMinuteChanged?.Invoke();
    }

    public void AdvanceToNextDayAndWakeAt0830()
    {
        Day += 1;
        Hour = 8;
        Minute = 30;

        if (debugLogs) Debug.Log($"[GameTime] Wake -> Day {Day} {Hour:00}:{Minute:00}");

        OnMinuteChanged?.Invoke();
    }

    public string GetClockString(bool withDay = true)
    {
        return withDay
            ? $"День {Day}  {Hour:00}:{Minute:00}"
            : $"{Hour:00}:{Minute:00}";
    }
}
