using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _achivePanel;
    [SerializeField] private string _sceneName = "";

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
