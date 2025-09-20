using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private GameDataSO _gameData;

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

        // Загружаем данные из JSON при старте игры
        SaveSystem.Load(_gameData);

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
        _gameData.AddAttempt(attempt.score, attempt.height, attempt.time);

        // Сохраняем JSON на диск
        SaveSystem.Save(_gameData);
    }
}
