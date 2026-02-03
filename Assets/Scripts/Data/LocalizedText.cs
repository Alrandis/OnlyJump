using TMPro;
using UnityEngine;
using YG;

public class LocalizedText : MonoBehaviour
{
    [TextArea][SerializeField] private string _russianText;
    [TextArea][SerializeField] private string _englishText;
    [TextArea][SerializeField] private string _belorussianText;
    [TextArea][SerializeField] private string _germanyText;

    public TMP_Text Text;

    private void Awake()
    {
        Text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        YG2.saves.LanguageChanged += UpdateText;
        UpdateText();
    }

    private void OnDisable()
    {
        YG2.saves.LanguageChanged -= UpdateText;
    }

    private void UpdateText()
    {
        Text.text = GetLine(YG2.saves.SelectedLanguage);
    }

    private string GetLine(string language)
    {
        return language switch
        {
            "ru" => _russianText,
            "en" => _englishText,
            "be" => _belorussianText,
            "de" => _germanyText,
            _ => "MISSING"
        };
    }
}
