using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace YG
{
    public partial class SavesYG
    {
        public float SoundVolume = 0f;
        public Language SelectedLanguage = Language.Russians;
        public int HighScore;
        public List<Level> Levels = new List<Level>();
        public List<Attempt> Attempts = new List<Attempt>();

        public event Action LanguageChanged;

        public void Changed()
        {
            LanguageChanged?.Invoke();
        }

        public void AddAttempt(int score, int height, int time)
        {
            var attempt = new Attempt(score, height, time);
            Attempts.Insert(0, attempt);

            HighScore = Mathf.Max(HighScore, score);

            if (Attempts.Count > 20)
            {
                Attempts.RemoveAt(Attempts.Count - 1);
            }
        }
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

[Serializable]
public class Level
{
    public int LevelID = 0;
    public bool IsOpen = false;
    public int StarCount = 0;
}