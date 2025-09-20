using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Scriptable Objects/GameDataSO")]
public class GameDataSO : ScriptableObject
{
    public const int c_MaxAttemptsStored = 20;

    public bool IsExistSave = false;

    [SerializeField] private int _highScore;
    public int HighScore => _highScore;

    public List<Attempt> Attempts = new List<Attempt>();

    public void AddAttempt(int score, int height, int time)
    {
        var attempt = new Attempt(score, height, time);
        Attempts.Insert(0, attempt);

        _highScore = Mathf.Max(_highScore, score);

        if (Attempts.Count > c_MaxAttemptsStored)
        {
            Attempts.RemoveAt(Attempts.Count - 1);
        }
    }

    public void Clear()
    {
        Attempts.Clear();
        _highScore = 0;
    }
}

[Serializable]
public class Attempt
{
    public int Score;
    public int Height;
    public int Time;

    public Attempt(int score, int height, int time)
    {
        Score = score;
        Height = height;
        Time = time;
    }
}
