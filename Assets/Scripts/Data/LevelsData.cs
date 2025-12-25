using System.Collections.Generic;
using UnityEngine;
using YG;

public class LevelsData : MonoBehaviour
{
    [SerializeField] private List<Level> _levels = new List<Level> ();

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
}
