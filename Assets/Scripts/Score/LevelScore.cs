using Unity.VisualScripting;
using UnityEngine;
using YG;

public class LevelScore : MonoBehaviour
{
    [SerializeField] private int _levelId;
    private float _startTime;
    [SerializeField] private int _stars;
    [SerializeField] private float _fastTime;
    [SerializeField] private float _midleTime;
    [SerializeField] private int _health = 3;

    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startTime = Time.time;
        _star1.SetActive(false);
        _star2.SetActive(false);
        _star3.SetActive(false);

        LevelCompleteHandler.Instance.LevelComplited += SetStars;
    }

    private void OnEnable()
    {
        Health.OnHealthChanged += GetHealth;
    }

    
    private void OnDisable()
    {
        Health.OnHealthChanged -= GetHealth;
        LevelCompleteHandler.Instance.LevelComplited -= SetStars;
    }

    public void GetHealth(int value)
    {
        _health = value;
    }

    public int GetStars()
    {
        int timeSpent = Mathf.FloorToInt(Time.time - _startTime);
        if (_health == 3)
        {
            if (timeSpent < _fastTime) return 3;
            else if (timeSpent < _midleTime) return 2;
            else return 1;
        }
        else if (_health == 2) 
        {
            if (timeSpent < _fastTime) return 2;
            else return 1;
        } 
        else return 1;
    }

    public void SetStars()
    {
        _stars = GetStars();
        if (_stars == 1) 
        {
            _star1.SetActive(true);
        }
        else if(_stars == 2)
        {
            _star1.SetActive(true);
            _star2.SetActive(true);
        }
        else
        {
            _star1.SetActive(true);
            _star2.SetActive(true);
            _star3.SetActive(true);
        }

        YG2.saves.Levels[_levelId].StarCount = _stars;
        
        if (YG2.saves.Levels[_levelId + 1].IsOpen == false) 
            YG2.saves.Levels[_levelId + 1].IsOpen = true;

        YG2.SaveProgress();

    }
}
