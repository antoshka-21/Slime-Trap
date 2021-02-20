using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    [SerializeField] private ScoreHandler scoreHandler;
    [SerializeField] private Platform[] platforms;
    [SerializeField] private float speedOffset;

    private int platformSide = -1;

    private void OnEnable()
    {
        scoreHandler.CurrentScoreUpdated += SetSpeed;
    }

    private void OnDisable()
    {
        scoreHandler.CurrentScoreUpdated -= SetSpeed;
    }

    private void SetSpeed(int score)
    {
        float targetSpeed = Mathf.Log(score + 1, 2);

        if (targetSpeed > speedOffset + 1f)
        {
            targetSpeed += Random.Range(-speedOffset, 0f);
        }

        int id = (platformSide + 1) / 2;
        platforms[id].SetSpeed(targetSpeed);

        platformSide *= -1;
    }
}
