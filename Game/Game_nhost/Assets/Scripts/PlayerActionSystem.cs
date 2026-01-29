using System;
using System.Collections;
using UnityEngine;

public class PlayerActionSystem : MonoBehaviour
{
    public static PlayerActionSystem Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private MonoBehaviour[] disableWhileBusy; // движение/камера (НЕ PlayerInteractor)
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private ActionOverlayUI overlay;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = true;

    public bool IsBusy { get; private set; }

    private bool manualMode;

    private void Awake()
    {
        Instance = this;
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    // ====== TIMED (как раньше) ======
    public bool TryStartTimed(float duration, string text, Action onComplete)
    {
        if (IsBusy)
        {
            if (debugLogs) Debug.Log($"[Action] Can't start '{text}' — player is busy.");
            return false;
        }

        StartCoroutine(RunTimed(duration, text, onComplete));
        return true;
    }

    private IEnumerator RunTimed(float duration, string text, Action onComplete)
    {
        BeginBusy(showOverlay: true, overlayText: text);
        if (debugLogs) Debug.Log($"[Action] START '{text}' ({duration:0.0}s)");

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            overlay?.SetProgress(duration <= 0f ? 1f : t / duration);
            yield return null;
        }

        overlay?.HideInstant();
        onComplete?.Invoke();

        EndBusy();

        if (debugLogs) Debug.Log($"[Action] END '{text}'");
    }

    // ====== MANUAL (старт/стоп игроком) ======
    public bool TryBeginManual(bool showOverlay, string overlayText = "")
    {
        if (IsBusy)
        {
            if (debugLogs) Debug.Log("[Action] Can't begin manual — player is busy.");
            return false;
        }

        manualMode = true;
        BeginBusy(showOverlay, overlayText);

        if (debugLogs) Debug.Log("[Action] MANUAL BEGIN");
        return true;
    }

    public void EndManual()
    {
        if (!manualMode) return;

        manualMode = false;
        overlay?.HideInstant();
        EndBusy();

        if (debugLogs) Debug.Log("[Action] MANUAL END");
    }

    // ====== common lock/unlock ======
    private void BeginBusy(bool showOverlay, string overlayText)
    {
        IsBusy = true;

        foreach (var c in disableWhileBusy)
        {
            if (c == null) continue;

            // PlayerInteractor лучше НЕ выключать, чтобы триггеры не ломались.
            if (c is PlayerInteractor) continue;

            c.enabled = false;
        }

        if (rb != null) rb.linearVelocity = Vector3.zero;

        promptUI?.Hide();

        if (showOverlay)
        {
            overlay?.Show(overlayText);
        }
        else
        {
            overlay?.HideInstant();
        }
    }

    private void EndBusy()
    {
        foreach (var c in disableWhileBusy)
        {
            if (c == null) continue;

            if (c is PlayerInteractor) continue;

            c.enabled = true;
        }

        IsBusy = false;
    }
}
