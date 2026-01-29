using UnityEngine;

public class WindowInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Открыть окно";

    [SerializeField] private float focusGain = 15f;

    public void Interact()
    {
        var s = PlayerStats.Instance;
        if (s == null) return;

        s.AddFocus(focusGain);
        Debug.Log("Окно: +фокус");
    }
}
