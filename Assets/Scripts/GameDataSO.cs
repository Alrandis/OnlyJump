using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Scriptable Objects/GameDataSO")]
public class GameDataSO : ScriptableObject
{
    [SerializeField] private int _highScore; // Не забыть убрать SerializeField в конце разработки

    public List<Attempt> Attempts;

    public void AddAttempt(Attempt attempt)
    {
        Attempts.Insert(0, attempt);

        _highScore = (_highScore < attempt.Score) ? attempt.Score : _highScore;

        if (Attempts.Count >= Attempt.c_MaxAttemptsStored)
        {
            Attempts.RemoveAt(Attempt.c_MaxAttemptsStored);
        }
    }

    public string GetHighScore()
    {
        return _highScore.ToString();
    }

    public void Cliar()
    {
        Attempts.Clear();
        _highScore = 0;
    }

}

public struct Attempt
{
    public int Score;
    public int Height;
    public string Timestamp;
    public const int c_MaxAttemptsStored = 10;
}