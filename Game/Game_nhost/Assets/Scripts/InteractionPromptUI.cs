using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject root;   // контейнер подсказки (панель)
    [SerializeField] private TMP_Text label;    // TMP текст подсказки

    private bool manualOverride;

    private void Reset() => AutoWire();

    private void Awake()
    {
        AutoWire();
        manualOverride = false;
        HideForce();
    }

    private void AutoWire()
    {
        if (root == null) root = gameObject;
        if (label == null) label = GetComponentInChildren<TMP_Text>(true);
    }

    // обычная подсказка от Interactor
    public void Show(string message)
    {
        if (manualOverride) return;

        if (root != null) root.SetActive(true);

        if (label != null)
        {
            var msg = (message ?? "").Trim();

            // Если уже начинается с "E" — не дублируем
            if (!string.IsNullOrEmpty(msg) && !msg.StartsWith("E"))
                label.text = $"E - {msg}";
            else
                label.text = msg;
        }
    }

    public void Hide()
    {
        if (manualOverride) return;
        HideForce();
    }

    // ручная подсказка (например "E — Закончить")
    public void SetManualHint(string message)
    {
        manualOverride = true;

        if (root != null) root.SetActive(true);
        if (label != null) label.text = message;
    }

    public void ClearManualHint()
    {
        manualOverride = false;
        HideForce();
    }

    private void HideForce()
    {
        if (root != null) root.SetActive(false);
    }
}
