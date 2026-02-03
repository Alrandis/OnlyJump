using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class ScoreEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _heightText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private List<StringList> _infoList = new List<StringList>();

    public void SetData(Attempt attempt)
    {
        _scoreText.text = $"{GetText(0)}: {attempt.Score.ToString()}";
        _heightText.text = $"{GetText(1)}: {attempt.Height.ToString()}";
        _timeText.text = $"{GetText(2)}: {attempt.Time}";
    }

    private string GetText(int id)
    {
        return YG2.saves.SelectedLanguage switch
        {
            "ru" => _infoList[id].list[0],
            "en" => _infoList[id].list[1],
            "be" => _infoList[id].list[2],
            "de" => _infoList[id].list[3],
            _ => "MISSING"
        };
    }


}
