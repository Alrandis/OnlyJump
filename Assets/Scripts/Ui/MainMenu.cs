using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private string _sceneName = "GameScene";
    [SerializeField] private GameDataSO _scoreDataSO;
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

        // Удаляем файл сохранения JSON, если он есть
        string path = System.IO.Path.Combine(Application.persistentDataPath, "gamedata.json");
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);

        SceneManager.LoadScene(_sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
