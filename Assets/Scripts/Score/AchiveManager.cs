using UnityEngine;
using YG;

public class AchiveManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
        else
        {
            YG2.saves.Achives[12] = false;
        }
    }

    public void RestartCheck() 
    {
        if (YG2.saves.Achives[14] == true) return;

        if (YG2.saves.IsRestart == true)
        {
            YG2.saves.Achives[14] = true;
        }
        else
        {
            YG2.saves.Achives[14] = false;
        }
    }

    public void PainCheck()
    {
        if (YG2.saves.Achives[13] == true) return;

        if (YG2.saves.DamageCount == 10)
        {
            YG2.saves.Achives[13] = true;
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
        }
        else
        {
            YG2.saves.Achives[9] = false;
        }
    }

    public void HeightCheck()
    {
        if (YG2.saves.Achives[4] == true) return;

        if (YG2.saves.MaxHeight >= 300)
        {
            YG2.saves.Achives[4] = true;
        }
        else
        {
            YG2.saves.Achives[4] = false;
        }
    }

    public void TimeCheck()
    {
        if (YG2.saves.Achives[5] == true) return;

        if (YG2.saves.HighTime >= 100)
        {
            YG2.saves.Achives[5] = true;
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
        }
        else
        {
            YG2.saves.Achives[10] = false;
        }
    }

    public void ScoreCheck()
    {
        if (YG2.saves.Achives[6] == true) return;

        if (YG2.saves.HighScore >= 200)
        {
            YG2.saves.Achives[6] = true;
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

        if (YG2.saves.DeathCount >= 30)
        {
            YG2.saves.Achives[8] = true;
        }
        else
        {
            YG2.saves.Achives[8] = false;
        }

        if (YG2.saves.DeathCount >= 60)
        {
            YG2.saves.Achives[11] = true;
        }
        else
        {
            YG2.saves.Achives[11] = false;
        }
    }

    public void DeathLavaCheck()
    {
        if (YG2.saves.Achives[7] == true) return;

        if (YG2.saves.DeathLava >= 10)
        {
            YG2.saves.Achives[7] = true;
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
       
        if (starCount == 30)
        {
            YG2.saves.Achives[0] = true;
        }
        else if(starCount == 60)
        {
            YG2.saves.Achives[1] = true;
        }
        else if (starCount == 90)
        {
            YG2.saves.Achives[2] = true;
        }

        YG2.SaveProgress();
    }
}
