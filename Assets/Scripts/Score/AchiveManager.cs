using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using YG;

public class AchiveManager : MonoBehaviour
{
    public static AchiveManager Instance;

    [SerializeField] private GameObject _achiveUI;
    [SerializeField] private List<bool> Achives = new List<bool>();
    [SerializeField] private List<StringList> _nameList = new List<StringList>();

    [SerializeField] private TextMeshProUGUI _nameInfo;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        YG2.saves.SelectedLanguage = YG2.lang;
        YG2.SaveProgress();
        YG2.saves.LangChanged();

        Achives = YG2.saves.Achives;
        if (YG2.saves.Levels == null) return;
        DeathCheck();
        StarCheck();
        HeightCheck();
        ScoreCheck();
        TimeCheck();
        TryCheck();
        DeathLavaCheck();
        SecretCheck();
        SpeedCheck();
        RestartCheck();
        BossCheck();
        PainCheck();
    }

    public void ShowAchive(int id)
    {
        StartCoroutine(WaiteForShow(id));
    }

    IEnumerator WaiteForShow(int id)  
    {
        _achiveUI.SetActive(true);
        switch (YG2.saves.SelectedLanguage)
        {
            case "ru":
                _nameInfo.text = _nameList[id].list[0];
                break;
            case "en":
                _nameInfo.text = _nameList[id].list[1];
                break;
            case "be":
                _nameInfo.text = _nameList[id].list[2];
                break;
            case "de":
                _nameInfo.text = _nameList[id].list[3];
                break;
        }
        yield return new WaitForSecondsRealtime(3);
        _achiveUI.SetActive(false);
    }

    public void BossCheck()
    {
        if (YG2.saves.Achives[12] == true) return;
        int counter = 0;
        foreach(var achive in YG2.saves.Achives)
        {
            if (achive == true)
            {
                counter++;
            }
        }

        if (counter == 14)
        {
            YG2.saves.Achives[12] = true;
            ShowAchive(12);
        }
        else
        {
            YG2.saves.Achives[12] = false;
        }
    }

    public void RestartCheck() 
    {
        if (YG2.saves.Achives[14] == true) return;

        if (YG2.saves.CountRestart >= 10)
        {
            YG2.saves.Achives[14] = true;
            ShowAchive(14);
        }
        else
        {
            YG2.saves.Achives[14] = false;
        }
    }

    public void PainCheck()
    {
        if (YG2.saves.Achives[13] == true) return;

        if (YG2.saves.DamageCount >= 10)
        {
            YG2.saves.Achives[13] = true;
            ShowAchive(13);
        }
        else
        {
            YG2.saves.Achives[13] = false;
        }
    }

    public void SecretCheck()
    {
        if (YG2.saves.Achives[9] == true) return;

        if (YG2.saves.IsSecret == true)
        {
            YG2.saves.Achives[9] = true;
            ShowAchive(9);
        }
        else
        {
            YG2.saves.Achives[9] = false;
        }
    }

    public void HeightCheck()
    {
        if (YG2.saves.Achives[4] == true) return;

        if (YG2.saves.MaxHeight >= 800)
        {
            YG2.saves.Achives[4] = true;
            ShowAchive(4);
        }
        else
        {
            YG2.saves.Achives[4] = false;
        }
    }

    public void TimeCheck()
    {
        if (YG2.saves.Achives[5] == true) return;

        if (YG2.saves.HighTime >= 240)
        {
            YG2.saves.Achives[5] = true;
            ShowAchive(5);
        }
        else
        {
            YG2.saves.Achives[5] = false;
        }
    }

    public void SpeedCheck()
    {
        if (YG2.saves.Achives[10] == true) return;

        if (YG2.saves.IsFast == true)
        {
            YG2.saves.Achives[10] = true;
            ShowAchive(10);
        }
        else
        {
            YG2.saves.Achives[10] = false;
        }
    }

    public void ScoreCheck()
    {
        if (YG2.saves.Achives[6] == true) return;

        if (YG2.saves.HighScore >= 1000)
        {
            YG2.saves.Achives[6] = true;
            ShowAchive(6);
        }
        else
        {
            YG2.saves.Achives[6] = false;
        }
    }

    public void TryCheck()
    {
        if (YG2.saves.Achives[3] == true) return;

        foreach(var level in YG2.saves.Levels)
        {
            if(level.TryCount >= 10)
            {
                YG2.saves.Achives[3] = true;
                ShowAchive(3);
                break;
            }
            else
            {
                YG2.saves.Achives[3] = false;
            }
        }
    }

    public void DeathCheck()
    {
        if (YG2.saves.Achives[8] == true
            && YG2.saves.Achives[11] == true) return;

        if (YG2.saves.DeathCount == 30 && YG2.saves.Achives[8] == false)
        {
            ShowAchive(8);
            YG2.saves.Achives[8] = true;
        }
 

        if (YG2.saves.DeathCount == 60)
        {
            YG2.saves.Achives[11] = true;
            ShowAchive(11);
        }
    }

    public void DeathLavaCheck()
    {
        if (YG2.saves.Achives[7] == true) return;

        if (YG2.saves.DeathLava >= 15)
        {
            YG2.saves.Achives[7] = true;
            ShowAchive(7);
        }
        else
        {
            YG2.saves.Achives[7] = false;
        }
    }

    public void StarCheck()
    {
        if (YG2.saves.Achives[0] == true 
            && YG2.saves.Achives[1] == true 
            && YG2.saves.Achives[2] == true) return;

        int starCount = 0;
        foreach (var level in YG2.saves.Levels)
        {
            starCount += level.StarCount;
        }
       
        if (starCount >= 30 && YG2.saves.Achives[0] == false)
        {
            YG2.saves.Achives[0] = true;
            ShowAchive(0);
        }
        if(starCount >= 60 && YG2.saves.Achives[1] == false)
        {
            YG2.saves.Achives[1] = true;
            ShowAchive(1);
        }
        if (starCount == 90 && YG2.saves.Achives[2] == false)
        {
            YG2.saves.Achives[2] = true;
            ShowAchive(2);
        }

        YG2.SaveProgress();
    }
}
