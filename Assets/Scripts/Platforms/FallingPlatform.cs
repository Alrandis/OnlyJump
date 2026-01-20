using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float _delayBeforeDisappear = 1f;
    [SerializeField] private float _fallSpeed = 5f;
    private Animator _animator;

    private bool _isTriggered = false;
    private float _disappearTimer = 0f;

    public bool MarkedForRemoval { get; private set; } = false;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void UpdateDisappearDelay(float playerY)
    {
        // при создании можно обновить таймер (если надо)
        _disappearTimer = _delayBeforeDisappear;
        _isTriggered = false;
        MarkedForRemoval = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isTriggered && collision.collider.CompareTag("Player"))
        {
            _isTriggered = true;
            _disappearTimer = _delayBeforeDisappear;
        }
    }

    private void Update()
    {
        if (_isTriggered && !MarkedForRemoval)
        {
            _animator.SetTrigger("Disappear");
            _disappearTimer -= Time.deltaTime;
            if (_disappearTimer <= 0f)
            {
                MarkedForRemoval = true;
                _isTriggered = false;
               
                GetComponent<Collider2D>().enabled = false;
                SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.enabled = false;
                }
                //GetComponentInChildren<SpriteRenderer>().enabled = false; 
            }
        }
    }

    public void ResetPlatform()
    {
        _isTriggered = false;
        MarkedForRemoval = false;
        _disappearTimer = 0f;
        // включаем обратно компоненты
        GetComponent<Collider2D>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        _animator.SetTrigger("Idle");
    }
}
