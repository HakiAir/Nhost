using UnityEngine;

public class FridgeInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Поесть";

    [SerializeField] private float hungerGain = 30f;

    public void Interact()
    {
        var s = PlayerStats.Instance;
        if (s == null) return;

        s.AddHunger(hungerGain);
        Debug.Log("Холодильник: +еда");
    }
}
