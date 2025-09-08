using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerWallSlide : MonoBehaviour
{
    [SerializeField] private PlayerDataSO PlayerData;

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
        // 3. ������ ����
        if (PlayerData.IsTouchingWall && !PlayerData.IsGrounded && !PlayerData.IsJumping && _rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity = new Vector2(0, -PlayerData.WallSlideSpeed);
        }
    }
}
