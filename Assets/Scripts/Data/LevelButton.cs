using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int _levelId;
    [SerializeField] private string _levelName = "Level";
    [SerializeField] private List<GameObject> _stars = new List<GameObject>();
    [SerializeField] private Button _levButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _levButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _levButton.interactable = YG2.saves.Levels[_levelId].IsOpen ? true : false;
        SetStars();
    }

    public void SetStars()
    {

        if (YG2.saves.Levels[_levelId].StarCount == 1)
        {
            _stars[0].SetActive(true);
        }
        else if (YG2.saves.Levels[_levelId].StarCount == 2)
        {
            _stars[0].SetActive(true);
            _stars[1].SetActive(true);
        }
        else
        {
            _stars[0].SetActive(true);
            _stars[1].SetActive(true);
            _stars[2].SetActive(true);
        }
    }

    public void OpenLevel()
    {
        SceneManager.LoadScene(_levelName);
    }
}
