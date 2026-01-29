using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Спать";

    [SerializeField] private float sleepGain = 25f;
    [SerializeField] private float focusGain = 10f;

    public void Interact()
    {
        var s = PlayerStats.Instance;
        if (s == null) return;

        s.AddSleep(sleepGain);
        s.AddFocus(focusGain);
        Debug.Log("Кровать: +сон, +фокус");
    }
}
