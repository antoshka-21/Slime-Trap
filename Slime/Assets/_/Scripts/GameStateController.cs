using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private Slime slime;
    [SerializeField] private float delayBeforeRestart;



    public event System.Action GameStarted;
    public event System.Action GamePaused;
    public event System.Action GameResumed;
    public event System.Action GameOver;
    public event System.Action GameStartedToRestart;



    public GameState CurrentState { get; private set; } = GameState.NotStarted;



    private void OnEnable()
    {
        inputHandler.PushDown += StartGame;
        slime.Lost += FinishGame;
    }



    private void OnDisable()
    {
        inputHandler.PushDown -= StartGame;
        slime.Lost -= FinishGame;
    }



    private void StartGame()
    {
        if (CurrentState == GameState.NotStarted)
        {
            CurrentState = GameState.Started;
            GameStarted?.Invoke();
        }
    }



    public void Pause()
    {
        CurrentState = GameState.Paused;
        Time.timeScale = 0f;
        GamePaused?.Invoke();
    }



    public void Resume()
    {
        CurrentState = GameState.Started;
        Time.timeScale = 1f;
        GameResumed?.Invoke();
    }



    public void FinishGame()
    {
        CurrentState = GameState.Finished;
        GameOver?.Invoke();
    }



    public void Restart()
    {
        StartCoroutine(ReloadScene());
        GameStartedToRestart?.Invoke();
    }



    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(delayBeforeRestart);
        SceneManager.LoadScene("Main");
    }



    private void OnApplicationQuit()
    {
        SaveLoad.SaveAll();
    }

}

public enum GameState
{
    NotStarted,
    Started,
    Paused,
    Finished
}
