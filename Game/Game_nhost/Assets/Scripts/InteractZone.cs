using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractZone : MonoBehaviour
{
    [SerializeField] private MonoBehaviour interactableBehaviour;

    public IInteractable Interactable => interactableBehaviour as IInteractable;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        // Автопоиск интерактива на родителе
        interactableBehaviour = GetComponentInParent<MonoBehaviour>();
    }
}
