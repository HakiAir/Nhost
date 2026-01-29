using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionOverlayUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private Image progressFill;

    private void Awake()
    {
        HideInstant();
    }

    public void Show(string text)
    {
        gameObject.SetActive(true);

        if (group != null)
        {
            group.alpha = 1f;
            group.blocksRaycasts = true;
            group.interactable = false;
        }

        if (actionText != null) actionText.text = text;
        SetProgress(0f);
    }

    public void SetProgress(float t01)
    {
        if (progressFill != null)
            progressFill.fillAmount = Mathf.Clamp01(t01);
    }

    public void HideInstant()
    {
        if (group != null)
        {
            group.alpha = 0f;
            group.blocksRaycasts = false;
            group.interactable = false;
        }

        gameObject.SetActive(false);
    }
}
