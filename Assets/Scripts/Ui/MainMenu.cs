using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _achivePanel;
    [SerializeField] private string _sceneName = "";

    [SerializeField] private List<Level> _levels = new List<Level>();

    private void Start()
    {
        YG2.saves.SelectedLanguage = YG2.lang;
        YG2.SaveProgress();
        YG2.saves.LangChanged();

        if (YG2.saves.Levels.Count == 0)
        {
            for (int i = 0; i < 30; i++)
            {
                _levels.Add(new Level(i));
            }
            _levels[0].IsOpen = true;

            for (int i = 0; i < 30; i++)
            {
                YG2.saves.Levels.Add(_levels[i]);
            }
        }
        else
        {
            for (int i = 0; i < 30; i++)
            {
                _levels.Add(YG2.saves.Levels[i]);
            }
        }
    }

    public void OpenAchive()
    {
        gameObject.SetActive(false);
        _achivePanel.SetActive(true);
    }

    public void OpenScore()
    {
        gameObject.SetActive(false);
        _scorePanel.SetActive(true);
    }

    public void OpenSetting()
    {
        gameObject.SetActive(false);
        _settingPanel.SetActive(true);
    }

    public void OpenEndlesMode() 
    {
        SceneManager.LoadScene("EternalLevel");
    }

    public void OpenStorysMode()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
