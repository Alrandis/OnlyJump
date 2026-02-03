using UnityEngine;

public class DontDestroyOnload : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }
}
