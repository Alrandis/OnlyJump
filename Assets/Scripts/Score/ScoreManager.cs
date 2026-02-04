using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using System.Linq; // Нужно для поиска в списке

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private float _startTime;
    private int _maxHeight;
    [SerializeField] private LevelGenerator _levelGenerator; // Перетащи генератор в инспекторе
    [SerializeField] private Health _playerHealth;         // Перетащи игрока (Health) в инспекторе
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _startTime = Time.time;
        _maxHeight = 0;

        Health.OnPlayerDeadConfirmed += SaveAttempt;
    }

    private void OnDestroy()
    {
        Health.OnPlayerDeadConfirmed -= SaveAttempt;
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
        if (SceneManager.GetActiveScene().name != "EternalLevel") return;
        Debug.Log("Сработал SaveAttempt");
        var attempt = GetCurrentAttempt();
        YG2.saves.AddAttempt(attempt.score, attempt.height, attempt.time);
        if(YG2.saves.MaxHeight < attempt.height)
            YG2.saves.MaxHeight = attempt.height;
        // Сохраняем
        YG2.SaveProgress();
        AchiveManager.Instance.HeightCheck();
        AchiveManager.Instance.ScoreCheck();
        AchiveManager.Instance.TimeCheck();
    }

    public void Reward()
    {
        if (_levelGenerator == null || _playerHealth == null) return;

        // Ищем платформу:
        // 1. Которая активна
        // 2. Которая НЕ является шипами (проверяем по имени префаба или тегу)
        // 3. Которая ближе всего к (MaxHeight + интервал)
        var targetPlatform = _levelGenerator.GetActivePlatforms()
            .Where(p => p != null && p.activeInHierarchy)
            // Исключаем платформы с шипами, чтобы не умереть сразу
            .Where(p => !p.GetComponentInChildren<SpikeDamage>() || !p.GetComponent<VerticalPlatform>())
            .OrderBy(p => Mathf.Abs(p.transform.position.y - (_maxHeight + _levelGenerator.PlatformSpacingY)))
            .FirstOrDefault();

        Vector3 spawnPosition;

        if (targetPlatform != null)
        {
            spawnPosition = targetPlatform.transform.position + Vector3.up * 1.5f;
        }
        else
        {
            // Резервный вариант, если подходящих платформ рядом нет
            spawnPosition = new Vector3(0, _maxHeight + 2f, 0);
        }

        _playerHealth.transform.position = spawnPosition;
        _playerHealth.RestoreHealth();
    }
}
