using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanelUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _menuPanel;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private ScrollRect _scrollView;

    [Header("Prefab")]
    [SerializeField] private GameObject _scoreEntryPrefab;

    [Header("Scriptable Objects")]
    [SerializeField] private GameDataSO _scoreDataSO;

    // Object Pool
    private Queue<GameObject> _entryPool = new Queue<GameObject>();

    private void OnEnable()
    {
        // Обновляем лучший счет
        _bestScoreText.text = $"Лучший счет: {_scoreDataSO.GetHighScore()}";

        // Заполняем ScrollView
        UpdateScoreList();
    }

    /// <summary>
    /// Заполняет ScrollView элементами попыток
    /// </summary>
    public void UpdateScoreList()
    {
        ClearScrollView(); // очищаем предыдущие записи

        foreach (var attempt in _scoreDataSO.Attempts)
        {
            GameObject entry = GetPooledEntry();
            entry.transform.SetParent(_scrollView.content, false);

            // Заполняем данные в entry
            var entryUI = entry.GetComponent<ScoreEntryUI>();
            if (entryUI != null)
            {
                entryUI.SetData(attempt);
            }

            entry.SetActive(true);
        }
    }

    /// <summary>
    /// Берем объект из пула или создаем новый
    /// </summary>
    private GameObject GetPooledEntry()
    {
        if (_entryPool.Count > 0)
        {
            return _entryPool.Dequeue();
        }
        else
        {
            return Instantiate(_scoreEntryPrefab);
        }
    }

    /// <summary>
    /// Очищаем ScrollView и возвращаем все объекты в пул
    /// </summary>
    private void ClearScrollView()
    {
        foreach (Transform child in _scrollView.content)
        {
            child.gameObject.SetActive(false);
            _entryPool.Enqueue(child.gameObject);
        }
    }

    /// <summary>
    /// Возврат в меню
    /// </summary>
    public void BackToMenu()
    {
        gameObject.SetActive(false);
        _menuPanel.SetActive(true);
    }
}
