using UnityEngine;
using UnityEngine.UI;
using YG;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private LanguagesSO _languageSO;

    private void Start()
    {
        _soundSlider.value = YG2.saves.SoundVolume;
    }

    public void SetRussian()
    {
        YG2.saves.SelectedLanguage = Language.Russians;
        Save();
        YG2.saves.Changed();
    }

    public void SetEnglish()
    {
        YG2.saves.SelectedLanguage = Language.English;
        Save();
        YG2.saves.Changed();
    }

    public void SetBelorussian()
    {
        YG2.saves.SelectedLanguage = Language.Belorussian;
        Save();
        YG2.saves.Changed();
    }

    public void SetGermany()
    {
        YG2.saves.SelectedLanguage = Language.Germany;
        Save();
        YG2.saves.Changed();
    }

    public void SoundChange(float soundValue)
    {
        Debug.Log("Громкость до изменения " + YG2.saves.SoundVolume);
        YG2.saves.SoundVolume = soundValue;
        Debug.Log("Громкость после изменения " + YG2.saves.SoundVolume);
        Save();
    }

    public void Save()
    {
        YG2.SaveProgress();
    }

    public void BackToMenu()
    {
        gameObject.SetActive(false);
        _menuPanel.SetActive(true);
    }
}

public enum Language
{
    Russians,
    English,
    Belorussian,
    Germany
}
