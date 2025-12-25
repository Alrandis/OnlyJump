using System;
using UnityEngine;

public class LevelCompleteHandler : MonoBehaviour
{
    public static LevelCompleteHandler Instance { get; private set; }

    public event Action LevelComplited;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelComplited?.Invoke();
            Debug.Log("Test touch");
        }
    }
}
