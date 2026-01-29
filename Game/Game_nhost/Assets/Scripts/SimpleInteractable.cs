using UnityEngine;

public class SimpleInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Взаимодействовать";
    [TextArea][SerializeField] private string debugMessage = "Interact!";

    public string Prompt => prompt;

    public void Interact()
    {
        Debug.Log($"{name}: {debugMessage}");
    }
}