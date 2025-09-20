using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _panelMenu;
    [SerializeField] private GameObject _heatlhBar;
    [SerializeField] private GameObject _panelDeath;
    [SerializeField] private GameObject _buttonMenu;
    [SerializeField] private TextMeshProUGUI _textScore;
    [SerializeField] private TextMeshProUGUI _textDeathScore;

    private bool _isMenuOpen = false;

    private void Start()
    {
        _panelMenu.SetActive(false);
        _panelDeath.SetActive(false);
        _heatlhBar.SetActive(true);
    }

    private void OnEnable()
    {
        Health.OnPlayerDead += OpenDeathPanel;
    }

    private void OnDisable()
    {
        Health.OnPlayerDead -= OpenDeathPanel;
    }

    private void OpenDeathPanel()
    {
        _buttonMenu.SetActive(false);
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

        _textScore.text = $"Счёт: {attempt.score}\n" +
                         $"Время: {attempt.time}\n" +
                         $"Высота: {attempt.height}";

        _textDeathScore.text = _textScore.text;
    }

    public void OnContinue()
    {
        _panelMenu.SetActive(false);
        ToggleMenu();
        _buttonMenu.SetActive(true);
        _heatlhBar.SetActive(true);
    }

    public void OpenMenu()
    {
        ToggleMenu();
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
        SceneManager.LoadScene("MainMenu"); // ⚠️ название твоей сцены меню
    }

    public void OnQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
