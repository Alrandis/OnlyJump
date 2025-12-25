using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformTracker : MonoBehaviour
{
    private bool _touched = false;
    private string _curScene;

    private void Awake()
    {
        _curScene = SceneManager.GetActiveScene().name;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_touched || _curScene != "EternalLevel") return;

        if (collision.gameObject.CompareTag("Player"))
        {
            int height = Mathf.FloorToInt(transform.position.y);
            ScoreManager.Instance.RegisterPlatformHeight(height);
            _touched = true; // чтобы не считать повторно
        }
    }
}
