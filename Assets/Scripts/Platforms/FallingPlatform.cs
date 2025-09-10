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
        _animator = GetComponent<Animator>();
        _currentDelay = _defaultDisappearDelay;
    }

    public void Activate(float delay)
    {
        _currentDelay = delay > 0 ? delay : _defaultDisappearDelay;
        ResetPlatform();
    }

    public override void ResetPlatform()
    {
        _isActivated = false;

        _animator.SetTrigger("Idle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isActivated) return;

        if (!collision.collider.CompareTag("Player")) return;   

        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y < -0.8f) // игрок сверху, давит вниз
            {
                _isActivated = true;
                StartCoroutine(DisappearRoutine());
                break;
            }
        }
    }

    private IEnumerator DisappearRoutine()
    {
        _animator.SetTrigger("Disappear");
        
        yield return new WaitForSeconds(_currentDelay);
        ReturnToPool();
    }
}
