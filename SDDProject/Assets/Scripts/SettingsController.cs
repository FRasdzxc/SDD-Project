using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject confirmationPanel;

    public void SetGraphics(int graphics)
    {
        QualitySettings.SetQualityLevel(graphics);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        confirmationPanel.SetActive(true);
    }

    public void FinalExitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        confirmationPanel.SetActive(false);
    }
}
