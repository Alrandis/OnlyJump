using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class LevelMenuUI : MonoBehaviour
{
    [SerializeField] private string _nextLevel;
    [SerializeField] private int _health = 3;
    [SerializeField] private GameObject _panelMenu;
    [SerializeField] private GameObject _heatlhBar;
    [SerializeField] private GameObject _panelLose;
    [SerializeField] private GameObject _panelWin;
    [SerializeField] private GameObject _buttonMenu;
    private float _startTime;
    private int _countRestart = 0;

    private void Start()
    {
        _startTime = Time.time;

        _panelLose.SetActive(false);
        _panelWin.SetActive(false);
        _heatlhBar.SetActive(true);

        LevelCompleteHandler.Instance.LevelComplited += OpenWinPanel;
    }

    private void OnEnable()
    {
        Health.OnPlayerDead += OpenDeathPanel;
        Health.OnHealthChanged += GetHealth;
    }
    public void GetHealth(int value)
    {
        _health = value;
    }

    private void OnDisable()
    {
        Health.OnPlayerDead -= OpenDeathPanel;
        LevelCompleteHandler.Instance.LevelComplited -= OpenWinPanel;
    }

    private void OpenDeathPanel()
    {
        _buttonMenu.SetActive(false);
        _heatlhBar.SetActive(false);
        _panelLose.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OpenWinPanel()
    {
        _buttonMenu.SetActive(false);
        _heatlhBar.SetActive(false);
        _panelWin.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnContinue()
    {
        Time.timeScale = 1f;
        _panelMenu.SetActive(false);
        _heatlhBar.SetActive(true);
    }

    public void OnNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_nextLevel);
    }

    public void OpenMenu()
    {
        Time.timeScale = 0f;
        _buttonMenu.SetActive(false);
        _panelMenu.SetActive(true);
        _heatlhBar.SetActive(false);
    }

    public void OnRestart()
    {
        int timeSpent = Mathf.FloorToInt(Time.time - _startTime);
        if(timeSpent <= 4)
        {
            _countRestart++;
            if(_countRestart >= 5)
            {
                YG2.saves.IsRestart = true;
            }
        }
        else
        {
            _countRestart = 0;
        }

        YG2.saves.Levels[SceneManager.GetActiveScene().buildIndex].TryCount++;
        YG2.SaveProgress();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnBackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // название твоей сцены меню
    }

}
