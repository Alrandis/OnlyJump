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

    private Rigidbody2D _rb;
    private PlayerAirControl _playerAirControl;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAirControl = GetComponent<PlayerAirControl>();
        _animator = GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        // ѕроверка земли
        _playerData.IsGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundLayer);
        if (_playerData.IsGrounded)
        {
            _playerAirControl.ResetAirControl();
            if (_animator != null)
                _animator.SetTrigger("Idle");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DetectWall(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // если игрок оторвалс€ от стены
        if (((1 << collision.gameObject.layer) & _wallLayer) != 0)
        {
            _playerData.IsTouchingWall = false;
        }
    }

    private void DetectWall(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & _wallLayer) != 0)
        {
            _playerAirControl.ResetAirControl();
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.x) > 0.9f)
                {
                    _playerData.IsTouchingWall = true;
                   
                    // сбросить горизонтальную скорость при контакте
                    _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);

                    _playerData.IsJumping = false;
                }
            }
        }
    }

}
