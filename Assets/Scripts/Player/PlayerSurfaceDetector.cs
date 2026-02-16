using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerSurfaceDetector : MonoBehaviour
{
    [SerializeField] private PlayerDataSO _playerData;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Wall Check")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _wallCheckDistance = 0.1f;

    private Rigidbody2D _rb;
    private Collider2D _collider;
    private PlayerAirControl _playerAirControl;
    private Animator _animator;

    private MovingPlatform _currentMovingPlatform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAirControl = GetComponent<PlayerAirControl>();
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        CheckGround();
        CheckWall();
        ApplyPlatformVelocity(); // добавили аккуратно в конец
    }

    private void CheckGround()
    {
        Collider2D groundCollider = Physics2D.OverlapCircle(
            _groundCheck.position,
            _groundRadius,
            _groundLayer
        );

        _playerData.IsGrounded = groundCollider != null;

        if (_playerData.IsGrounded)
        {
            _playerAirControl.ResetAirControl();

            if (_animator != null)
                _animator.SetTrigger("Idle");

            // Проверяем, стоим ли на движущейся платформе
            _currentMovingPlatform = groundCollider.GetComponent<MovingPlatform>();
        }
        else
        {
            _currentMovingPlatform = null;
        }
    }


    private void ApplyPlatformVelocity()
    {
        if (_currentMovingPlatform == null)
            return;

        if (!_playerData.IsGrounded)
            return;

        // Не передаём скорость если игрок прыгает вверх
        if (_playerData.IsJumping && _rb.linearVelocity.y > 0f)
            return;

        _rb.position += _currentMovingPlatform.CurrentVelocity * Time.fixedDeltaTime;
    }


    private void CheckWall()
    {
        if (_playerData.IsKnockedBack)
        {
            _playerData.IsTouchingWall = false;
            return;
        }

        Bounds bounds = _collider.bounds;

        Vector2 leftOrigin = new Vector2(bounds.min.x, bounds.center.y);
        Vector2 rightOrigin = new Vector2(bounds.max.x, bounds.center.y);

        RaycastHit2D hitLeft = Physics2D.Raycast(
            leftOrigin,
            Vector2.left,
            _wallCheckDistance,
            _wallLayer
        );

        RaycastHit2D hitRight = Physics2D.Raycast(
            rightOrigin,
            Vector2.right,
            _wallCheckDistance,
            _wallLayer
        );

        bool touchingWall = hitLeft.collider != null || hitRight.collider != null;

        _playerData.IsTouchingWall = touchingWall;

        if (touchingWall)
        {
            _playerAirControl.ResetAirControl();

            // обнуляем X только если игрок НЕ начал прыжок
            if (!_playerData.IsJumping)
            {
                _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
            }
        }
    }
}
  
