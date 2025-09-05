using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    //[SerializeField] private Button _continueButton;
    //[SerializeField] private Button _newGameButton;
    //[SerializeField] private Button _scoreButton;
    //[SerializeField] private Button _quitGameButton;

    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private string _sceneName = "GameScene";

    private GameObject _menuPanel;

    private void Awake()
    {
        _menuPanel = GetComponent<GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OpenScore()
    {
        _menuPanel.SetActive(false);
        _scorePanel.SetActive(true);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(_sceneName);
        // ƒобавить обработку уничтожени€ прогресса
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
