using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float _delayBeforeDisappear = 1f;
    [SerializeField] private float _respawnDelay = 2f;
    private Animator _animator;

    private bool _isTriggered = false;
    private float _disappearTimer = 0f;
    private float _respawnTimer = 0f;

    public bool MarkedForRemoval { get; private set; } = false;
    SpriteRenderer[] sprites;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
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
            _respawnTimer = _respawnDelay;
            _animator.SetTrigger("Disappear");
        }
    }

    private void Update()
    {
        if (_isTriggered && !MarkedForRemoval)
        {
            _disappearTimer -= Time.deltaTime;
            if (_disappearTimer <= 0f)
            {
                MarkedForRemoval = true;
                _isTriggered = false;
               
                GetComponent<Collider2D>().enabled = false;
               
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.enabled = false;
                }
            }
        }

        if (MarkedForRemoval && _disappearTimer <= 0f)
        {
            _respawnTimer -= Time.deltaTime;
            if (_respawnTimer <= 0f)
            {
                ResetPlatform();
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
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = true;
        }
        _animator.SetTrigger("Idle");
    }
}
