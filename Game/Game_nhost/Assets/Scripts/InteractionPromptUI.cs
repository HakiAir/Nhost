using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    private void Awake()
    {
        Hide();
    }

    public void Show(string prompt)
    {
        gameObject.SetActive(true);
        if (label != null)
            label.text = $"E - {prompt}";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
