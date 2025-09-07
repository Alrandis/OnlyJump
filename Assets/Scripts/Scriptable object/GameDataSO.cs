using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Scriptable Objects/GameDataSO")]
public class GameDataSO : ScriptableObject
{
    public const int c_MaxAttemptsStored = 20;

    [SerializeField] private int _highScore; // Не забыть убрать SerializeField в конце разработки
    public bool IsExistSave { get; set; } = false;

    public List<Attempt> Attempts = new List<Attempt>();

    public void AddAttempt(int score, int height, int time)
    {
        var attempt = new Attempt(score, height, time);
        Attempts.Insert(0, attempt);

        _highScore = (_highScore < attempt.Score) ? attempt.Score : _highScore;

        if (Attempts.Count >= c_MaxAttemptsStored)
        {
            Attempts.RemoveAt(Attempts.Count - 1);
        }
    }

    public string GetHighScore()
    {
        return _highScore.ToString();
    }

    public void Clear()
    {
        Attempts.Clear();
        _highScore = 0;
    }

}

public class Attempt
{
    public int Score { get; private set; }
    public int Height { get; private set; }
    public string Timestamp { get; private set; }

    public Attempt(int score, int height, int time)
    {
        Score = score;
        Height = height;
        Timestamp = time.ToString();
    }
}