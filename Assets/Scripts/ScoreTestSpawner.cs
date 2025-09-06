using UnityEngine;

public class ScoreTestSpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameDataSO _gameDataSO;

    [Header("UI")]
    [SerializeField] private ScorePanelUI _scorePanelUI;

    [Header("Test Settings")]
    [SerializeField] private int _testEntriesCount = 20;

    private void Start()
    {
        GenerateTestScores();
        // Обновляем UI
        _scorePanelUI.UpdateScoreList();
    }

    private void GenerateTestScores()
    {
        if (_gameDataSO == null)
        {
            Debug.LogError("GameDataSO не назначен!");
            return;
        }

        // Чистим старые данные
        _gameDataSO.Clear();

        // Создаём тестовые записи
        for (int i = 0; i < _testEntriesCount; i++)
        {
            int score = Random.Range(100, 1000);
            int height = Random.Range(10, 100);
            int time = Random.Range(30, 300); // секунды

            _gameDataSO.AddAttempt(score, height, time);
        }
    }
}
