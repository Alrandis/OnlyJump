using TMPro;
using UnityEngine;

public class ScorePanelUI : MonoBehaviour
{
    private GameObject _scorePanel;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private GameObject _menuPanel;


    private void Awake()
    {
        _scorePanel = GetComponent<GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        // Обновлять информацию при срабатывании скрипта о счете игрока 
    }

    public void BackToMenu()
    {
        _scorePanel.SetActive(false);
        _menuPanel.SetActive(true);    
    }
}
