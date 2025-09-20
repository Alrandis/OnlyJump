using TMPro;
using UnityEngine;

public class ScoreEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _heightText;
    [SerializeField] private TextMeshProUGUI _timeText;

    public void SetData(Attempt attempt)
    {
        _scoreText.text = $"Счет: {attempt.Score.ToString()}";
        _heightText.text = $"Высота: {attempt.Height.ToString()}";
        _timeText.text = $"Время: {attempt.Time}";
    }
}
