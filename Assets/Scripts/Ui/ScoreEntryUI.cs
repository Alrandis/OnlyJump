using TMPro;
using UnityEngine;

public class ScoreEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _heightText;
    [SerializeField] private TextMeshProUGUI _timeText;

    public void SetData(Attempt attempt)
    {
        _scoreText.text = $"����: {attempt.Score.ToString()}";
        _heightText.text = $"������: {attempt.Height.ToString()}";
        _timeText.text = $"�����: {attempt.Time}";
    }
}
