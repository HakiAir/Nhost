using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    private InteractZone currentZone;

    private void OnTriggerEnter(Collider other)
    {
        var zone = other.GetComponentInParent<InteractZone>();
        if (zone == null || zone.Interactable == null) return;

        currentZone = zone;
        Debug.Log($"В зоне: {zone.Interactable.Prompt} (нажми E)");
    }

    private void OnTriggerExit(Collider other)
    {
        var zone = other.GetComponentInParent<InteractZone>();
        if (zone == null) return;

        if (currentZone == zone)
        {
            currentZone = null;
            Debug.Log("Вышел из зоны");
        }
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
