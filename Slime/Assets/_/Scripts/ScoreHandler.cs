using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private Slime slime;
    [SerializeField] private GameStateController gameStateController;



    public event System.Action<int> CurrentScoreUpdated;
    public event System.Action<int> BestScoreUpdated;
    public event System.Action<int> FinalScoreUpdated;
    public event System.Action<int> PerfectScoreUpdated;



    private int bestScore = 0;
    private int currentScore = 0;
    private int perfectScore = 0;



    private void Start()
    {
        bestScore = SaveLoad.LoadInt("BestScore", 0);
        BestScoreUpdated?.Invoke(bestScore);
    }



    private void OnEnable()
    {
        slime.Landed += IncreaseCurrentScore;
        slime.PerfectLanding += IncereasePerfectScore;
        slime.NotPerfectLanding += ResetPerfectScore;
        gameStateController.GameOver += CheckFinalScore;
    }



    private void OnDisable()
    {
        slime.Landed -= IncreaseCurrentScore;
        slime.PerfectLanding -= IncereasePerfectScore;
        slime.NotPerfectLanding -= ResetPerfectScore;
        gameStateController.GameOver -= CheckFinalScore;
    }



    private void IncreaseCurrentScore()
    {
        currentScore += 1 + perfectScore;
        CurrentScoreUpdated?.Invoke(currentScore);
    }



    private void IncereasePerfectScore()
    {
        perfectScore++;
        PerfectScoreUpdated?.Invoke(perfectScore);
    }



    private void ResetPerfectScore()
    {
        perfectScore = 0;
        PerfectScoreUpdated?.Invoke(perfectScore);
    }



    private void CheckFinalScore()
    {
        FinalScoreUpdated?.Invoke(currentScore);

        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            SaveLoad.SaveInt("BestScore", bestScore);
        }
    }
}
