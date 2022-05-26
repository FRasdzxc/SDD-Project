using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject pausePanel;

    public void Pause() // MainPanel PauseButton triggers this function
    {
        Time.timeScale = 0;
        mainPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void Resume() // MainPanel ResumeButton triggers this function
    {
        Time.timeScale = 1;
        mainPanel.SetActive(true);
        pausePanel.SetActive(false);
    }
}
