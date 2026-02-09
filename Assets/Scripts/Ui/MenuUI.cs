using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using YG;
using System.Collections;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private Health _playerHealth;

    [SerializeField] private GameObject _panelMenu;
    [SerializeField] private GameObject _heatlhBar;
    [SerializeField] private GameObject _panelDeath;
    [SerializeField] private GameObject _panelCommon;
    [SerializeField] private GameObject _buttonMenu;
    [SerializeField] private TextMeshProUGUI _textHightValue;
    [SerializeField] private TextMeshProUGUI _textScoreValue;
    [SerializeField] private TextMeshProUGUI _textTimeValue;

    private bool _isMenuOpen = false;

    [SerializeField] private GameObject _btnRestart;
    [SerializeField] private GameObject _btnYes;
    [SerializeField] private GameObject _btnNo;
    [SerializeField] private GameObject _btnBack;
    [SerializeField] private GameObject _imgReward;

    private bool _waitingForReward;
    private bool _rewardReceived;

    private void Start()
    {
        _panelMenu.SetActive(false);
        _panelDeath.SetActive(false);
        _heatlhBar.SetActive(true);

        _btnRestart.SetActive(false);
        _btnBack.SetActive(false);
    }

    private void OnEnable()
    {
        Health.OnPlayerDown += OpenDeathPanel;
    }

    private void OnDisable()
    {
        Health.OnPlayerDown -= OpenDeathPanel;
    }

    private void OpenDeathPanel()
    {
        _buttonMenu.SetActive(false);
        _panelCommon.SetActive(true);
        _panelDeath.SetActive(true);
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        // стопаем время только при открытии меню
        Time.timeScale = _isMenuOpen ? 0f : 1f;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        var attempt = ScoreManager.Instance.GetCurrentAttempt();

        _textScoreValue.text = attempt.score.ToString();
        _textTimeValue.text = attempt.time.ToString();
        _textHightValue.text = attempt.height.ToString();
    }

    public void OnContinue()
    {
        _panelMenu.SetActive(false);
        _panelCommon.SetActive(false);
        ToggleMenu();
        _buttonMenu.SetActive(true);
        _heatlhBar.SetActive(true);
    }

    public void OpenMenu()
    {
        ToggleMenu();
        _buttonMenu.SetActive(false);
        _panelCommon.SetActive(true);
        _panelMenu.SetActive(true);
        _heatlhBar.SetActive(false);
    }

    public void OnRestart()
    {
        StartCoroutine(ShowAdsAndLoadScene(SceneManager.GetActiveScene().buildIndex));

        ScoreManager.Instance.SaveAttempt();
    }

    public void OnBackToMenu()
    {
        StartCoroutine(ShowAdsAndLoadScene("MainMenu"));

        ScoreManager.Instance.SaveAttempt();
    }

    public void ShowAdvReward()
    {
        _waitingForReward = true;
        _rewardReceived = false;

        string id = "retry"; // Передача id требуется для внутренней работы плагина
        YG2.RewardedAdvShow(id, OnRewardReceived);

        StartCoroutine(WaitRewardResultRoutine());
    }

    private void OnRewardReceived()
    {
        _rewardReceived = true;
    }

    private IEnumerator WaitRewardResultRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        // ждём, пока реклама закончится ЛЮБЫМ способом
        while (YG2.nowRewardAdv)
            yield return null;

        _waitingForReward = false;

        if (_rewardReceived)
        {
            _panelDeath.SetActive(false);
            _panelCommon.SetActive(false);
            ToggleMenu();
            _buttonMenu.SetActive(true);
            _heatlhBar.SetActive(true);

            ScoreManager.Instance.Reward();
            YG2.SkipNextInterAdCall();
        }
        else
        {
            OnNo();
        }
    }

    public void OnNo() 
    {
        _playerHealth.ConfirmDeath();

        _btnYes.SetActive(false);
        _imgReward.SetActive(false);
        _btnNo.SetActive(false);

        _btnRestart.SetActive(true);
        _btnBack.SetActive(true);
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
