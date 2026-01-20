using TMPro;
using UnityEngine;
using YG;

public class LocalizedText : MonoBehaviour
{
    [TextArea][SerializeField] private string _russianText;
    [TextArea][SerializeField] private string _englishText;
    [TextArea][SerializeField] private string _belorussianText;
    [TextArea][SerializeField] private string _germanyText;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        UpdateText();
        YG2.saves.LanguageChanged += UpdateText;
    }

    private void OnDisable()
    {
        YG2.saves.LanguageChanged -= UpdateText;
    }

    private void UpdateText()
    {
        _text.text = GetLine(YG2.saves.SelectedLanguage);
    }

    private string GetLine(Language language)
    {
        return language switch
        {
            Language.Russians => _russianText,
            Language.English =>_englishText,
            Language.Belorussian => _belorussianText,
            Language.Germany => _germanyText,
            _ => "MISSING"
        };
    }
}
