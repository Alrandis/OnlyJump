using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _scorePanel;

    [Header("Scene")]
    [SerializeField] private string _sceneName = "GameScene";

    [Header("Scriptable Objects")]
    [SerializeField] private GameDataSO _scoreDataSO;

    [Header("UI")]
    [SerializeField] private Button _continueButton; 

    private void Start()
    {
        // Отключаем кнопку Continue, если сохранения нет
        _continueButton.interactable = _scoreDataSO.IsExistSave;
    }

    /// <summary>
    /// Открыть окно счета
    /// </summary>
    public void OpenScore()
    {
        gameObject.SetActive(false);
        _scorePanel.SetActive(true);
    }

    public void ContinueGame()
    {
        //if (_scoreDataSO.IsExistSave)
        //{
        //    SceneManager.LoadScene(_sceneName);
        //}

        SceneManager.LoadScene(_sceneName);
    }

    public void StartNewGame()
    {
        _scoreDataSO.Clear();
        _scoreDataSO.IsExistSave = true;
        SceneManager.LoadScene(_sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
