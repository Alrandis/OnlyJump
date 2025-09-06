using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerJumpController : MonoBehaviour
{
    [Header("Настройки прыжка")]
    [SerializeField] private float _jumpTime = 0.4f;
    [SerializeField] private float _maxJumpX = 8f;
    [SerializeField] private float _maxJumpY = 4f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Скользкие поверхности")]
    [SerializeField] private LayerMask _slipperyLayer;
    [SerializeField] private float _slideDamping = 0.95f;

    [Header("Стены")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _wallSlideSpeed = 2f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private float _gravity;

    private bool _isGrounded;
    private bool _isJumping;
    private bool _isOnSlippery;
    private bool _isTouchingWall;
    private int _wallDirection;
    private Vector2 _jumpTarget;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _gravity = Mathf.Abs(Physics2D.gravity.y * _rb.gravityScale);
    }

    private void Update()
    {
        // Проверка земли
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundLayer);

        // Проверка скользкой поверхности
        _isOnSlippery = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _slipperyLayer);

        // Прыжок с поверхности (земля или стена)
        if (Input.GetMouseButtonDown(0) && (_isGrounded || _isTouchingWall))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 target = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            TryJump(target);
        }
    }

    private void FixedUpdate()
    {
        // Движение к цели прыжка
        if (_isJumping)
        {
            Vector2 currentPos = _rb.position;
            float distance = Vector2.Distance(currentPos, _jumpTarget);

            if (distance < 0.2f)
            {
                // эта строка важна, оставляем как есть
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x * 0.2f, _rb.linearVelocity.y * 0.01f);
                _rb.position = _jumpTarget;
                _isJumping = false;

                if (_animator != null)
                    _animator.SetTrigger("Idle");
            }
        }

        // Скользкие платформы и стены применяем только если игрок не прыгает
        if (!_isJumping)
        {
            if (_isOnSlippery)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x * _slideDamping, _rb.linearVelocity.y);
            }

            if (_isTouchingWall && !_isGrounded && _rb.linearVelocity.y < 0)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -_wallSlideSpeed);
            }
        }
    }

    private void TryJump(Vector2 target)
    {
        Vector2 startPos = _rb.position;
        Vector2 delta = target - startPos;

        delta.x = Mathf.Clamp(delta.x, -_maxJumpX, _maxJumpX);
        delta.y = Mathf.Clamp(delta.y, -_maxJumpY, _maxJumpY);

        float vx = delta.x / _jumpTime;
        float vy = delta.y / _jumpTime + 0.5f * _gravity * _jumpTime;

        _rb.linearVelocity = new Vector2(vx, vy);

        _jumpTarget = startPos + delta;
        _isJumping = true;

        if (_animator != null)
            _animator.SetTrigger("Jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isTouchingWall = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Проверка стен
            if (((1 << collision.gameObject.layer) & _wallLayer) != 0)
            {
                if (Mathf.Abs(contact.normal.x) > 0.9f)
                {
                    _isTouchingWall = true;
                    _wallDirection = contact.normal.x > 0 ? 1 : -1;
                    // останавливаем горизонтальное движение при контакте с обычной стеной
                    _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
                }
            }
        }

        if (_animator != null && _isGrounded)
            _animator.SetTrigger("Idle");
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundRadius);
        }
    }
}
