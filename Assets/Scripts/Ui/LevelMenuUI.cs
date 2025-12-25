using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenuUI : MonoBehaviour
{
    [SerializeField] private string _nextLevel;

    [SerializeField] private GameObject _panelMenu;
    [SerializeField] private GameObject _heatlhBar;
    [SerializeField] private GameObject _panelLose;
    [SerializeField] private GameObject _panelWin;
    [SerializeField] private GameObject _buttonMenu;

    private void Start()
    {
        _panelLose.SetActive(false);
        _panelWin.SetActive(false);
        _heatlhBar.SetActive(true);

        LevelCompleteHandler.Instance.LevelComplited += OpenWinPanel;
    }

    private void OnEnable()
    {
        Health.OnPlayerDead += OpenDeathPanel;
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnBackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // название твоей сцены меню
    }

}
