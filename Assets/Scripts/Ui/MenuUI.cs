using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject buttonMenu;
    [SerializeField] private TextMeshProUGUI textScore;
    
    private bool isMenuOpen = false;

    private void Start()
    {
        panelMenu.SetActive(false);
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        panelMenu.SetActive(isMenuOpen);

        // стопаем время только при открытии меню
        Time.timeScale = isMenuOpen ? 0f : 1f;

        if (isMenuOpen)
        {
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        // пока плейсхолдер, потом подключим систему очков
        textScore.text = $"Высота: 0\n" +
                         $"Время: 0\n" +
                         $"Счёт: 0";
    }

    public void OnContinue()
    {
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
