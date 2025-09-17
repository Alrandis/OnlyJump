using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private GameDataSO gameData;
    [SerializeField] private float scoreMultiplier = 10f;

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

        // --- Базовый счёт (за высоту) ---
        int baseScore = _maxHeight;

        // --- Бонус за скорость ---
        float timePerHeight = 0.5f; // "эталонное" время на 1 единицу высоты
        float targetTime = _maxHeight * timePerHeight;

        int bonus = 0;
        if (timeSpent < targetTime)
        {
            bonus = Mathf.RoundToInt((targetTime - timeSpent) * 2f);
            // 2f — коэффициент, подбираешь под баланс
        }

        int finalScore = baseScore + bonus;

        return (finalScore, _maxHeight, timeSpent);
    }


    public void SaveAttempt()
    {
        var attempt = GetCurrentAttempt();
        gameData.AddAttempt(attempt.score, attempt.height, attempt.time);
    }
}
