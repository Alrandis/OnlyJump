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
        int score = Mathf.RoundToInt((_maxHeight * scoreMultiplier) / Mathf.Max(1, timeSpent));

        return (score, _maxHeight, timeSpent);
    }

    public void SaveAttempt()
    {
        var attempt = GetCurrentAttempt();
        gameData.AddAttempt(attempt.score, attempt.height, attempt.time);
    }
}
