using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePresenter : MonoBehaviour
{
    [SerializeField] private GameStateController stateController;
    [SerializeField] private Panels panels = new Panels();



    [System.Serializable]
    private struct Panels
    {
        public GameObject startPanel;
        public GameObject gameplayPanel;
        public GameObject pausePanel;
        public GameObject finalPanel;
        public GameObject settingsPanel;
    }



    private void OnEnable()
    {
        stateController.GameStarted += ShowGameplayPanel;
        stateController.GamePaused += ShowPausePanel;
        stateController.GameResumed += ShowGameplayPanel;
        stateController.GameOver += ShowFinalPanel;
    }



    private void OnDisable()
    {
        stateController.GameStarted -= ShowGameplayPanel;
        stateController.GamePaused -= ShowPausePanel;
        stateController.GameResumed -= ShowGameplayPanel;
        stateController.GameOver -= ShowFinalPanel;
    }



    public void ShowStartPanel()
    {
        panels.settingsPanel.SetActive(false);
        panels.startPanel.SetActive(true);
    }



    public void ShowSettingsPanel()
    {
        panels.startPanel.SetActive(false);
        panels.settingsPanel.SetActive(true);
    }



    private void ShowGameplayPanel()
    {
        panels.startPanel.SetActive(false);
        panels.pausePanel.SetActive(false);
        panels.gameplayPanel.SetActive(true);
    }



    private void ShowPausePanel()
    {
        panels.gameplayPanel.SetActive(false);
        panels.pausePanel.SetActive(true);
    }



    private void ShowFinalPanel()
    {
        panels.gameplayPanel.SetActive(false);
        panels.finalPanel.SetActive(true);
    }

}
