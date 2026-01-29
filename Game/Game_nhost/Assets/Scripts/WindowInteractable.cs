using UnityEngine;
using UnityEngine.InputSystem;

public class WindowInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => isActive ? "Закончить (E)" : "Открыть окно";

    [Header("Focus restore")]
    [SerializeField, Min(1)] private int focusFullRestoreMinutes = 5; // <n> внутриигровых минут до 100%

    [Header("Camera")]
    [SerializeField] private Transform viewPoint;        // Empty перед окном
    [SerializeField] private Transform cameraTransform;  // обычно Main Camera
    [SerializeField, Min(0f)] private float moveDuration = 0.25f;

    [Header("Cooldown / Exit guard")]
    [Tooltip("Кулдаун до повторного начала взаимодействия (после выхода)")]
    [SerializeField, Min(0f)] private float reenterCooldownSeconds = 0.75f;

    [Tooltip("Сколько секунд после входа игнорировать выход по E (чтобы не ловить тот же E)")]
    [SerializeField, Min(0f)] private float exitInputDelaySeconds = 0.2f;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = true;

    private bool isActive;

    // cooldown
    private float nextStartTime;

    // exit guard
    private float exitAllowedTime;
    private bool waitForERelease;

    // Camera restore
    private Transform camParentBefore;
    private Vector3 camLocalPosBefore;
    private Quaternion camLocalRotBefore;

    private Coroutine moveRoutine;

    // Focus restore
    private float startFocus;
    private int minutesSpent;

    public void Interact()
    {
        if (!isActive) BeginWindowMode();
        else EndWindowMode();
    }

    private void BeginWindowMode()
    {
        if (Time.time < nextStartTime)
        {
            if (debugLogs) Debug.Log($"[Window] Re-enter cooldown: {nextStartTime - Time.time:0.00}s left");
            return;
        }

        if (viewPoint == null)
        {
            if (debugLogs) Debug.LogWarning("[Window] viewPoint is not assigned.");
            return;
        }

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (cameraTransform == null)
        {
            if (debugLogs) Debug.LogWarning("[Window] cameraTransform is not assigned and Camera.main not found.");
            return;
        }

        var actions = PlayerActionSystem.Instance;
        var time = GameTime.Instance;
        var stats = PlayerStats.Instance;

        if (actions == null || time == null || stats == null)
        {
            if (debugLogs) Debug.LogWarning("[Window] Missing PlayerActionSystem/GameTime/PlayerStats.");
            return;
        }

        if (!actions.TryBeginManual(showOverlay: false))
            return;

        isActive = true;

        // --- Exit guard: игнорим "тот же E" ---
        exitAllowedTime = Time.time + exitInputDelaySeconds;

        var kb = Keyboard.current;
        waitForERelease = (kb != null && kb[Key.E].isPressed); // если E зажата в момент входа, ждём отпускания

        // --- Save camera relative to player ---
        camParentBefore = cameraTransform.parent;
        camLocalPosBefore = cameraTransform.localPosition;
        camLocalRotBefore = cameraTransform.localRotation;

        // detach camera from player while watching window
        cameraTransform.SetParent(null, true);

        // Focus start
        startFocus = stats.Focus;
        minutesSpent = 0;

        // Subscribe minutes
        time.OnMinuteChanged += OnMinuteChanged;

        // Stop previous move routine if any
        if (moveRoutine != null) StopCoroutine(moveRoutine);

        // Move to window viewpoint
        if (moveDuration > 0f)
            moveRoutine = StartCoroutine(MoveCamera(cameraTransform, viewPoint.position, viewPoint.rotation, moveDuration));
        else
            cameraTransform.SetPositionAndRotation(viewPoint.position, viewPoint.rotation);

        if (debugLogs)
            Debug.Log($"[Window] BEGIN. Focus start={startFocus:0}. Full in {focusFullRestoreMinutes} min.");
    }

    private void EndWindowMode()
    {
        if (!isActive) return;

        var actions = PlayerActionSystem.Instance;
        var time = GameTime.Instance;

        if (time != null)
            time.OnMinuteChanged -= OnMinuteChanged;

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        // Restore camera back to player
        if (cameraTransform != null)
        {
            cameraTransform.SetParent(camParentBefore, true);
            cameraTransform.localPosition = camLocalPosBefore;
            cameraTransform.localRotation = camLocalRotBefore;
        }

        isActive = false;
        actions?.EndManual();

        // cooldown before next start
        nextStartTime = Time.time + reenterCooldownSeconds;

        if (debugLogs) Debug.Log("[Window] END");
    }

    private void Update()
    {
        if (!isActive) return;

        var kb = Keyboard.current;
        if (kb == null) return;

        // пока не прошла задержка — игнорим выход
        if (Time.time < exitAllowedTime) return;

        // если входили на зажатой E — ждём отпускания E, и только потом разрешаем выход по нажатию
        if (waitForERelease)
        {
            if (!kb[Key.E].isPressed)
                waitForERelease = false;

            return;
        }

        // выход только игроком
        if (kb[Key.E].wasPressedThisFrame || kb.escapeKey.wasPressedThisFrame)
            EndWindowMode();
    }

    private void OnMinuteChanged()
    {
        if (!isActive) return;

        var stats = PlayerStats.Instance;
        if (stats == null) return;

        minutesSpent++;

        float missing = 100f - startFocus;
        if (missing <= 0f) return;

        float perMinute = missing / focusFullRestoreMinutes;
        float target = startFocus + perMinute * minutesSpent;

        stats.SetFocus(Mathf.Min(100f, target));

        if (debugLogs)
            Debug.Log($"[Window] +minute ({minutesSpent}/{focusFullRestoreMinutes}) -> Focus {stats.Focus:0}");
    }

    private System.Collections.IEnumerator MoveCamera(Transform cam, Vector3 toPos, Quaternion toRot, float dur)
    {
        float t = 0f;
        Vector3 fromPos = cam.position;
        Quaternion fromRot = cam.rotation;

        while (t < dur)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / dur);
            cam.position = Vector3.Lerp(fromPos, toPos, k);
            cam.rotation = Quaternion.Slerp(fromRot, toRot, k);
            yield return null;
        }

        cam.SetPositionAndRotation(toPos, toRot);
        moveRoutine = null;
    }

    private void OnDisable()
    {
        if (isActive) EndWindowMode();
    }
}
