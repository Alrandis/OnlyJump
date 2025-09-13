using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelDeath;
    [SerializeField] private GameObject buttonMenu;
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textDeathScore;

    private bool isMenuOpen = false;

    private void Start()
    {
        panelMenu.SetActive(false);
        panelDeath.SetActive(false);
    }

    private void OnEnable()
    {
        Health.PlayerDead += OpenDeathPanel;
    }

    private void OnDisable()
    {
        Health.PlayerDead -= OpenDeathPanel;
    }

    private void OpenDeathPanel()
    {
        buttonMenu.SetActive(false);
        panelDeath.SetActive(true);
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        // стопаем время только при открытии меню
        Time.timeScale = isMenuOpen ? 0f : 1f;

        UpdateScoreText();

    }

    private void UpdateScoreText()
    {
        // пока плейсхолдер, потом подключим систему очков
        textScore.text = $"Счёт: 0\n" +
                         $"Время: 0\n" +
                         $"Высота: 0";

        textDeathScore.text = $"Счёт: 0\n" +
                 $"Время: 0\n" +
                 $"Высота: 0";
    }

    public void OnContinue()
    {
        panelMenu.SetActive(false);
        ToggleMenu();
        buttonMenu.SetActive(true);
    }

    public void OpenMenu()
    {
        ToggleMenu();
        buttonMenu.SetActive(false);
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
