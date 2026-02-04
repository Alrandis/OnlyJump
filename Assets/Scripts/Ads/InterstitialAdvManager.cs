using System.Collections;
using UnityEngine;
using YG;

public class InterstitialAdvManager : MonoBehaviour
{
    public static InterstitialAdvManager Instance;
    private bool _isAdClosed;
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

    private void OnEnable()
    {
        YG2.onCloseInterAdv += OnAdClosed;
    }

    private void OnDisable()
    {
        YG2.onCloseInterAdv -= OnAdClosed;
    }

    private void OnAdClosed()
    {
        _isAdClosed = true;
    }

    public IEnumerator ShowAdsAndWait()
    {
        if (!YG2.isTimerAdvCompleted) yield break;

        _isAdClosed = false;

        _currValue = YG2.saves.SoundVolume;
        YG2.saves.SoundVolume = 0;
        YG2.saves.SoundVolumeChanged();

        YG2.InterstitialAdvShow();

        // ∆дЄм, пока реклама не будет закрыта
        yield return new WaitUntil(() => _isAdClosed);

        YG2.saves.SoundVolume = _currValue;
        YG2.saves.SoundVolumeChanged();
    }
}
