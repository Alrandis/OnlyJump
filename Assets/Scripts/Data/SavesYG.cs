using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

namespace YG
{
    public partial class SavesYG
    {
        public float SoundVolume = 0.5f;
        public string SelectedLanguage = "";
        public List<Level> Levels = new List<Level>();
        public List<Attempt> Attempts = new List<Attempt>();

        public List<bool> Achives = Enumerable.Repeat(default(bool), 15).ToList();
  

        public int StarCount = 0;
        public int MaxHeight = 0;
        public int DeathCount = 0;
        public int DeathLava = 0;
        public int HighScore = 0;
        public int DamageCount = 0;
        public int CountRestart = 0;
        public int HighTime = 0;

        public bool IsSecret = false;
        public bool IsFast = false;
       
        public event Action LanguageChanged;
        public event Action SoundVolumChanged;

        public void LangChanged()
        {
            LanguageChanged?.Invoke();
        }

        public void SoundVolumeChanged()
        {
            SoundVolumChanged?.Invoke();
        }

        public void AddAttempt(int score, int height, int time)
        {
            bool exists = Attempts.Any(a =>
                a.Score == score &&
                a.Height == height &&
                a.Time == time);

            if (exists)
                return;

            var attempt = new Attempt(score, height, time);
            Attempts.Insert(0, attempt);

            HighScore = Mathf.Max(HighScore, score);
            HighTime = Mathf.Max(HighTime, time);
            MaxHeight = Mathf.Max(MaxHeight, height);

            YG2.SetLeaderboard("LeaderBord", HighScore);

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
    public int LevelID;
    public bool IsOpen;
    public int StarCount;
    public int TryCount;

    public Level(int id)
    {
        LevelID = id;
        IsOpen = false;
        StarCount = 0;
        TryCount = 0;
    }
}