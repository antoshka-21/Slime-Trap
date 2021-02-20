using UnityEngine;
using UnityEngine.UI;

public class ThemeUnlocker : MonoBehaviour
{
    [SerializeField] private ThemeChanger themeChanger;
    [SerializeField] private ScoreHandler scoreHandler;
    [SerializeField] private Platform upperPlatform;
    [SerializeField] private GameObject lockGO;
    [SerializeField] private Text scoreboardUpperText;
    [SerializeField] private Text scoreboardText;

    private void OnEnable()
    {
        scoreHandler.BestScoreUpdated += CompareScores;
    }

    private void OnDisable()
    {
        scoreHandler.BestScoreUpdated -= CompareScores;
    }

    private void CompareScores(int bestScore)
    {
        if (bestScore >= themeChanger.currentTheme.scoreToUnlock)
        {
            Unlock();
        }

        else
        {
            Lock();
        }
    }

    private void Unlock()
    {
        upperPlatform.SetSpeed(1f);
    }

    private void Lock()
    {
        scoreboardUpperText.text = "target";
        scoreboardText.text = "" + themeChanger.currentTheme.scoreToUnlock;
        lockGO.SetActive(true);

        themeChanger.SetID(0);
    }
}
