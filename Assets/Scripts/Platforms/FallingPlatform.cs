using UnityEngine;
using System.Collections;

public class FallingPlatform : PlatformBase
{
    [SerializeField] private float _defaultDisappearDelay = 2f;
    private float _currentDelay;

    private bool _isActivated;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _currentDelay = _defaultDisappearDelay;
    }

    public override void ResetPlatform()
    {
        _isActivated = false;

        if (_animator != null)
            _animator.SetTrigger("Idle");

        _currentDelay = _defaultDisappearDelay;
    }

    // Автоматическая установка задержки по высоте игрока
    public void UpdateDisappearDelay(float playerHeight)
    {
        float maxDelay = 2f;
        float minDelay = 0.5f;
        float maxHeight = 100f; // высота, на которой сложность максимальна

        float t = Mathf.Clamp01(playerHeight / maxHeight);
        _currentDelay = Mathf.Lerp(maxDelay, minDelay, t);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isActivated) return;
        if (!collision.collider.CompareTag("Player")) return;

        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y < -0.8f) // игрок сверху
            {
                _isActivated = true;
                StartCoroutine(DisappearRoutine());
                break;
            }
        }
    }

    private IEnumerator DisappearRoutine()
    {
        if (_animator != null)
            _animator.SetTrigger("Disappear");

        yield return new WaitForSeconds(_currentDelay);

        ReturnToPool();
    }

    public new void Activate()
    {
        base.Activate();
        ResetPlatform();
    }
}
