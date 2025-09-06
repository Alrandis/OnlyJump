using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerJumpController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float _jumpTime = 0.4f;   // ������������ ������
    [SerializeField] private float _maxJumpX = 8f;     // ������������ ��������� �� X
    [SerializeField] private float _maxJumpY = 4f;     // ������������ ������ �� Y

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;   // ����� ��� �������� �����
    [SerializeField] private float _groundRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;   // ���� ��������

    private Rigidbody2D _rb;
    private Animator _animator;
    private float _gravity;

    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _gravity = Mathf.Abs(Physics2D.gravity.y * _rb.gravityScale);
    }

    private void Update()
    {
        // ��������� ����� ������ ����
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundLayer);

        if (Input.GetMouseButtonDown(0) && _isGrounded) // ������ ������ ���� �� �����
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 target = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            TryJump(target);
        }
    }

    private void TryJump(Vector2 target)
    {
        Vector2 startPos = transform.position;
        Vector2 delta = target - startPos;

        // ������������ �� ��������
        delta.x = Mathf.Clamp(delta.x, -_maxJumpX, _maxJumpX);
        delta.y = Mathf.Clamp(delta.y, -_maxJumpY, _maxJumpY);

        // ������� ���������� � ������������� ��������
        float vx = delta.x / _jumpTime;
        float vy = delta.y / _jumpTime + 0.5f * _gravity * _jumpTime;

        _rb.linearVelocity = new Vector2(vx, vy);

        // ��������� ��������
        if (_animator != null)
            _animator.SetTrigger("Jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_animator != null && _isGrounded)
            _animator.SetTrigger("Ilde");
    }

    private void OnDrawGizmosSelected()
    {
        // ������ ������ GroundCheck � ���������
        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundRadius);
        }
    }
}
