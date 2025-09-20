using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerWallSlide : MonoBehaviour
{
    [SerializeField] private PlayerDataSO _playerData;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // �������� ������ ����:
        // 1. ����� �������� �����
        // 2. �� �� �����
        // 3. �� �������
        // 4. ������ ����
        if (_playerData.IsTouchingWall && !_playerData.IsGrounded && !_playerData.IsJumping && _rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity = new Vector2(0, -_playerData.WallSlideSpeed);
        }
    }
}
