using System.Collections;
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
        Health.OnPlayerDown += OpenDeathPanel;
        Health.OnHealthChanged += GetHealth;
    }
    public void GetHealth(int value)
    {
        _health = value;
    }

    private void OnDisable()
    {
        Health.OnPlayerDown -= OpenDeathPanel;
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
        StartCoroutine(ShowAdsAndLoadScene(_nextLevel));
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
            YG2.saves.CountRestart++;
            YG2.SaveProgress();
        }


        YG2.saves.Levels[SceneManager.GetActiveScene().buildIndex - 1].TryCount++;
        YG2.SaveProgress();

        StartCoroutine(ShowAdsAndLoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void OnBackToMenu()
    {
        StartCoroutine(ShowAdsAndLoadScene("MainMenu"));
    }

    IEnumerator ShowAdsAndLoadScene(string scene)
    {
        yield return StartCoroutine(
            InterstitialAdvManager.Instance.ShowAdsAndWait()
        );

        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
    IEnumerator ShowAdsAndLoadScene(int scene)
    {
        yield return StartCoroutine(
            InterstitialAdvManager.Instance.ShowAdsAndWait()
        );

        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }

}
