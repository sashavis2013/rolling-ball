using System;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    static int _score;

    public static event Action<int> OnScoreUpdated;

    private void OnEnable()
    {
        MoveCommand.OnTileFilled += AddScore;
    }

    private void OnDisable()
    {
        MoveCommand.OnTileFilled -= AddScore;
    }

    private void Start()
    {
        OnScoreUpdated?.Invoke(_score);
    }

    public int GetCurrentScore()
    {
        return _score;
    }

    public void ResetScore()
    {
        _score = 0;
    }

    public int GetnSetBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("HighScore", 0);

        if (_score > bestScore)
        {
            PlayerPrefs.SetInt("HighScore", _score);
            bestScore = _score;
            Debug.Log($"Set new highscore of {bestScore}");
        }

        return bestScore;
    }

    public void AddScore(int score)
    {
        _score += score;
        OnScoreUpdated?.Invoke(_score);
    }
}