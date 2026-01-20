using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LevelsData : MonoBehaviour
{
    [SerializeField] private List<Level> _levels = new List<Level> ();
    [SerializeField] private List<GameObject> _levelsPanel = new List<GameObject>();

    private int index = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (YG2.saves.Levels.Count == 0)
        {
            for (int i = 0; i < 30; i++)
            {
                _levels.Add(new Level(i));
            }
            _levels[0].IsOpen = true;

            for (int i = 0; i < 30; i++)
            {
                YG2.saves.Levels.Add(_levels[i]);
            }
        }
        else
        {
            for (int i = 0; i < 30; i++)
            {
                _levels.Add(YG2.saves.Levels[i]);
            }
        }

    }

    public void SwichLeft()
    {
        _levelsPanel[index].SetActive(false);
        index--;
        if(index < 0)
            index = 5;
        _levelsPanel[index].SetActive(true);
    }

    public void SwichRight()
    {
        _levelsPanel[index].SetActive(false);
        index++;
        if(index > 5)
            index = 0;
        _levelsPanel[index].SetActive(true);
    }
}
