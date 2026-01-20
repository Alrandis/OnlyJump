
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Finish : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private string _nextLevel = "Level";

    private bool _isTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTriggered)
            return;

        if (!other.CompareTag(_playerTag))
            return;

        _isTriggered = true;
    }
}
