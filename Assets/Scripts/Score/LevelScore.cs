using UnityEngine;

public class LevelScore : MonoBehaviour
{
    private float _startTime;
    private float _fastTime;
    private float _midleTime;
    private int _health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startTime = Time.time;
    }

    private void OnEnable()
    {
        Health.OnHealthChanged += GetHealth;
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

}
