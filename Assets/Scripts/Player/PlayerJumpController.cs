using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJumpController : MonoBehaviour
{
    [SerializeField] private PlayerDataSO _playerData; // ������ �� ScriptableObject
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _gravity;

    [SerializeField] private Vector2 _jumpTarget;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _gravity = Mathf.Abs(Physics2D.gravity.y * _rb.gravityScale);
    }

    private void Update()
    {
        // ������ ������ ���� ����� �� ����� ��� � ����� (������ ���� �� SO)
        if (Input.GetMouseButtonDown(0) && (_playerData.IsGrounded || _playerData.IsTouchingWall))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 target = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            Jump(target);
        }
    }

    private void FixedUpdate()
    {
        if (_playerData.IsJumping)
        {
            Vector2 currentPos = _rb.position;
            float distance = Vector2.Distance(currentPos, _jumpTarget);

            if (distance < 0.2f)
            {
                // �� ������ ��� �� ������
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x * 0.2f, _rb.linearVelocity.y * 0.01f);
                _playerData.IsJumping = false;
            }
        }

        // ��������� velocity � SO (����� ������ ������� ����� ���������� ���������)
        _playerData.CurrentVelocity = _rb.linearVelocity;
    }

    private void Jump(Vector2 target)
    {
        Vector2 startPos = _rb.position;
        Vector2 delta = target - startPos;

        delta.x = Mathf.Clamp(delta.x, -_playerData.MaxJumpX, _playerData.MaxJumpX);
        delta.y = Mathf.Clamp(delta.y, -_playerData.MaxJumpY, _playerData.MaxJumpY);

        float vx = delta.x / _playerData.JumpTime;
        float vy = delta.y / _playerData.JumpTime + 0.5f * _gravity * _playerData.JumpTime;

        _rb.linearVelocity = new Vector2(vx, vy);

        _jumpTarget = startPos + delta;
        _playerData.IsJumping = true;

        if (_animator != null)
            _animator.SetTrigger("Jump");
    }
}
