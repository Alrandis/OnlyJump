using UnityEngine;
using YG;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private float _startTime;
    private int _maxHeight;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _startTime = Time.time;
        _maxHeight = 0;

        Health.OnPlayerDead += SaveAttempt;
    }

    private void OnDestroy()
    {
        Health.OnPlayerDead -= SaveAttempt;
    }

    public void RegisterPlatformHeight(int height)
    {
        if (height > _maxHeight)
            _maxHeight = height;
    }

    public (int score, int height, int time) GetCurrentAttempt()
    {
        int timeSpent = Mathf.FloorToInt(Time.time - _startTime);
        int baseScore = _maxHeight;

        float timePerHeight = 0.5f;
        float targetTime = _maxHeight * timePerHeight;

        int bonus = 0;
        if (timeSpent < targetTime)
            bonus = Mathf.RoundToInt((targetTime - timeSpent) * 2f);

        int finalScore = baseScore + bonus;
        return (finalScore, _maxHeight, timeSpent);
    }

    public void SaveAttempt()
    {
        var attempt = GetCurrentAttempt();
        YG2.saves.AddAttempt(attempt.score, attempt.height, attempt.time);

        // Сохраняем
        YG2.SaveProgress();
    }
}
