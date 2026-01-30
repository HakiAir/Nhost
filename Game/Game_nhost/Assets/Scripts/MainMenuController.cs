using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button continueButton;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AudioSettings.Load();

        if (continueButton != null)
            continueButton.interactable = SaveSystemStub.HasSave();
    }

    public void StartNewGame()
    {
        SaveSystemStub.ClearSave();          // новая игра — очищаем
        SceneManager.LoadScene(GameScenes.Gameplay);
    }

    public void ContinueGame()
    {
        if (!SaveSystemStub.HasSave()) return;
        SceneManager.LoadScene(GameScenes.Gameplay);
    }

    public void OpenSettings()
    {
        if (mainPanel) mainPanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
