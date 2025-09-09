using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float _defaultDisappearDelay = 2f;
    private float _currentDelay;

    private bool _isActivated;
    private Collider2D _collider;
    private SpriteRenderer _renderer;
    private Animator _animator;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _currentDelay = _defaultDisappearDelay;
    }

    public void Activate(float delay)
    {
        _currentDelay = delay > 0 ? delay : _defaultDisappearDelay;
        ResetPlatform();
    }

    private void ResetPlatform()
    {
        _isActivated = false;
        _collider.enabled = true;
        _renderer.enabled = true;
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

        _collider.enabled = false;
        _renderer.enabled = false;

        // Если используешь пул → верни платформу в пул
        gameObject.SetActive(false);
    }
}
