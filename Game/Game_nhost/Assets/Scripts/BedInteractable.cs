using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    // Состояния:
    // доступно: 21:00 - 09:59
    // заблокировано: 10:00 - 20:59

    [SerializeField] private bool debugLogs = true;

    private float nextUseTime;

    public string Prompt
    {
        get
        {
            var t = GameTime.Instance;
            if (t == null) return "Спать";

            return IsBedAvailableNow(t) ? "Спать" : "Кровать недоступна";
        }
    }

    public void Interact()
    {
        var time = GameTime.Instance;
        var stats = PlayerStats.Instance;

        if (time == null || stats == null)
        {
            if (debugLogs) Debug.LogWarning("[Bed] GameTime or PlayerStats not found.");
            return;
        }

        // Проверка окна доступности по времени
        if (!IsBedAvailableNow(time))
        {
            if (debugLogs) Debug.Log($"[Bed] Locked now ({time.Hour:00}:{time.Minute:00}). Available 21:00-08:29.");
            return;
        }

        // Кулдаун (по желанию, сейчас 0)
        if (Time.time < nextUseTime)
        {
            if (debugLogs) Debug.Log($"[Bed] Cooldown: {nextUseTime - Time.time:0.0}s left");
            return;
        }

        var actions = PlayerActionSystem.Instance;
        if (actions == null) return;

        // Время, когда игрок лёг спать (для расчёта концентрации)
        int sleepStartHour = time.Hour;
        int sleepStartMinute = time.Minute;

        // По желанию: чтобы время не тикало во время затемнения
        time.SetPaused(true);

        bool started = actions.TryStartTimed(GameplayBalance.Actions.BedDuration, "Спишь...", () =>
        {
            // 2) Наступает следующий игровой день, пробуждение в 08:30 (фикс)
            time.AdvanceToNextDayAndWakeAt0830();

            // 3) Сон восполняется до 100%
            stats.SetSleep(100f);

            // 4) Концентрация по формуле из SRS
            float targetFocus = CalculateFocusAfterSleep(sleepStartHour, sleepStartMinute);
            stats.SetFocus(targetFocus);

            time.SetPaused(false);

            if (debugLogs)
            {
                Debug.Log($"[Bed] Wake up: Day {time.Day} {time.Hour:00}:{time.Minute:00}");
                Debug.Log($"[Bed] Sleep -> 100, Focus -> {targetFocus:0}");
            }
        });

        if (started)
        {
            nextUseTime = Time.time + GameplayBalance.Actions.BedCooldown; // сейчас 0
            if (debugLogs) Debug.Log($"[Bed] Started at {sleepStartHour:00}:{sleepStartMinute:00}");
        }
        else
        {
            // если не стартануло (busy) — вернём паузу времени обратно
            time.SetPaused(false);
            if (debugLogs) Debug.Log("[Bed] Not started (player busy?)");
        }
    }

    private static bool IsBedAvailableNow(GameTime time)
    {
        int m = time.TotalMinutesOfDay;

        int start = 21 * 60;          // 21:00
        int end = 8 * 60 + 29;        // 08:29

        // окно через полночь: [21:00..23:59] U [00:00..08:29]
        return m >= start || m <= end;
    }

    private static float CalculateFocusAfterSleep(int hour, int minute)
    {
        // Если лёг до 23:00 включительно -> 100%
        if (hour < 23 || (hour == 23 && minute == 0))
            return 100f;

        // Если после 23:01 -> 100 - (n*10), где:
        // 23:01-00:00 -> n=1
        // 00:01-01:00 -> n=2
        // и т.д.
        int minutesSince23;
        if (hour == 23)
        {
            minutesSince23 = minute; // 23:01 => 1
        }
        else
        {
            // после полуночи: 00:00 должно быть 60 минут после 23:00
            minutesSince23 = (hour + 1) * 60 + minute; // 00:00 => 60, 01:00 => 120
        }

        int n = Mathf.CeilToInt(minutesSince23 / 60f);
        float focus = 100f - n * 10f;

        return Mathf.Clamp(focus, 0f, 100f);
    }
}
