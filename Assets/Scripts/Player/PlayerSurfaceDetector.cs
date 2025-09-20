using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerSurfaceDetector : MonoBehaviour
{
    [SerializeField] private PlayerDataSO PlayerData;

    [Header("Ground Check")]
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private float GroundRadius = 0.2f;
    [SerializeField] private LayerMask GroundLayer;

    [Header("Wall Check")]
    [SerializeField] private LayerMask WallLayer;

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
        // �������� �����
        PlayerData.IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayer);
        if (PlayerData.IsGrounded)
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
        // ���� ����� ��������� �� �����
        if (((1 << collision.gameObject.layer) & WallLayer) != 0)
        {
            PlayerData.IsTouchingWall = false;
        }
    }

    private void DetectWall(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & WallLayer) != 0)
        {
            _playerAirControl.ResetAirControl();
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.x) > 0.9f)
                {
                    PlayerData.IsTouchingWall = true;
                   
                    // �������� �������������� �������� ��� ��������
                    _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);

                    PlayerData.IsJumping = false;
                }
            }
        }
    }

}
