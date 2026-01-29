using System;
using System.Collections;
using UnityEngine;

public class PlayerActionSystem : MonoBehaviour
{
    public static PlayerActionSystem Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private MonoBehaviour[] disableWhileBusy;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private ActionOverlayUI overlay;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = true;

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        Instance = this;
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    public bool TryStartTimed(float duration, string text, Action onComplete)
    {
        if (IsBusy)
        {
            if (debugLogs) Debug.Log($"[Action] Can't start '{text}' — player is busy.");
            return false;
        }

        StartCoroutine(Run(duration, text, onComplete));
        return true;
    }

    private IEnumerator Run(float duration, string text, Action onComplete)
    {
        IsBusy = true;

        if (debugLogs) Debug.Log($"[Action] START '{text}' ({duration:0.0}s)");

        foreach (var c in disableWhileBusy)
            if (c != null) c.enabled = false;

        if (rb != null) rb.linearVelocity = Vector3.zero;

        promptUI?.Hide();
        overlay?.Show(text);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            overlay?.SetProgress(duration <= 0f ? 1f : t / duration);
            yield return null;
        }

        overlay?.HideInstant();

        onComplete?.Invoke();

        foreach (var c in disableWhileBusy)
            if (c != null) c.enabled = true;

        IsBusy = false;

        if (debugLogs) Debug.Log($"[Action] END '{text}'");
    }
}
