using UnityEngine;
using YG;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _volumSource;

    private void Awake()
    {
        _volumSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ValueChange();
        YG2.saves.SoundVolumChanged += ValueChange;

    }

    private void OnDisable()
    {
        YG2.saves.SoundVolumChanged -= ValueChange;

    }

    private void ValueChange()
    {
        _volumSource.volume = YG2.saves.SoundVolume;
    }
}
