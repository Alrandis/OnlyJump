using System.Collections;
using UnityEngine;
using YG;

public class InterstitialAdvManager : MonoBehaviour
{
    public static InterstitialAdvManager Instance;
    private float _currValue = 0;
    private void Awake()
    {
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

    public IEnumerator ShowAdsAndWait()
    {
        if (!YG2.isTimerAdvCompleted) yield break;

        AudioListener.pause = true;

        YG2.InterstitialAdvShow();

        yield return new WaitForSecondsRealtime(0.5f);

        // ∆дЄм, пока реклама не будет закрыта
        while (YG2.nowInterAdv) { yield return null; }

        AudioListener.pause = false;
    }
}
