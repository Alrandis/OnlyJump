using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void Show(float time)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(CountdownRoutine(time));
    }

    public void Hide()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private IEnumerator CountdownRoutine(float time)
    {
        float t = time;

        while (t > 0f)
        {
            _text.text = Mathf.CeilToInt(t).ToString();
            t -= Time.unscaledDeltaTime;
            yield return null;
        }

        _text.text = "";
    }
}
