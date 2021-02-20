using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePresenter : MonoBehaviour
{
    [SerializeField] private ScoreHandler scoreHandler;
    [SerializeField] private GameStateController gameStateController;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Text perfectScoreTxt;



    private int currentDisplayedScore;
    private int startScoreUpdateStep;



    private void OnEnable()
    {
        scoreHandler.BestScoreUpdated += RefreshScore;
        gameStateController.GameStarted += ResetScoreboard;
        scoreHandler.CurrentScoreUpdated += RefreshScore;
        scoreHandler.PerfectScoreUpdated += RefreshPerfectScore;
    }



    private void OnDisable()
    {
        scoreHandler.BestScoreUpdated -= RefreshScore;
        gameStateController.GameStarted -= ResetScoreboard;
        scoreHandler.CurrentScoreUpdated -= RefreshScore;
        scoreHandler.PerfectScoreUpdated -= RefreshPerfectScore;
    }



    private void RefreshScore(int score)
    {
        currentDisplayedScore = score;
        RefreshScoreboard(score);
    }



    private void ResetScoreboard()
    {
        StartCoroutine(SmoothlyResetScore());
    }



    private void RefreshScoreboard(int score)
    {
        currentDisplayedScore = score;

             if (score < 10)    scoreTxt.text = "00" + score;
        else if (score < 100)   scoreTxt.text = "0" + score;
        else                    scoreTxt.text = "" + score;
    }



    private void RefreshPerfectScore(int score)
    {
        if (score != 0)
        {
            perfectScoreTxt.gameObject.SetActive(true);
            perfectScoreTxt.text = "perf x" + score;
        }

        else
        {
            perfectScoreTxt.gameObject.SetActive(false);
        }
    }



    private IEnumerator SmoothlyResetScore()
    {
        startScoreUpdateStep = currentDisplayedScore / 100 + 1;

        while (currentDisplayedScore > 0)
        {
            currentDisplayedScore -= startScoreUpdateStep;

            if (currentDisplayedScore < 0)
                currentDisplayedScore = 0;

            RefreshScoreboard(currentDisplayedScore);

            yield return null;
        }
    }
}
