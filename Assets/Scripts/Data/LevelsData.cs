using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LevelsData : MonoBehaviour
{
 
    [SerializeField] private List<GameObject> _levelsPanel = new List<GameObject>();

    private int index = 0;


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
