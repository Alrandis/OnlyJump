using UnityEngine;

public class LevelCompleteHandler : MonoBehaviour
{
    public static LevelCompleteHandler Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CompleteLevel(string levelName)
    {

    }
}
