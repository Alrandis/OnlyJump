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
        // ��������� UI
        _scorePanelUI.UpdateScoreList();
    }

    private void GenerateTestScores()
    {
        if (_gameDataSO == null)
        {
            Debug.LogError("GameDataSO �� ��������!");
            return;
        }

        // ������ ������ ������
        _gameDataSO.Clear();

        // ������ �������� ������
        for (int i = 0; i < _testEntriesCount; i++)
        {
            int score = Random.Range(100, 1000);
            int height = Random.Range(10, 100);
            int time = Random.Range(30, 300); // �������

            _gameDataSO.AddAttempt(score, height, time);
        }
    }
}
