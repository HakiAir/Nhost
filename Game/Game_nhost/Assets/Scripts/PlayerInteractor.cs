using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private InteractionPromptUI promptUI;

    private readonly System.Collections.Generic.List<InteractZone> zones = new();
    private InteractZone currentZone;

    private void OnTriggerEnter(Collider other)
    {
        var zone = other.GetComponentInParent<InteractZone>();
        if (zone == null || zone.Interactable == null) return;

        if (!zones.Contains(zone))
            zones.Add(zone);

        SelectBestZone();
    }

    private void OnTriggerExit(Collider other)
    {
        var zone = other.GetComponentInParent<InteractZone>();
        if (zone == null) return;

        zones.Remove(zone);
        SelectBestZone();
    }

    private void SelectBestZone()
    {
        currentZone = zones.Count > 0 ? zones[zones.Count - 1] : null;

        if (currentZone != null)
            promptUI?.Show(currentZone.Interactable.Prompt);
        else
            promptUI?.Hide();
    }

    private void Update()
    {
        if (currentZone == null) return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentZone.Interactable.Interact();
        }
    }
}
